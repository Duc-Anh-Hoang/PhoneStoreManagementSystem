using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Staff.DTOs;

namespace PhoneStoreAPI.Features.Staff;

[ApiController]
[Route("api/nhanvien")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    /// <summary>
    /// Lấy danh sách nhân viên (kèm phân trang, lọc chức vụ, từ khóa)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager}")]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? keyword,
        [FromQuery] string? chucVu,
        [FromQuery] string? trangThai,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, totalCount) = await _staffService.GetAllAsync(keyword, chucVu, trangThai, page, pageSize);
        Response.Headers.Append("X-Total-Count", totalCount.ToString());
        return Ok(items);
    }

    /// <summary>
    /// Lấy thông tin chi tiết nhân viên theo mã
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager}")]
    public async Task<IActionResult> GetById(string id)
    {
        var staff = await _staffService.GetByIdAsync(id);
        return Ok(staff);
    }

    /// <summary>
    /// Tạo mới nhân viên (Chỉ Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = AppConstants.Roles.Admin)]
    public async Task<IActionResult> Create([FromBody] CreateNhanVienRequest request)
    {
        var result = await _staffService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.MaNV }, result);
    }

    /// <summary>
    /// Cập nhật thông tin nhân viên (Chỉ Admin)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = AppConstants.Roles.Admin)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateNhanVienRequest request)
    {
        var result = await _staffService.UpdateAsync(id, request);
        return Ok(result);
    }

    /// <summary>
    /// Xóa hoặc chuyển trạng thái nhân viên nếu đã có chứng từ (Chỉ Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = AppConstants.Roles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var (isDeleted, message) = await _staffService.DeleteAsync(id);
        return Ok(new { isDeleted, message });
    }
}
