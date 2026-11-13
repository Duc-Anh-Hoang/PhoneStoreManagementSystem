using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Promotions.DTOs;

namespace PhoneStoreAPI.Features.Promotions;

[ApiController]
[Route("api/promotions")]
public class PromotionsController : ControllerBase
{
    private readonly IPromotionService _promotionService;

    public PromotionsController(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActive()
    {
        var list = await _promotionService.GetActivePromotionsAsync();
        return Ok(list);
    }

    [HttpGet]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier}")]
    public async Task<IActionResult> GetAll()
    {
        var list = await _promotionService.GetAllPromotionsAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales},{AppConstants.Roles.Cashier}")]
    public async Task<IActionResult> GetById(string id)
    {
        var km = await _promotionService.GetPromotionByIdAsync(id);
        return Ok(km);
    }

    [HttpPost]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager}")]
    public async Task<IActionResult> Create([FromBody] CreatePromotionRequest request)
    {
        var created = await _promotionService.CreatePromotionAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.MaKM }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdatePromotionRequest request)
    {
        var updated = await _promotionService.UpdatePromotionAsync(id, request);
        return Ok(updated);
    }

    [HttpPut("{id}/tamdung")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager}")]
    public async Task<IActionResult> UpdateStatus(string id, [FromQuery] bool? tamDung, [FromBody] UpdateStatusBody? body)
    {
        bool statusToSet = tamDung ?? body?.TamDung ?? true;
        await _promotionService.UpdateStatusAsync(id, statusToSet);
        return Ok(new { message = "Cập nhật trạng thái khuyến mãi thành công." });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _promotionService.DeletePromotionAsync(id);
        return Ok(new { message = $"Đã xóa khuyến mãi {id} thành công." });
    }

    public class UpdateStatusBody
    {
        public bool TamDung { get; set; }
    }
}
