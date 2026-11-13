using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;

namespace PhoneStoreAPI.Features.OnlineOrders;

[ApiController]
[Route("api/donhangonline")]
[AllowAnonymous]
public class DonHangOnlineController : ControllerBase
{
    private readonly IOnlineOrderService _onlineOrderService;
    private readonly IShippingService _shippingService;
    private readonly AppDbContext _context;

    public DonHangOnlineController(
        IOnlineOrderService onlineOrderService,
        IShippingService shippingService,
        AppDbContext context)
    {
        _onlineOrderService = onlineOrderService;
        _shippingService = shippingService;
        _context = context;
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

    /// <summary>
    /// Giai đoạn 6: Tạo lại vận đơn (retry khi carrier=NONE)
    /// </summary>
    [HttpPost("{maDH}/shipping/retry")]
    public async Task<IActionResult> RetryShipment(string maDH)
    {
        var order = await _context.DonHangOnlines
            .Include(d => d.ChiTietDonHangs)
                .ThenInclude(c => c.MatHang)
            .FirstOrDefaultAsync(d => d.MaDH == maDH);
        if (order == null) return NotFound(new { message = "Không tìm thấy đơn hàng." });

        var shipping = await _shippingService.CreateShipmentAsync(order, order.ChiTietDonHangs.ToList());
        return Ok(shipping);
    }
}
