using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.PaymentProviders;
using PhoneStoreAPI.Features.Checkout.Services;

namespace PhoneStoreAPI.Features.Checkout.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentWebhookHandler _webhookHandler;
    private readonly IPaymentProvider _paymentProvider;
    private readonly AppDbContext _context;

    public PaymentsController(
        IPaymentWebhookHandler webhookHandler,
        IPaymentProvider paymentProvider,
        AppDbContext context)
    {
        _webhookHandler = webhookHandler;
        _paymentProvider = paymentProvider;
        _context = context;
    }

    [HttpPost("webhook/{provider}")]
    public async Task<IActionResult> HandleWebhook(string provider)
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var rawPayload = await reader.ReadToEndAsync();

        string? signature = null;
        if (Request.Headers.TryGetValue("X-Signature", out var sigValues))
        {
            signature = sigValues.ToString();
        }

        var result = await _webhookHandler.ProcessQrWebhookAsync(provider, rawPayload, signature);
        return StatusCode(result.StatusCode, new { message = result.Message, orderStatus = result.FinalOrderStatus });
    }

    [HttpPost("create-qr")]
    public async Task<IActionResult> CreateQrPayment([FromBody] CreateQrPaymentRequest request)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderCode == request.OrderCode);
        if (order == null)
        {
            return NotFound(new { message = $"Không tìm thấy đơn hàng {request.OrderCode}." });
        }

        var paymentResult = await _paymentProvider.CreatePaymentAsync(order);
        return Ok(paymentResult);
    }

    [HttpPost("confirm-cod")]
    public async Task<IActionResult> ConfirmCodOrder([FromBody] ConfirmCodRequest request)
    {
        var result = await _webhookHandler.ConfirmCodOrderAsync(request.OrderCode, request.Otp);
        if (!result.Success)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }
        return Ok(new { message = "Xác nhận đơn hàng COD thành công." });
    }
}

public class CreateQrPaymentRequest
{
    public string OrderCode { get; set; } = string.Empty;
}

public class ConfirmCodRequest
{
    public string OrderCode { get; set; } = string.Empty;
    public string? Otp { get; set; }
}
