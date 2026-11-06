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

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateHoaDon([FromBody] CreateHoaDonRequest request)
    {
        var result = await _salesService.CreateHoaDonAsync(request);
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
        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [HttpGet("{soHD}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHoaDonChiTiet(string soHD)
    {
        var result = await _salesService.GetHoaDonChiTietAsync(soHD);
        return Ok(result);
    }
}
