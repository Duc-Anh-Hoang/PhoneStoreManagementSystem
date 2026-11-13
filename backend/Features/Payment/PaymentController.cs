using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Features.Payment.DTOs;

namespace PhoneStoreAPI.Features.Payment;

[ApiController]
[Route("api/payment")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
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
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmPayment(string maDH, [FromBody] ConfirmPaymentRequest request)
    {
        var success = await _paymentService.ConfirmOrderPaymentAsync(maDH, request);
        return Ok(new { success, message = $"Đã xác nhận thanh toán cho đơn hàng {maDH} thành công." });
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
        var processed = await _paymentService.ProcessWebhookAsync(payload);
        return Ok(new { success = processed });
    }
}
