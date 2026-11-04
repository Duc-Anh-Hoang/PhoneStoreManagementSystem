using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Products.Categories.DTOs;

namespace PhoneStoreAPI.Features.Products.Categories;

[ApiController]
[Route("api/nhomhang")]
public class NhomHangController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public NhomHangController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var list = await _categoryService.GetAllNhomHangAsync();
        return Ok(list);
    }

    [HttpGet("{ma}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByMa([FromRoute] string ma)
    {
        var item = await _categoryService.GetNhomHangByMaAsync(ma);
        return Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> Create([FromBody] CreateNhomHangRequest request)
    {
        var created = await _categoryService.CreateNhomHangAsync(request);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpPut("{ma}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> Update([FromRoute] string ma, [FromBody] UpdateNhomHangRequest request)
    {
        var updated = await _categoryService.UpdateNhomHangAsync(ma, request);
        return Ok(updated);
    }

    [HttpDelete("{ma}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> Delete([FromRoute] string ma)
    {
        await _categoryService.DeleteNhomHangAsync(ma);
        return Ok(new { success = true, message = $"Đã xóa nhóm hàng '{ma}'" });
    }
}

[ApiController]
[Route("api/loaihang")]
public class LoaiHangController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public LoaiHangController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var list = await _categoryService.GetAllLoaiHangAsync();
        return Ok(list);
    }

    [HttpGet("{ma}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByMa([FromRoute] string ma)
    {
        var item = await _categoryService.GetLoaiHangByMaAsync(ma);
        return Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> Create([FromBody] CreateLoaiHangRequest request)
    {
        var created = await _categoryService.CreateLoaiHangAsync(request);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpPut("{ma}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> Update([FromRoute] string ma, [FromBody] UpdateLoaiHangRequest request)
    {
        var updated = await _categoryService.UpdateLoaiHangAsync(ma, request);
        return Ok(updated);
    }

    [HttpDelete("{ma}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> Delete([FromRoute] string ma)
    {
        await _categoryService.DeleteLoaiHangAsync(ma);
        return Ok(new { success = true, message = $"Đã xóa loại hàng '{ma}'" });
    }
}
