using Microsoft.AspNetCore.Mvc;

namespace PhoneStoreAPI.Features.Sales.Returns;

[ApiController]
[Route("api/[controller]")]
public class ReturnsController : ControllerBase
{
    private readonly IReturnsService _returnsService;

    public ReturnsController(IReturnsService returnsService)
    {
        _returnsService = returnsService;
    }
}
