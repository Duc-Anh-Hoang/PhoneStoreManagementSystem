using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Payment.DTOs;
using QRCoder;
using System.Text.RegularExpressions;

namespace PhoneStoreAPI.Features.Payment;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService? _emailService;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        AppDbContext context,
        IConfiguration configuration,
        ILogger<PaymentService> logger,
        IEmailService? emailService = null)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _emailService = emailService;
    }

    public async Task<VietQrResponse> GenerateVietQrForOrderAsync(string maDH)
    {
        if (string.IsNullOrWhiteSpace(maDH))
        {
            throw new BadRequestException("Mã đơn hàng không hợp lệ.");
        }

        var donHang = await _context.DonHangOnlines.FirstOrDefaultAsync(d => d.MaDH == maDH.Trim());
        if (donHang == null)
        {
            throw new NotFoundException($"Không tìm thấy đơn hàng '{maDH}'.");
        }

        // Đọc cấu hình ngân hàng từ appsettings (hoặc fallback mặc định)
        string bankId = _configuration["VietQR:BankId"] ?? "MB";
        string bankBin = _configuration["VietQR:BankBin"] ?? "970422";
        string accountNumber = _configuration["VietQR:AccountNumber"] ?? "098765432100";
        string accountName = _configuration["VietQR:AccountName"] ?? "SAMAMOBILE STORE";
        string template = _configuration["VietQR:Template"] ?? "compact2";

        long amount = (long)Math.Round(donHang.TongTien);
        string memo = donHang.MaDH;

        // 1. Tạo Link ảnh VietQR chuẩn Napas 247
        string encodedAccountName = Uri.EscapeDataString(accountName);
        string encodedMemo = Uri.EscapeDataString(memo);
        string qrUrl = $"https://img.vietqr.io/image/{bankBin}-{accountNumber}-{template}.png?amount={amount}&addInfo={encodedMemo}&accountName={encodedAccountName}";

        // 2. Dùng thư viện QRCoder của .NET 10 để tự render mã QR Base64 PNG
        string qrBase64 = string.Empty;
        try
        {
            using var qrGenerator = new QRCodeGenerator();
            // Chuỗi dữ liệu thanh toán chuẩn VietQR / QuickLink
            string qrPayload = $"https://api.vietqr.io/{bankBin}/{accountNumber}/{amount}/{memo}/vietqr.jpg";
            using var qrCodeData = qrGenerator.CreateQrCode(qrPayload, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrBytes = qrCode.GetGraphic(20);
            qrBase64 = "data:image/png;base64," + Convert.ToBase64String(qrBytes);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Không thể sinh mã Base64 QR code bằng QRCoder.");
        }

        return new VietQrResponse
        {
            MaDH = donHang.MaDH,
            BankId = bankId,
            BankBin = bankBin,
            AccountNumber = accountNumber,
            AccountName = accountName,
            Amount = donHang.TongTien,
            Memo = memo,
            QrUrl = qrUrl,
            QrBase64 = qrBase64
        };
    }

    public async Task<bool> ConfirmOrderPaymentAsync(string maDH, ConfirmPaymentRequest request)
    {
        if (string.IsNullOrWhiteSpace(maDH))
        {
            throw new BadRequestException("Mã đơn hàng không hợp lệ.");
        }

        var donHang = await _context.DonHangOnlines.FirstOrDefaultAsync(d => d.MaDH == maDH.Trim());
        if (donHang == null)
        {
            throw new NotFoundException($"Không tìm thấy đơn hàng '{maDH}'.");
        }

        if (donHang.TrangThai == AppConstants.OrderStatus.DaHuy)
        {
            throw new BadRequestException("Đơn hàng đã bị hủy, không thể xác nhận thanh toán.");
        }

        // Đã thanh toán chuyển khoản thành công

        if (donHang.TrangThai == AppConstants.OrderStatus.ChoXacNhan)
        {
            donHang.TrangThai = AppConstants.OrderStatus.DaXacNhan;
        }

        await _context.SaveChangesAsync();

        // Gửi email xác nhận thanh toán nếu có email khách
        if (_emailService != null)
        {
            var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.MaKH == donHang.MaKH);
            if (kh != null && !string.IsNullOrWhiteSpace(kh.Email) && !kh.Email.EndsWith("@cellphones.customer"))
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        string body = $"<p>Xin chào <strong>{donHang.TenNguoiNhan}</strong>,</p>" +
                                      $"<p>Chúng tôi đã nhận được thanh toán <strong>{donHang.TongTien:N0} đ</strong> cho đơn hàng <strong>#{donHang.MaDH}</strong> qua VietQR.</p>" +
                                      $"<p>Đơn hàng của bạn đã được chuyển sang trạng thái <strong>Đã xác nhận</strong> và đang được đóng gói giao tới bạn.</p>";
                        await _emailService.SendEmailAsync(kh.Email, $"[Sama Mobile] Đã nhận thanh toán đơn hàng #{donHang.MaDH}", body);
                    }
                    catch { }
                });
            }
        }

        return true;
    }

    public async Task<bool> ProcessWebhookAsync(PaymentWebhookPayload payload)
    {
        if (payload == null || string.IsNullOrWhiteSpace(payload.Content))
        {
            return false;
        }

        // Tự động tìm mã đơn hàng dạng DHxxxxx trong nội dung chuyển khoản
        var match = Regex.Match(payload.Content, @"DH\d+", RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            return false;
        }

        string maDH = match.Value.ToUpper();
        var donHang = await _context.DonHangOnlines.FirstOrDefaultAsync(d => d.MaDH == maDH);
        if (donHang == null)
        {
            return false;
        }

        return await ConfirmOrderPaymentAsync(donHang.MaDH, new ConfirmPaymentRequest
        {
            TransactionId = payload.ReferenceCode ?? payload.Gateway,
            Amount = payload.TransferAmount,
            Note = $"Tự động xác thực qua Webhook {payload.Gateway}"
        });
    }
}
