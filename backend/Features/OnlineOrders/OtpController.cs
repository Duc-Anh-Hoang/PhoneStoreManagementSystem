using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;

namespace PhoneStoreAPI.Features.OnlineOrders;

[ApiController]
[Route("api/otp")]
public class OtpController : ControllerBase
{
    private readonly IOtpService _otpService;
    private readonly ILogger<OtpController> _logger;

    public OtpController(IOtpService otpService, ILogger<OtpController> logger)
    {
        _otpService = otpService;
        _logger = logger;
    }

    /// <summary>
    /// Giai đoạn 4: Yêu cầu gửi OTP đến SĐT.
    /// Rate-limited: 3 req/phút/IP (policy "otp").
    /// Luôn trả cùng message để tránh lộ SĐT có tồn tại hay không (bảo mật).
    /// </summary>
    [HttpPost("request")]
    [AllowAnonymous]
    [EnableRateLimiting("otp")]
    public async Task<IActionResult> RequestOtp([FromBody] OtpRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Sdt))
            return BadRequest(new { message = "Số điện thoại không được để trống." });

        var res = await _otpService.RequestOtpAsync(dto.Sdt);
        if (res.Blocked)
            return StatusCode(StatusCodes.Status429TooManyRequests, new { message = res.Message });

        return Ok(new { message = res.Message, code = res.Code });
    }

    /// <summary>
    /// Giai đoạn 4: Xác minh OTP. Nếu đúng trả JWT token (10 phút) để tra cứu đơn hàng.
    /// Rate-limited: 3 req/phút/IP (policy "otp").
    /// </summary>
    [HttpPost("verify")]
    [AllowAnonymous]
    [EnableRateLimiting("otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Sdt) || string.IsNullOrWhiteSpace(dto.Otp))
            return BadRequest(new { message = "Số điện thoại và mã OTP không được để trống." });

        var result = await _otpService.VerifyOtpAsync(dto.Sdt, dto.Otp);

        if (!result.Success)
        {
            // Trả 400 với lý do từ service (không lộ thông tin hệ thống)
            return BadRequest(new { message = result.Error });
        }

        return Ok(new OtpVerifyResponse
        {
            Token = result.Token!,
            ExpiresAt = result.ExpiresAt!.Value
        });
    }
}
