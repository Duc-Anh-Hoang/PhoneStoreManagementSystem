using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Sales.DTOs;

namespace PhoneStoreAPI.Features.Sales;

[ApiController]
[Route("api/hoadon")]
public class SalesController : ControllerBase
{
    private readonly ISalesService _salesService;

    public SalesController(ISalesService salesService)
    {
        _salesService = salesService;
    }

    private bool CanViewCustomerPhone() =>
        User.IsInRole(AppConstants.Roles.Admin) || User.IsInRole(AppConstants.Roles.Manager) ||
        User.IsInRole(AppConstants.Roles.Sales) || User.IsInRole(AppConstants.Roles.Cashier);

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateHoaDon([FromBody] CreateHoaDonRequest request)
    {
        var result = await _salesService.CreateHoaDonAsync(request);
        if (!CanViewCustomerPhone()) result.SdtKhachHang = null;
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetHoaDons(
        [FromQuery] DateOnly? tuNgay,
        [FromQuery] DateOnly? denNgay,
        [FromQuery] string? kenhBan,
        [FromQuery] string? maKH,
        [FromQuery] string? keyword,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, total) = await _salesService.GetHoaDonsAsync(tuNgay, denNgay, kenhBan, maKH, keyword, page, pageSize);
        if (!CanViewCustomerPhone())
        {
            foreach (var item in items) item.SdtKhachHang = null;
        }
        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [HttpGet("{soHD}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHoaDonChiTiet(string soHD)
    {
        var result = await _salesService.GetHoaDonChiTietAsync(soHD);
        if (!CanViewCustomerPhone()) result.SdtKhachHang = null;
        return Ok(result);
    }
}
