using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Promotions;

[ApiController]
[Route("api/khuyenmai")]
[Route("api/promotions")]
public class PromotionsController : ControllerBase
{
    private readonly IPromotionService _promotionService;

    public PromotionsController(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _promotionService.GetAllPromotionsAsync();
        return Ok(list);
    }

    [HttpGet("hoatdong")]
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var list = await _promotionService.GetActivePromotionsAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var km = await _promotionService.GetPromotionByIdAsync(id);
        if (km == null) return NotFound(new { message = $"Không tìm thấy khuyến mãi {id}" });
        return Ok(km);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] KhuyenMai km)
    {
        var created = await _promotionService.CreatePromotionAsync(km);
        return CreatedAtAction(nameof(GetById), new { id = created.MaKM }, created);
    }

    [HttpPut("{id}/tamdung")]
    public async Task<IActionResult> UpdateStatus(string id, [FromQuery] bool tamDung)
    {
        var success = await _promotionService.UpdateStatusAsync(id, tamDung);
        if (!success) return NotFound(new { message = $"Không tìm thấy khuyến mãi {id}" });
        return Ok(new { message = "Cập nhật trạng thái khuyến mãi thành công" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var success = await _promotionService.DeletePromotionAsync(id);
        if (!success) return NotFound(new { message = $"Không tìm thấy khuyến mãi {id}" });
        return Ok(new { message = "Xóa khuyến mãi thành công" });
    }
}
