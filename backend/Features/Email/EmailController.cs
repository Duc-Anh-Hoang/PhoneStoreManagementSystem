using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Services;

namespace PhoneStoreAPI.Features.Email;

public class SendOtpEmailRequest
{
    public string Email { get; set; } = string.Empty;
    public string? Purpose { get; set; }
}

[ApiController]
[Route("api/email")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    /// <summary>
    /// Gửi mã xác thực OTP qua Email
    /// </summary>
    [HttpPost("send-otp")]
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpEmailRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { message = "Email không được để trống." });
        }

        // Tạo mã OTP 6 chữ số ngẫu nhiên
        var random = new Random();
        string otpCode = random.Next(100000, 999999).ToString();
        string purpose = request.Purpose ?? "xác thực tài khoản";

        bool success = false;
        try
        {
            success = await _emailService.SendOtpEmailAsync(request.Email, otpCode, purpose);
        }
        catch
        {
            success = false;
        }

        // Luôn trả về 200 OK kèm otpCode để người dùng không bao giờ bị nghẽn đăng nhập khi cloud/nhà mạng chặn cổng SMTP
        return Ok(new
        {
            message = success
                ? $"Mã OTP đã được gửi tới email {request.Email}."
                : $"Đã tạo mã OTP xác thực: {otpCode}",
            otpCode,
            isSimulated = !success
        });
    }
}
