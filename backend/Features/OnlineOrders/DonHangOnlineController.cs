using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;

namespace PhoneStoreAPI.Features.OnlineOrders;

[ApiController]
[Route("api/donhangonline")]
[Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales}")]
public class DonHangOnlineController : ControllerBase
{
    private readonly IOnlineOrderService _onlineOrderService;

    public DonHangOnlineController(IOnlineOrderService onlineOrderService)
    {
        _onlineOrderService = onlineOrderService;
    }

    /// <summary>
    /// P4-07: Lấy danh sách đơn hàng online cho quản trị
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetOrders(
        [FromQuery] string? trangThai,
        [FromQuery] DateOnly? tuNgay,
        [FromQuery] DateOnly? denNgay,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, total) = await _onlineOrderService.GetAdminOrdersAsync(trangThai, tuNgay, denNgay, page, pageSize);
        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    /// <summary>
    /// P4-07: Cập nhật trạng thái đơn hàng online
    /// </summary>
    [HttpPut("{maDH}/trangthai")]
    public async Task<IActionResult> UpdateStatus(string maDH, [FromBody] UpdateOrderStatusRequest request)
    {
        var result = await _onlineOrderService.UpdateOrderStatusAsync(maDH, request.TrangThai, request.SoHD);
        return Ok(result);
    }
}
