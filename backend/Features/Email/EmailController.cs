using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Services;

namespace PhoneStoreAPI.Features.Email;

public class SendTestEmailRequest
{
    public string ToEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = "Thử nghiệm gửi email từ hệ thống Sama Mobile";
    public string? Content { get; set; }
}

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
    /// Kiểm tra kết nối và gửi email thử nghiệm (Dành riêng cho Quản trị viên)
    /// </summary>
    [HttpPost("test")]
    [Authorize(Roles = AppConstants.Roles.Admin)]
    public async Task<IActionResult> SendTestEmail([FromBody] SendTestEmailRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ToEmail))
        {
            return BadRequest(new { message = "Email người nhận không được để trống." });
        }

        string body = request.Content ?? "<p>Đây là email thử nghiệm kết nối dịch vụ SMTP của hệ thống Sama Mobile Store.</p>";
        bool success = await _emailService.SendEmailAsync(request.ToEmail, request.Subject, body);

        if (success)
        {
            return Ok(new { message = $"Email đã được xử lý gửi tới {request.ToEmail} thành công." });
        }

        return StatusCode(500, new { message = "Gửi email thất bại. Vui lòng kiểm tra lại cấu hình SMTP Server trong appsettings.json." });
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

        bool success = await _emailService.SendOtpEmailAsync(request.Email, otpCode, purpose);

        if (success)
        {
            return Ok(new
            {
                message = $"Mã OTP đã được gửi tới email {request.Email}.",
                otpCode // Trả về để tiện test môi trường dev
            });
        }

        return StatusCode(500, new { message = "Không thể gửi OTP qua email vào lúc này. Vui lòng thử lại sau." });
    }
}
