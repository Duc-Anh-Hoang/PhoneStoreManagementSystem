using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Features.Checkout.Services;

namespace PhoneStoreAPI.Features.Checkout.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("checkout")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IRefundAndCancellationService _cancellationService;
    private readonly ICurrentUser _currentUser;

    public OrdersController(
        IOrderService orderService,
        IRefundAndCancellationService cancellationService,
        ICurrentUser currentUser)
    {
        _orderService = orderService;
        _cancellationService = cancellationService;
        _currentUser = currentUser;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        if (!Request.Headers.TryGetValue("Idempotency-Key", out var keyValues) || string.IsNullOrWhiteSpace(keyValues.ToString()))
        {
            return BadRequest(new { message = "Header Idempotency-Key là bắt buộc để chống tạo đơn trùng lặp." });
        }

        var idempotencyKey = keyValues.ToString().Trim();
        var userId = _currentUser.IsAuthenticated ? _currentUser.UserName : null;

        try
        {
            var order = await _orderService.CreateOrderAsync(idempotencyKey, request, userId);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{orderCode}")]
    public async Task<ActionResult<OrderDTO>> GetOrderByCode(string orderCode)
    {
        var order = await _orderService.GetOrderByCodeAsync(orderCode);
        if (order == null)
        {
            return NotFound(new { message = $"Không tìm thấy đơn hàng {orderCode}." });
        }
        return Ok(order);
    }

    [HttpPost("{orderCode}/cancel")]
    public async Task<IActionResult> CancelOrder(string orderCode, [FromBody] CancelOrderRequest request)
    {
        var result = await _cancellationService.CancelOrderAsync(orderCode, request.Reason ?? "Khách hàng yêu cầu hủy đơn");
        if (!result.Success)
        {
            return BadRequest(new { message = result.Message, status = result.FinalStatus });
        }
        return Ok(new { message = result.Message, status = result.FinalStatus });
    }
}

public class CancelOrderRequest
{
    public string? Reason { get; set; }
}
