using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Sales.Returns.DTOs;

namespace PhoneStoreAPI.Features.Sales.Returns;

[ApiController]
[Route("api/trahang")]
public class ReturnsController : ControllerBase
{
    private readonly IReturnsService _returnsService;

    public ReturnsController(IReturnsService returnsService)
    {
        _returnsService = returnsService;
    }

    [HttpPost]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier}")]
    public async Task<IActionResult> CreateTraHang([FromBody] CreateTraHangRequest request)
    {
        var result = await _returnsService.CreateTraHangAsync(request);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier}")]
    public async Task<IActionResult> GetTraHangs(
        [FromQuery] DateOnly? tuNgay,
        [FromQuery] DateOnly? denNgay,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, total) = await _returnsService.GetTraHangsAsync(tuNgay, denNgay, page, pageSize);
        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }
}
