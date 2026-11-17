using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Features.Checkout.Services;

namespace PhoneStoreAPI.Features.Checkout.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReconciliationController : ControllerBase
{
    private readonly IReconciliationService _reconciliationService;

    public ReconciliationController(IReconciliationService reconciliationService)
    {
        _reconciliationService = reconciliationService;
    }

    [HttpGet("daily")]
    public async Task<ActionResult<ReconciliationReportDTO>> GetDailyReport([FromQuery] DateTime? date)
    {
        var report = await _reconciliationService.ReconcileDailyTransactionsAsync(date);
        return Ok(report);
    }
}
