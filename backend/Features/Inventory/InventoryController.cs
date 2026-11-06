using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Inventory.DTOs;

namespace PhoneStoreAPI.Features.Inventory;

[ApiController]
[Route("api/tonkho")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    /// <summary>
    /// P2-09: Xem danh sách tồn kho từ vw_TonKho kèm cờ cảnh báo
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetTonKho([FromQuery] TonKhoQueryParameters query)
    {
        var (items, totalCount) = await _inventoryService.GetTonKhoAsync(query);
        Response.Headers["X-Total-Count"] = totalCount.ToString();
        return Ok(items);
    }

    /// <summary>
    /// Xem tồn kho của một mặt hàng cụ thể
    /// </summary>
    [HttpGet("{maHang}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTonKhoByMa([FromRoute] string maHang)
    {
        var item = await _inventoryService.GetTonKhoByMaHangAsync(maHang);
        return Ok(item);
    }
}

[ApiController]
[Route("api/imei")]
public class ImeiController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public ImeiController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    /// <summary>
    /// P2-08: Lấy danh sách IMEI theo mặt hàng, trạng thái, từ khóa
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetImeis([FromQuery] ImeiQueryParameters query)
    {
        var (items, totalCount) = await _inventoryService.GetImeisAsync(query);
        Response.Headers["X-Total-Count"] = totalCount.ToString();
        return Ok(items);
    }

    /// <summary>
    /// P2-08: Lấy danh sách IMEI khả dụng (con_hang) theo thứ tự nhập trước xuất trước (FIFO)
    /// </summary>
    [HttpGet("available/{maHang}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAvailableImeis([FromRoute] string maHang)
    {
        var list = await _inventoryService.GetAvailableImeisAsync(maHang);
        return Ok(list);
    }
}
