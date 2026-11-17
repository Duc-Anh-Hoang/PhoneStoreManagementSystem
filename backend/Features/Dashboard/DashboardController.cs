using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Features.Dashboard.DTOs;

namespace PhoneStoreAPI.Features.Dashboard;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromQuery] DateTime? tuNgay,
        [FromQuery] DateTime? denNgay)
    {
        var result = await _dashboardService.GetSummaryAsync(tuNgay, denNgay);
        return Ok(result);
    }

    [HttpGet("channels")]
    public async Task<IActionResult> GetRevenueByChannel(
        [FromQuery] DateTime? tuNgay,
        [FromQuery] DateTime? denNgay)
    {
        var result = await _dashboardService.GetRevenueByChannelAsync(tuNgay, denNgay);
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetRevenueByCategory(
        [FromQuery] DateTime? tuNgay,
        [FromQuery] DateTime? denNgay)
    {
        var result = await _dashboardService.GetRevenueByCategoryAsync(tuNgay, denNgay);
        return Ok(result);
    }

    [HttpGet("top-selling")]
    public async Task<IActionResult> GetTopSelling(
        [FromQuery] int top = 5,
        [FromQuery] DateTime? tuNgay = null,
        [FromQuery] DateTime? denNgay = null)
    {
        var result = await _dashboardService.GetTopSellingProductsAsync(top, tuNgay, denNgay);
        return Ok(result);
    }

    [HttpGet("slow-moving")]
    public async Task<IActionResult> GetSlowMoving(
        [FromQuery] int top = 5,
        [FromQuery] DateTime? tuNgay = null,
        [FromQuery] DateTime? denNgay = null)
    {
        var result = await _dashboardService.GetSlowMovingProductsAsync(top, tuNgay, denNgay);
        return Ok(result);
    }

    [HttpGet("top-customers")]
    public async Task<IActionResult> GetTopCustomers(
        [FromQuery] int top = 5,
        [FromQuery] DateTime? tuNgay = null,
        [FromQuery] DateTime? denNgay = null)
    {
        var result = await _dashboardService.GetTopCustomersAsync(top, tuNgay, denNgay);
        return Ok(result);
    }
}
