using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.SystemSettings.DTOs;

namespace PhoneStoreAPI.Features.SystemSettings;

[ApiController]
[Route("api/thamso")]
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

    [HttpPut("{ma}")]
    [Authorize(Roles = AppConstants.Roles.Admin)]
    public async Task<IActionResult> Update(string ma, [FromBody] UpdateThamSoRequest request)
    {
        var updated = await _systemSettingService.UpdateAsync(ma, request);
        return Ok(updated);
    }
}
