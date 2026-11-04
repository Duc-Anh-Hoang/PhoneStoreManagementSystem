using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Warranty.DTOs;

namespace PhoneStoreAPI.Features.Warranty;

[ApiController]
[Route("api/tiepnhanbaohanh")]
public class TiepNhanBaoHanhController : ControllerBase
{
    private readonly IWarrantyService _warrantyService;

    public TiepNhanBaoHanhController(IWarrantyService warrantyService)
    {
        _warrantyService = warrantyService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? trangThai,
        [FromQuery] DateTime? tuNgay,
        [FromQuery] DateTime? denNgay,
        [FromQuery] string? maNVKyThuat,
        [FromQuery] string? keyword,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, totalCount) = await _warrantyService.GetTiepNhanListAsync(
            trangThai, tuNgay, denNgay, maNVKyThuat, keyword, page, pageSize);

        Response.Headers.Append("X-Total-Count", totalCount.ToString());
        return Ok(items);
    }

    [HttpGet("{ma}")]
    [Authorize]
    public async Task<IActionResult> GetById(string ma)
    {
        var result = await _warrantyService.GetTiepNhanByIdAsync(ma);
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateTiepNhanRequest request)
    {
        var result = await _warrantyService.CreateTiepNhanAsync(request);
        return CreatedAtAction(nameof(GetById), new { ma = result.MaTN }, result);
    }

    [HttpPut("{ma}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Tech}")]
    public async Task<IActionResult> Update(string ma, [FromBody] UpdateTiepNhanRequest request)
    {
        var result = await _warrantyService.UpdateTiepNhanAsync(ma, request);
        return Ok(result);
    }

    [HttpPut("{ma}/trangthai")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Tech}")]
    public async Task<IActionResult> UpdateTrangThai(string ma, [FromBody] UpdateTrangThaiRequest request)
    {
        var result = await _warrantyService.UpdateTrangThaiAsync(ma, request);
        return Ok(result);
    }
}
