using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.OnlineOrders;
using PhoneStoreAPI.Features.OnlineOrders.Hubs;
using PhoneStoreAPI.Features.Payment.DTOs;
using System.Text.RegularExpressions;

namespace PhoneStoreAPI.Features.Payment;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService? _emailService;
    private readonly IHubContext<OrderHub>? _hubContext;
    private readonly ILogger<PaymentService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public PaymentService(
        AppDbContext context,
        IConfiguration configuration,
        ILogger<PaymentService> logger,
        IServiceProvider serviceProvider,
        IEmailService? emailService = null,
        IHubContext<OrderHub>? hubContext = null)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _serviceProvider = serviceProvider;
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

        if (donHang.TrangThai != AppConstants.OrderStatus.ChoThanhToan ||
            donHang.NgayDat.AddMinutes(15) <= DateTime.UtcNow)
        {
            throw new ConflictException("Đơn hàng không còn trong thời gian chờ quét mã.");
        }

        var bankId = _configuration["VietQR:BankId"];
        var bankBin = _configuration["VietQR:BankBin"];
        if (string.IsNullOrWhiteSpace(bankBin) && string.Equals(bankId, "MB", StringComparison.OrdinalIgnoreCase))
        {
            bankBin = "970422";
        }
        var accountNumber = _configuration["VietQR:AccountNumber"] ?? _configuration["VietQR:AccountNo"];
        var accountName = _configuration["VietQR:AccountName"];
        if (string.IsNullOrWhiteSpace(bankId) || string.IsNullOrWhiteSpace(bankBin) ||
            string.IsNullOrWhiteSpace(accountNumber) || string.IsNullOrWhiteSpace(accountName))
        {
            throw new ConflictException("Cửa hàng chưa cấu hình tài khoản nhận tiền.");
        }

        const string template = "compact2";

        long amount = (long)Math.Round(donHang.TongTien);
        string memo = donHang.MaDH;

        // 1. Tạo Link ảnh VietQR chuẩn Napas 247
        string encodedAccountName = Uri.EscapeDataString(accountName);
        string encodedMemo = Uri.EscapeDataString(memo);
        string qrUrl = $"https://img.vietqr.io/image/{bankBin}-{accountNumber}-{template}.png?amount={amount}&addInfo={encodedMemo}&accountName={encodedAccountName}";

        return new VietQrResponse
        {
            MaDH = donHang.MaDH,
            BankId = bankId,
            BankBin = bankBin,
            AccountNumber = accountNumber,
            AccountName = accountName,
            Amount = donHang.TongTien,
            Memo = memo,
            QrUrl = qrUrl
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

        var reference = request.TransactionId?.Trim().ToUpperInvariant();
        if (string.IsNullOrWhiteSpace(reference) || reference.Length > 100 || request.Amount is null or <= 0)
        {
            throw new BadRequestException("Cần nhập mã giao dịch và số tiền thực nhận.");
        }

        if (donHang.PaymentStatus == "paid" && request.Amount == donHang.TongTien &&
            await _context.OrderStatusLogs.AnyAsync(log =>
                log.OrderId == donHang.MaDH && log.Actor == "payment" && log.Reason == reference))
        {
            return true;
        }

        using var scope = _serviceProvider.CreateScope();
        var onlineOrderService = scope.ServiceProvider.GetRequiredService<IOnlineOrderService>();
        var order = await onlineOrderService.UpdateOrderStatusAsync(donHang.MaDH, AppConstants.OrderStatus.DaXacNhan,
            confirmedAmount: request.Amount, paymentReference: reference);

        // Gửi thông báo Realtime qua SignalR tới cả trang khách hàng đang mở mã QR và Dashboard Admin
        if (_hubContext != null)
        {
            var paymentPayload = new
            {
                maDH = donHang.MaDH,
                trangThai = order.TrangThai,
                isPaid = true,
                daThanhToan = true,
                amount = donHang.TongTien,
                transactionId = reference,
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
        if (payload == null || string.IsNullOrWhiteSpace(payload.Content) ||
            string.IsNullOrWhiteSpace(payload.ReferenceCode) || payload.TransferAmount is null or <= 0 ||
            !string.Equals(payload.TransferType, "in", StringComparison.OrdinalIgnoreCase))
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
            d.MaDH == matchedCode || d.MaDH.Replace("-", "") == matchedCode);

        if (donHang == null)
        {
            return false;
        }

        return await ConfirmOrderPaymentAsync(donHang.MaDH, new ConfirmPaymentRequest
        {
            TransactionId = payload.ReferenceCode,
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

        bool isPaid = donHang.PaymentStatus == "paid";

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
