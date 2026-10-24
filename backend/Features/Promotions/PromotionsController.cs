using Microsoft.AspNetCore.Mvc;

namespace PhoneStoreAPI.Features.Promotions;

[ApiController]
[Route("api/[controller]")]
public class PromotionsController : ControllerBase
{
    private readonly IPromotionService _promotionService;

    public PromotionsController(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }
}
