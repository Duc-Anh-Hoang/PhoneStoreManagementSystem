using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Purchasing.DTOs;

namespace PhoneStoreAPI.Features.Purchasing;

[ApiController]
[Route("api/phieunhap")]
[Route("api/purchasing")]
[AllowAnonymous]
public class PurchasingController : ControllerBase
{
    private readonly IPurchasingService _purchasingService;

    public PurchasingController(IPurchasingService purchasingService)
    {
        _purchasingService = purchasingService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePhieuNhapRequest request)
    {
        var result = await _purchasingService.CreateAsync(request);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DateTime? tuNgay,
        [FromQuery] DateTime? denNgay,
        [FromQuery] string? maNCC,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var (items, totalCount) = await _purchasingService.GetAllAsync(tuNgay, denNgay, maNCC, page, pageSize);
        Response.Headers.Append("X-Total-Count", totalCount.ToString());
        return Ok(items);
    }

    [HttpGet("{so}")]
    public async Task<IActionResult> GetBySo(string so)
    {
        var item = await _purchasingService.GetBySoAsync(so);
        return Ok(item);
    }
}
