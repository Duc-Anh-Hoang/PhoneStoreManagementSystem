using System.Security.Claims;
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
    /// P4-06: Khách tra cứu chi tiết đơn hàng theo mã đơn và SĐT (thông tin đã mask).
    /// Trả lỗi chung khi không tìm thấy để tránh lộ thông tin sai mã/sai SĐT.
    /// </summary>
    [HttpGet("{maDH}")]
    [AllowAnonymous]
    [EnableRateLimiting("public-lookup")]
    public async Task<IActionResult> GetOrderByMaDH(string maDH, [FromQuery] string? sdt, [FromQuery] string? token = null)
    {
        string? lookupPhone = sdt;

        // Hỗ trợ tra cứu chi tiết từ luồng OTP bằng token ngắn hạn (Giai đoạn 4)
        if (!string.IsNullOrWhiteSpace(token))
        {
            var tokenPhone = OtpTokenValidator.ValidateAndExtractPhone(token, HttpContext.RequestServices);
            if (tokenPhone == null)
            {
                return Unauthorized(new { message = "Phiên tra cứu OTP không hợp lệ hoặc đã hết hạn." });
            }
            lookupPhone = tokenPhone;
        }

        // Yêu cầu SĐT bắt buộc (hoặc OTP token hợp lệ) để xác thực quyền tra cứu (bảo mật P4-06)
        if (string.IsNullOrWhiteSpace(lookupPhone))
        {
            return BadRequest(new { message = "Vui lòng cung cấp số điện thoại để tra cứu." });
        }

        var normalizedCode = maDH.Trim().ToUpperInvariant();
        var result = await _onlineOrderService.GetOrderForLookupAsync(normalizedCode, lookupPhone);
        if (result == null)
        {
            // Trả lỗi chung — không tiết lộ sai mã hay sai SĐT (bảo mật)
            return NotFound(new { message = "Không tìm thấy đơn hàng phù hợp." });
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
    /// Giai đoạn 4: Tra cứu danh sách đơn theo SĐT đã xác thực OTP.
    /// Yêu cầu JWT short-lived có claim "phone_lookup" do OTP service phát.
    /// </summary>
    [HttpGet("by-phone")]
    [AllowAnonymous] // Tự xác thực JWT OTP token trong action (không dùng [Authorize] vì scheme khác)
    public async Task<IActionResult> GetOrdersByOtpToken([FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return BadRequest(new { message = "Token không hợp lệ." });

        // Xác thực JWT OTP token và trích xuất số điện thoại từ claim "phone_lookup"
        var phone = OtpTokenValidator.ValidateAndExtractPhone(token, HttpContext.RequestServices);
        if (phone == null)
        {
            // Bảo mật: không tiết lộ lý do token không hợp lệ
            return Unauthorized(new { message = "Token không hợp lệ hoặc đã hết hạn." });
        }

        var result = await _onlineOrderService.GetOrdersByPhoneMaskedAsync(phone);
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
