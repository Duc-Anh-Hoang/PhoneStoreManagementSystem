using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;

namespace PhoneStoreAPI.Features.OnlineOrders;

[ApiController]
[Route("api/shipping")]
public class ShippingWebhookController : ControllerBase
{
    private readonly IShippingService _shippingService;

    public ShippingWebhookController(IShippingService shippingService)
    {
        _shippingService = shippingService;
    }

    /// <summary>
    /// Giai đoạn 6 & Bước 5: Webhook nhận cập nhật trạng thái từ GHN.
    /// Hỗ trợ cả query param ?secret= và header X-Webhook-Secret.
    /// </summary>
    [HttpPost("ghn/webhook")]
    [HttpPost("/api/ghn/webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> GhnWebhook(
        [FromBody] GhnWebhookPayload payload,
        [FromQuery] string? secret = null,
        [FromHeader(Name = "X-Webhook-Secret")] string? webhookSecret = null)
    {
        var effectiveSecret = !string.IsNullOrWhiteSpace(secret) ? secret : webhookSecret;
        var ok = await _shippingService.HandleGhnWebhookAsync(payload, effectiveSecret);
        if (!ok)
        {
            return Unauthorized(new { message = "Webhook không hợp lệ." });
        }

        return Ok(new { message = "Đã cập nhật trạng thái vận chuyển." });
    }
}
