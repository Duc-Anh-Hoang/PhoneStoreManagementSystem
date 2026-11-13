using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.OnlineOrders.Hubs;
using PhoneStoreAPI.Features.Payment.DTOs;
using QRCoder;
using System.Text.RegularExpressions;

namespace PhoneStoreAPI.Features.Payment;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService? _emailService;
    private readonly IHubContext<OrderHub>? _hubContext;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        AppDbContext context,
        IConfiguration configuration,
        ILogger<PaymentService> logger,
        IEmailService? emailService = null,
        IHubContext<OrderHub>? hubContext = null)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _emailService = emailService;
        _hubContext = hubContext;
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

        // Đã thanh toán chuyển khoản thành công -> Chuyển về Chờ xác nhận để chờ Admin duyệt
        if (donHang.TrangThai == AppConstants.OrderStatus.ChoThanhToan)
        {
            donHang.TrangThai = AppConstants.OrderStatus.ChoXacNhan;
        }

        await _context.SaveChangesAsync();

        // Gửi thông báo Realtime qua SignalR tới cả trang khách hàng đang mở mã QR và Dashboard Admin
        if (_hubContext != null)
        {
            var paymentPayload = new
            {
                maDH = donHang.MaDH,
                trangThai = donHang.TrangThai,
                isPaid = true,
                daThanhToan = true,
                amount = donHang.TongTien,
                transactionId = request.TransactionId ?? $"PAY-{DateTime.UtcNow:yyyyMMddHHmmss}",
                message = $"Đơn hàng #{donHang.MaDH} đã thanh toán thành công {donHang.TongTien:N0}đ qua VietQR/Ngân hàng!",
                sdt = donHang.SdtNguoiNhan,
                updatedAt = DateTime.UtcNow
            };

            _ = Task.Run(async () =>
            {
                try
                {
                    await _hubContext.Clients.Group($"Order_{donHang.MaDH.Trim()}").SendAsync("PaymentSuccess", paymentPayload);
                    await _hubContext.Clients.Group(OrderHub.AdminGroup).SendAsync("ReceiveOrderPaid", paymentPayload);
                    await _hubContext.Clients.All.SendAsync("OrderStatusUpdated", paymentPayload);
                }
                catch { }
            });
        }

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

        // Tự động tìm mã đơn hàng dạng DH-xxxxxx, DHxxxx hoặc SAMAxxxx trong nội dung chuyển khoản
        string? matchedCode = null;
        var matchDH = Regex.Match(payload.Content, @"DH-?[A-Z0-9]+", RegexOptions.IgnoreCase);
        if (matchDH.Success)
        {
            matchedCode = matchDH.Value.ToUpper();
        }
        else
        {
            var matchSama = Regex.Match(payload.Content, @"SAMA\s*([0-9]+)", RegexOptions.IgnoreCase);
            if (matchSama.Success)
            {
                matchedCode = matchSama.Groups[1].Value;
            }
        }

        if (string.IsNullOrWhiteSpace(matchedCode))
        {
            return false;
        }

        var donHang = await _context.DonHangOnlines.FirstOrDefaultAsync(d =>
            d.MaDH == matchedCode ||
            d.MaDH.Replace("-", "") == matchedCode ||
            (matchedCode.Length >= 4 && d.MaDH.Contains(matchedCode)));

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

    public async Task<PaymentStatusResponse> GetPaymentStatusAsync(string maDH)
    {
        if (string.IsNullOrWhiteSpace(maDH))
        {
            throw new BadRequestException("Mã đơn hàng không hợp lệ.");
        }

        var clean = maDH.Trim();
        var donHang = await _context.DonHangOnlines.FirstOrDefaultAsync(d => d.MaDH == clean);
        if (donHang == null)
        {
            throw new NotFoundException($"Không tìm thấy đơn hàng '{maDH}'.");
        }

        // Với đơn online: khi thoát khỏi "cho_thanh_toan" và không bị "da_huy", nghĩa là đã thanh toán
        bool isOnline = donHang.PhuongThucThanhToan == "chuyen_khoan" || donHang.PhuongThucThanhToan == "the";
        bool isPaid = isOnline
            ? (donHang.TrangThai != AppConstants.OrderStatus.ChoThanhToan && donHang.TrangThai != AppConstants.OrderStatus.DaHuy)
            : (donHang.TrangThai == AppConstants.OrderStatus.HoanThanh);

        return new PaymentStatusResponse
        {
            MaDH = donHang.MaDH,
            TrangThai = donHang.TrangThai,
            DaThanhToan = isPaid,
            IsPaid = isPaid,
            TongTien = donHang.TongTien,
            PhuongThucThanhToan = donHang.PhuongThucThanhToan,
            NgayDat = donHang.NgayDat
        };
    }
}
