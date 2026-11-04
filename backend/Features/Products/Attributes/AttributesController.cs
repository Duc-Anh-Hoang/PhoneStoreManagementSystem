using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Products.Attributes.DTOs;

namespace PhoneStoreAPI.Features.Products.Attributes;

[ApiController]
[Route("api/thuoctinh")]
public class AttributesController : ControllerBase
{
    private readonly IAttributeService _attributeService;

    public AttributesController(IAttributeService attributeService)
    {
        _attributeService = attributeService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var list = await _attributeService.GetAllThuocTinhAsync();
        return Ok(list);
    }

    [HttpGet("{ma}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByMa([FromRoute] string ma)
    {
        var item = await _attributeService.GetThuocTinhByMaAsync(ma);
        return Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> Create([FromBody] CreateThuocTinhRequest request)
    {
        var created = await _attributeService.CreateThuocTinhAsync(request);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpPut("{ma}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> Update([FromRoute] string ma, [FromBody] UpdateThuocTinhRequest request)
    {
        var updated = await _attributeService.UpdateThuocTinhAsync(ma, request);
        return Ok(updated);
    }

    [HttpDelete("{ma}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> Delete([FromRoute] string ma)
    {
        await _attributeService.DeleteThuocTinhAsync(ma);
        return Ok(new { success = true, message = $"Đã xóa thuộc tính '{ma}'" });
    }
}
