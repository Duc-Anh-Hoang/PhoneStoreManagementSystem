using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Dashboard.DTOs;

namespace PhoneStoreAPI.Features.Dashboard;

[ApiController]
[Route("api/thongke")]
[Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager}")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("tonghop")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await _dashboardService.GetSummaryAsync();
        return Ok(result);
    }

    [HttpGet("kenhban")]
    public async Task<IActionResult> GetRevenueByChannel(
        [FromQuery] DateTime? tuNgay,
        [FromQuery] DateTime? denNgay)
    {
        var result = await _dashboardService.GetRevenueByChannelAsync(tuNgay, denNgay);
        return Ok(result);
    }

    [HttpGet("nhomhang")]
    public async Task<IActionResult> GetRevenueByCategory(
        [FromQuery] DateTime? tuNgay,
        [FromQuery] DateTime? denNgay)
    {
        var result = await _dashboardService.GetRevenueByCategoryAsync(tuNgay, denNgay);
        return Ok(result);
    }
}
