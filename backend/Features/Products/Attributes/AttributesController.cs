using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Products.Attributes;

[ApiController]
[Route("api/thuoctinh")]
[Route("api/attributes")]
public class AttributesController : ControllerBase
{
    private readonly IAttributeService _attributeService;

    public AttributesController(IAttributeService attributeService)
    {
        _attributeService = attributeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _attributeService.GetAllAttributesAsync();
        return Ok(list);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ThuocTinh thuocTinh)
    {
        var created = await _attributeService.CreateAttributeAsync(thuocTinh);
        return Ok(created);
    }
}
