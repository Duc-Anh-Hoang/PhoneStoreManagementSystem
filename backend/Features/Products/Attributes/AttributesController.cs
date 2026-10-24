using Microsoft.AspNetCore.Mvc;

namespace PhoneStoreAPI.Features.Products.Attributes;

[ApiController]
[Route("api/[controller]")]
public class AttributesController : ControllerBase
{
    private readonly IAttributeService _attributeService;

    public AttributesController(IAttributeService attributeService)
    {
        _attributeService = attributeService;
    }
}
