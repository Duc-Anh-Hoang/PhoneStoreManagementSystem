using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;

namespace PhoneStoreAPI.Features.OnlineOrders;

[ApiController]
[Route("api/orders")]
public class OnlineOrdersController : ControllerBase
{
    private readonly IOnlineOrderService _onlineOrderService;

    public OnlineOrdersController(IOnlineOrderService onlineOrderService)
    {
        _onlineOrderService = onlineOrderService;
    }

    /// <summary>
    /// P4-05: Đặt hàng công khai từ giao diện khách
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var result = await _onlineOrderService.CreateOrderAsync(request);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// P4-06: Khách tra cứu chi tiết đơn hàng theo mã đơn và SĐT
    /// </summary>
    [HttpGet("{maDH}")]
    [AllowAnonymous]
    [EnableRateLimiting("public-lookup")]
    public async Task<IActionResult> GetOrderByMaDH(string maDH, [FromQuery] string? sdt)
    {
        var result = await _onlineOrderService.GetOrderByMaDHAsync(maDH, sdt);
        if (result == null)
        {
            // Trả 404 để chống dò mã đơn
            return NotFound(new { message = $"Không tìm thấy đơn hàng {maDH}" });
        }
        return Ok(result);
    }

    /// <summary>
    /// P4-06: Khách hoặc nhân viên tra cứu danh sách đơn theo SĐT
    /// </summary>
    [HttpGet("phone/{sdt}")]
    [AllowAnonymous]
    [EnableRateLimiting("public-lookup")]
    public async Task<IActionResult> GetOrdersByPhone(string sdt)
    {
        var result = await _onlineOrderService.GetOrdersByPhoneAsync(sdt);
        return Ok(result);
    }

    /// <summary>
    /// Khách hàng tự hủy đơn hàng trực tuyến khi chưa giao
    /// </summary>
    [HttpPut("{maDH}/cancel")]
    [HttpPost("{maDH}/cancel")]
    [AllowAnonymous]
    public async Task<IActionResult> CancelOrder(string maDH, [FromBody] CancelOrderRequest request)
    {
        var result = await _onlineOrderService.CancelOrderByCustomerAsync(maDH, request);
        return Ok(result);
    }

}
