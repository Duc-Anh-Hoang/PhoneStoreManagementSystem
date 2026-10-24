using Microsoft.AspNetCore.Mvc;

namespace PhoneStoreAPI.Features.Warranty;

[ApiController]
[Route("api/[controller]")]
public class WarrantyController : ControllerBase
{
    private readonly IWarrantyService _warrantyService;

    public WarrantyController(IWarrantyService warrantyService)
    {
        _warrantyService = warrantyService;
    }
}
