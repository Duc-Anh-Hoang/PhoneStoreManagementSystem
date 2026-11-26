using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Models.Entities;
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

    [HttpPost, Authorize(Roles = AppConstants.Roles.Admin)]
    public async Task<IActionResult> Create(PhanQuyenDto request, [FromServices] AppDbContext db)
    {
        var code = request.MaQuyen.Trim().ToLowerInvariant();
        var name = request.TenQuyen.Trim();
        if (!System.Text.RegularExpressions.Regex.IsMatch(code, "^[a-z][a-z0-9_]{1,9}$") ||
            name.Length is < 2 or > 50 || request.MoTa?.Length > 255 ||
            request.QuyenCoSo is not ("manager" or "sales" or "cashier" or "stock" or "tech") ||
            code is "admin" or "manager" or "sales" or "cashier" or "stock" or "tech" or "customer")
            return BadRequest(new { message = "Mã, tên, mô tả hoặc nhóm quyền không hợp lệ." });

        if (AppConstants.ValidPositions.Any(p => string.Equals(p, name, StringComparison.OrdinalIgnoreCase)) ||
            await db.PhanQuyens.AnyAsync(p => p.MaQuyen == code || p.TenQuyen.ToLower() == name.ToLower()))
            return Conflict(new { message = "Mã hoặc tên vai trò đã tồn tại." });

        db.PhanQuyens.Add(new PhanQuyen { MaQuyen = code, TenQuyen = name, QuyenCoSo = request.QuyenCoSo, MoTa = request.MoTa?.Trim() });
        await db.SaveChangesAsync();
        return Created("/api/phanquyen", new { maQuyen = code, tenQuyen = name });
    }

    [HttpPut("{code}"), Authorize(Roles = AppConstants.Roles.Admin)]
    public async Task<IActionResult> Update(string code, PhanQuyenDto request, [FromServices] AppDbContext db)
    {
        var role = await db.PhanQuyens.FindAsync(code);
        if (role == null) return NotFound();
        if (request.MoTa?.Length > 255) return BadRequest(new { message = "Mô tả tối đa 255 ký tự." });
        role.MoTa = request.MoTa?.Trim();
        await db.SaveChangesAsync();
        return Ok(new { role.MaQuyen, role.MoTa });
    }
}
