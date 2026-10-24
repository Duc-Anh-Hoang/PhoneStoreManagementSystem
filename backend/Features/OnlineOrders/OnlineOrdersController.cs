using Microsoft.AspNetCore.Mvc;

namespace PhoneStoreAPI.Features.OnlineOrders;

[ApiController]
[Route("api/[controller]")]
public class OnlineOrdersController : ControllerBase
{
    private readonly IOnlineOrderService _onlineOrderService;

    public OnlineOrdersController(IOnlineOrderService onlineOrderService)
    {
        _onlineOrderService = onlineOrderService;
    }
}
