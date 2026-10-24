using Microsoft.AspNetCore.Mvc;

namespace PhoneStoreAPI.Features.SystemSettings;

[ApiController]
[Route("api/[controller]")]
public class SystemSettingsController : ControllerBase
{
    private readonly ISystemSettingService _systemSettingService;

    public SystemSettingsController(ISystemSettingService systemSettingService)
    {
        _systemSettingService = systemSettingService;
    }
}
