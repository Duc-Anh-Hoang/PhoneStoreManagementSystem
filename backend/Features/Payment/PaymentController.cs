using System.Security.Cryptography;
using System.Text;
using PhoneStoreAPI.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Features.Payment.DTOs;

namespace PhoneStoreAPI.Features.Payment;

[ApiController]
[Route("api/payment")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public PaymentController(IPaymentService paymentService, IConfiguration configuration, IWebHostEnvironment environment)
    {
        _paymentService = paymentService;
        _configuration = configuration;
        _environment = environment;
    }

    /// <summary>
    /// Sinh mã VietQR và ảnh QR Base64 trực tiếp cho đơn hàng
    /// </summary>
    [HttpGet("vietqr/{maDH}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVietQr(string maDH)
    {
        var result = await _paymentService.GenerateVietQrForOrderAsync(maDH);
        return Ok(result);
    }

    /// <summary>
    /// Khách hàng hoặc thu ngân xác nhận đã thanh toán chuyển khoản
    /// </summary>
    [HttpPost("confirm/{maDH}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Cashier}")]
    public async Task<IActionResult> ConfirmPayment(string maDH, [FromBody] ConfirmPaymentRequest request)
    {
        var success = await _paymentService.ConfirmOrderPaymentAsync(maDH, request);
        return Ok(new { success, message = $"Đã xác nhận thanh toán cho đơn hàng {maDH} thành công." });
    }

    /// <summary>
    /// Xác nhận thanh toán nhanh ở chế độ Demo (chỉ hoạt động khi bật Payment:DemoAutoConfirm)
    /// </summary>
    [HttpPost("demo-confirm/{maDH}")]
    [AllowAnonymous]
    public async Task<IActionResult> DemoConfirmPayment(string maDH)
    {
        var demoEnabled = _environment.IsDevelopment() && _configuration.GetValue<bool>("Payment:DemoAutoConfirm");
        if (!demoEnabled)
        {
            return StatusCode(403, new { message = "Chức năng xác nhận demo đang tắt." });
        }

        var status = await _paymentService.GetPaymentStatusAsync(maDH);
        if (status.IsPaid)
        {
            return Ok(new { success = true, isPaid = true, tongTien = status.TongTien, trangThai = status.TrangThai });
        }
        if (status.TrangThai != AppConstants.OrderStatus.ChoThanhToan ||
            status.NgayDat is not DateTime createdAt || createdAt.AddMinutes(15) <= DateTime.UtcNow)
            return Conflict(new { message = "Đơn hàng không còn chờ thanh toán." });

        await _paymentService.ConfirmOrderPaymentAsync(maDH, new ConfirmPaymentRequest
        {
            TransactionId = $"DEMO_{maDH}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}",
            Amount = status.TongTien
        });

        var updated = await _paymentService.GetPaymentStatusAsync(maDH);
        return Ok(new { success = true, isPaid = updated.IsPaid, tongTien = updated.TongTien, trangThai = updated.TrangThai });
    }

    /// <summary>
    /// Kiểm tra trạng thái thanh toán đơn hàng (phục vụ Realtime polling phía Client)
    /// </summary>
    [HttpGet("status/{maDH}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetStatus(string maDH)
    {
        var status = await _paymentService.GetPaymentStatusAsync(maDH);
        return Ok(status);
    }

    /// <summary>
    /// Webhook tự động nhận thông báo từ ngân hàng / cổng thanh toán (SePay, Casso,...)
    /// </summary>
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> Webhook([FromBody] PaymentWebhookPayload payload)
    {
        var secret = _configuration["Payment:WebhookSecret"];
        var authorization = Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(secret) || !CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(authorization), Encoding.UTF8.GetBytes($"Apikey {secret}")))
        {
            return Unauthorized();
        }

        var processed = await _paymentService.ProcessWebhookAsync(payload);
        return Ok(new { success = processed });
    }
}
