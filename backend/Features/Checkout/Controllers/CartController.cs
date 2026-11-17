using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Features.Checkout.Services;

namespace PhoneStoreAPI.Features.Checkout.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("checkout")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly ICurrentUser _currentUser;

    public CartController(ICartService cartService, ICurrentUser currentUser)
    {
        _cartService = cartService;
        _currentUser = currentUser;
    }

    private string? GetCartToken()
    {
        if (Request.Headers.TryGetValue("X-Cart-Token", out var tokenValues))
        {
            var token = tokenValues.ToString();
            if (!string.IsNullOrWhiteSpace(token))
            {
                return token.Trim();
            }
        }
        return null;
    }

    private string? GetUserId()
    {
        return _currentUser.IsAuthenticated ? _currentUser.UserName : null;
    }

    [HttpGet]
    public async Task<ActionResult<CartDTO>> GetCart()
    {
        var cart = await _cartService.GetCartAsync(GetCartToken(), GetUserId());
        Response.Headers["X-Cart-Token"] = cart.CartToken;
        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<ActionResult<CartDTO>> AddItem([FromBody] AddCartItemRequest request)
    {
        var cart = await _cartService.AddItemAsync(GetCartToken(), GetUserId(), request);
        Response.Headers["X-Cart-Token"] = cart.CartToken;
        return Ok(cart);
    }

    [HttpPut("items/{variantId}")]
    public async Task<ActionResult<CartDTO>> UpdateItem(long variantId, [FromBody] UpdateCartItemRequest request)
    {
        var cart = await _cartService.UpdateItemAsync(GetCartToken(), GetUserId(), variantId, request.Qty);
        Response.Headers["X-Cart-Token"] = cart.CartToken;
        return Ok(cart);
    }

    [HttpDelete("items/{variantId}")]
    public async Task<ActionResult<CartDTO>> RemoveItem(long variantId)
    {
        var cart = await _cartService.RemoveItemAsync(GetCartToken(), GetUserId(), variantId);
        Response.Headers["X-Cart-Token"] = cart.CartToken;
        return Ok(cart);
    }

    [HttpPost("merge")]
    public async Task<ActionResult<CartDTO>> MergeCart([FromBody] MergeCartRequest request)
    {
        var userId = GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest(new { message = "Vui lòng đăng nhập để đồng bộ giỏ hàng." });
        }

        var cart = await _cartService.MergeCartAsync(request.GuestCartToken, userId);
        Response.Headers["X-Cart-Token"] = cart.CartToken;
        return Ok(cart);
    }
}
