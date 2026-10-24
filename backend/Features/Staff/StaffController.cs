using Microsoft.AspNetCore.Mvc;

namespace PhoneStoreAPI.Features.Staff;

[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }
}
