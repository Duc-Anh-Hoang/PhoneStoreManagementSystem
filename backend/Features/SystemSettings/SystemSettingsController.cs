using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.SystemSettings.DTOs;

namespace PhoneStoreAPI.Features.SystemSettings;

[ApiController]
[Route("api/settings")]
public class SystemSettingsController : ControllerBase
{
    private readonly ISystemSettingService _systemSettingService;

    public SystemSettingsController(ISystemSettingService systemSettingService)
    {
        _systemSettingService = systemSettingService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var list = await _systemSettingService.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{ma}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByMa(string ma)
    {
        var item = await _systemSettingService.GetByMaAsync(ma);
        return Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = AppConstants.Roles.Admin)]
    public async Task<IActionResult> Create(CreateThamSoRequest request, [FromServices] AppDbContext db)
    {
        var code = request.MaThamSo?.Trim().ToUpperInvariant() ?? "";
        var name = request.TenThamSo?.Trim() ?? "";
        var unit = request.DonVi?.Trim();
        if (!System.Text.RegularExpressions.Regex.IsMatch(code, "^[A-Z][A-Z0-9_]{0,29}$") ||
            name.Length is < 1 or > 100 || unit?.Length > 20 ||
            request.GiaTri < 0 || request.GiaTri > 9999999999999999.99m || decimal.Round(request.GiaTri, 2) != request.GiaTri)
            return BadRequest(new { message = "Mã, tên, đơn vị hoặc giá trị tham số không hợp lệ." });
        if (await db.ThamSos.AnyAsync(t => t.MaThamSo == code))
            return Conflict(new { message = "Mã tham số đã tồn tại." });
        db.ThamSos.Add(new ThamSo { MaThamSo = code, TenThamSo = name, DonVi = unit, GiaTri = request.GiaTri });
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetByMa), new { ma = code }, new { maThamSo = code });
    }

    [HttpPut("{ma}")]
    [Authorize(Roles = AppConstants.Roles.Admin)]
    public async Task<IActionResult> Update(string ma, [FromBody] UpdateThamSoRequest request)
    {
        var updated = await _systemSettingService.UpdateAsync(ma, request);
        return Ok(updated);
    }
}
