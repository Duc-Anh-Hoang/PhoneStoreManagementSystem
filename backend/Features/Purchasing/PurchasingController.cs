using Microsoft.AspNetCore.Mvc;

namespace PhoneStoreAPI.Features.Purchasing;

[ApiController]
[Route("api/[controller]")]
public class PurchasingController : ControllerBase
{
    private readonly IPurchasingService _purchasingService;

    public PurchasingController(IPurchasingService purchasingService)
    {
        _purchasingService = purchasingService;
    }
}
