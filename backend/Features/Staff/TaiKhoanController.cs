using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Features.Staff.DTOs;

namespace PhoneStoreAPI.Features.Staff;

[ApiController]
[Route("api/taikhoan")]
[Authorize(Roles = AppConstants.Roles.Admin)]
public class TaiKhoanController : ControllerBase
{
    private readonly ITaiKhoanService _taiKhoanService;
    private readonly ICurrentUser _currentUser;

    public TaiKhoanController(ITaiKhoanService taiKhoanService, ICurrentUser currentUser)
    {
        _taiKhoanService = taiKhoanService;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Lấy danh sách tài khoản hệ thống (Chỉ Admin)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _taiKhoanService.GetAllAsync();
        return Ok(list);
    }

    /// <summary>
    /// Lấy chi tiết tài khoản theo username (Chỉ Admin)
    /// </summary>
    [HttpGet("{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var account = await _taiKhoanService.GetByUsernameAsync(username);
        return Ok(account);
    }

    /// <summary>
    /// Tạo tài khoản mới cho nhân viên (Chỉ Admin)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaiKhoanRequest request)
    {
        var result = await _taiKhoanService.CreateAsync(request);
        return CreatedAtAction(nameof(GetByUsername), new { username = result.Username }, result);
    }

    /// <summary>
    /// Đổi vai trò hoặc đặt lại mật khẩu cho tài khoản (Chỉ Admin)
    /// </summary>
    [HttpPut("{username}")]
    public async Task<IActionResult> Update(string username, [FromBody] UpdateTaiKhoanRequest request)
    {
        var result = await _taiKhoanService.UpdateAsync(username, request, _currentUser.UserName);
        return Ok(result);
    }

    /// <summary>
    /// Đổi trạng thái hoạt động / bị khóa của tài khoản (Chỉ Admin)
    /// URL: PUT /api/taikhoan/{username}/trangthai?trangThai=hoat_dong|bi_khoa
    /// </summary>
    [HttpPut("{username}/trangthai")]
    public async Task<IActionResult> UpdateStatus(string username, [FromQuery] string trangThai)
    {
        var result = await _taiKhoanService.UpdateStatusAsync(username, trangThai, _currentUser.UserName);
        return Ok(result);
    }
}

[ApiController]
[Route("api/phanquyen")]
[Authorize]
public class PhanQuyenController : ControllerBase
{
    private readonly ITaiKhoanService _taiKhoanService;

    public PhanQuyenController(ITaiKhoanService taiKhoanService)
    {
        _taiKhoanService = taiKhoanService;
    }

    /// <summary>
    /// Lấy danh sách các vai trò phân quyền trong hệ thống
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _taiKhoanService.GetAllRolesAsync();
        return Ok(list);
    }
}
