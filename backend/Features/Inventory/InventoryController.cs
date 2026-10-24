using Microsoft.AspNetCore.Mvc;

namespace PhoneStoreAPI.Features.Inventory;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }
}
