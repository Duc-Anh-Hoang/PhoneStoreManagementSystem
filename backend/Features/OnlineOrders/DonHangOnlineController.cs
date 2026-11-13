using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Customers.Helpers;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;
using PhoneStoreAPI.Features.OnlineOrders.Services;

namespace PhoneStoreAPI.Features.OnlineOrders;

[ApiController]
[Route("api/donhangonline")]
[AllowAnonymous]
public class DonHangOnlineController : ControllerBase
{
    private readonly IOnlineOrderService _onlineOrderService;
    private readonly IShippingService _shippingService;
    private readonly IOrderSettingsService _settings;
    private readonly AppDbContext _context;

    public DonHangOnlineController(
        IOnlineOrderService onlineOrderService,
        IShippingService shippingService,
        IOrderSettingsService settings,
        AppDbContext context)
    {
        _onlineOrderService = onlineOrderService;
        _shippingService = shippingService;
        _settings = settings;
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
    /// Giai đoạn 6: Lấy thông tin vận chuyển của đơn hàng
    /// </summary>
    [HttpGet("{maDH}/shipping")]
    public async Task<IActionResult> GetShipping(string maDH)
    {
        var shipping = await _shippingService.GetTrackingAsync(maDH);
        if (shipping == null) return NotFound(new { message = "Chưa có thông tin vận chuyển cho đơn hàng này." });
        return Ok(shipping);
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

    /// <summary>
    /// Pipeline Auto-Approval & Bulk Approval: Duyệt hàng loạt đơn hàng online khi lượng user đặt cao điểm
    /// </summary>
    [HttpPost("bulk-approve")]
    public async Task<IActionResult> BulkApproveOrders([FromBody] BulkApproveRequest? request)
    {
        var result = await _onlineOrderService.BulkApproveOrdersAsync(request?.OrderCodes);
        return Ok(result);
    }

    /// <summary>
    /// Lấy toàn bộ settings của hệ thống duyệt đơn (plan (8).md)
    /// </summary>
    [HttpGet("settings")]
    public async Task<IActionResult> GetSettings()
    {
        var dict = await _settings.GetAllSettingsAsync();
        return Ok(dict);
    }

    /// <summary>
    /// Cập nhật setting theo key (plan (8).md)
    /// </summary>
    [HttpPut("settings/{key}")]
    public async Task<IActionResult> UpdateSetting(string key, [FromBody] UpdateSettingDto request)
    {
        var ok = await _settings.UpdateSettingAsync(key, request.Value);
        return Ok(new { success = ok, key, value = request.Value });
    }

    /// <summary>
    /// Thêm hoặc gỡ SĐT khỏi Blacklist
    /// </summary>
    [HttpPost("blacklist/{phone}/toggle")]
    public async Task<IActionResult> ToggleBlacklist(string phone)
    {
        var norm = PhoneHelper.NormalizeVietnamesePhone(phone) ?? phone.Trim();
        var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.Sdt == norm);
        if (kh == null) return NotFound(new { message = "Không tìm thấy khách hàng với SĐT này." });
        kh.IsBlacklisted = !kh.IsBlacklisted;
        kh.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Ok(new { phone = norm, isBlacklisted = kh.IsBlacklisted });
    }

    /// <summary>
    /// Lấy lịch sử biến động trạng thái của đơn hàng (order_status_logs)
    /// </summary>
    [HttpGet("{maDH}/logs")]
    public async Task<IActionResult> GetOrderLogs(string maDH)
    {
        var logs = await _context.OrderStatusLogs
            .Where(l => l.OrderId == maDH)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
        return Ok(logs);
    }
}

public class UpdateSettingDto
{
    public string Value { get; set; } = string.Empty;
}
