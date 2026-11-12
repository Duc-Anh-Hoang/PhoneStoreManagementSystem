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
    /// Giai đoạn 6: Webhook nhận cập nhật trạng thái từ GHN.
    /// Xác thực nguồn gọi qua header X-Webhook-Secret (nếu cấu hình) và ShopID.
    /// </summary>
    [HttpPost("ghn/webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> GhnWebhook(
        [FromBody] GhnWebhookPayload payload,
        [FromHeader(Name = "X-Webhook-Secret")] string? webhookSecret = null)
    {
        var ok = await _shippingService.HandleGhnWebhookAsync(payload, webhookSecret);
        if (!ok)
        {
            return Unauthorized(new { message = "Webhook không hợp lệ." });
        }

        return Ok(new { message = "Đã cập nhật trạng thái vận chuyển." });
    }
}
