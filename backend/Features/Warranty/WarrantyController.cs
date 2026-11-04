using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PhoneStoreAPI.Features.Warranty.DTOs;

namespace PhoneStoreAPI.Features.Warranty;

[ApiController]
[Route("api/baohanh")]
[AllowAnonymous]
[EnableRateLimiting("public-lookup")]
public class WarrantyController : ControllerBase
{
    private readonly IWarrantyService _warrantyService;

    public WarrantyController(IWarrantyService warrantyService)
    {
        _warrantyService = warrantyService;
    }

    [HttpGet("imei/{imei}")]
    public async Task<IActionResult> GetByImei(string imei)
    {
        var result = await _warrantyService.GetByImeiAsync(imei);
        return Ok(result);
    }

    [HttpGet("trangthai/{imei}")]
    public async Task<IActionResult> GetStatusByImei(string imei)
    {
        var result = await _warrantyService.GetStatusByImeiAsync(imei);
        return Ok(result);
    }
}
