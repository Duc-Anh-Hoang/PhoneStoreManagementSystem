using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Products.Categories;

[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("api/nhomhang")]
    [HttpGet("api/categories/nhomhang")]
    public async Task<IActionResult> GetNhomHang()
    {
        var list = await _categoryService.GetAllNhomHangAsync();
        return Ok(list);
    }

    [HttpPost("api/nhomhang")]
    public async Task<IActionResult> CreateNhomHang([FromBody] NhomHang nhomHang)
    {
        var created = await _categoryService.CreateNhomHangAsync(nhomHang);
        return Ok(created);
    }

    [HttpGet("api/loaihang")]
    [HttpGet("api/categories/loaihang")]
    public async Task<IActionResult> GetLoaiHang()
    {
        var list = await _categoryService.GetAllLoaiHangAsync();
        return Ok(list);
    }

    [HttpPost("api/loaihang")]
    public async Task<IActionResult> CreateLoaiHang([FromBody] LoaiHang loaiHang)
    {
        var created = await _categoryService.CreateLoaiHangAsync(loaiHang);
        return Ok(created);
    }
}
