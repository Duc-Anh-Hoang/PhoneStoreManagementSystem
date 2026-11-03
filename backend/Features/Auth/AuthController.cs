using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Features.Auth.DTOs;

namespace PhoneStoreAPI.Features.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICurrentUser _currentUser;

    public AuthController(IAuthService authService, ICurrentUser currentUser)
    {
        _authService = authService;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Đăng nhập nhân viên & quản trị viên
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginStaffAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Đăng nhập khách hàng bằng số điện thoại
    /// </summary>
    [HttpPost("customer-login")]
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> CustomerLogin([FromBody] CustomerLoginRequest request)
    {
        var result = await _authService.LoginCustomerAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Đăng xuất khỏi hệ thống
    /// </summary>
    [HttpPost("logout")]
    [AllowAnonymous]
    public IActionResult Logout()
    {
        return Ok(new { message = "Đăng xuất thành công." });
    }

    /// <summary>
    /// Lấy thông tin tài khoản hiện tại từ JWT
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            username = _currentUser.UserName,
            maNV = _currentUser.MaNV,
            role = _currentUser.Role,
            maKH = _currentUser.MaKH,
            isAuthenticated = _currentUser.IsAuthenticated
        });
    }
}
