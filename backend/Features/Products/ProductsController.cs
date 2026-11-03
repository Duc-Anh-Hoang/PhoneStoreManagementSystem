using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Features.Products.DTOs;

namespace PhoneStoreAPI.Features.Products;

[ApiController]
[Route("api/mathang")]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null) return NotFound(new { message = $"Không tìm thấy mặt hàng với mã {id}" });
        return Ok(product);
    }

    [HttpGet("{id}/thongso")]
    public async Task<IActionResult> GetSpecs(string id)
    {
        var specs = await _productService.GetProductSpecsAsync(id);
        return Ok(specs);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.MaHang) || string.IsNullOrWhiteSpace(dto.TenHang))
        {
            return BadRequest(new { message = "Mã hàng và tên hàng không được để trống" });
        }

        var created = await _productService.CreateProductAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.MaHang }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductDto dto)
    {
        var success = await _productService.UpdateProductAsync(id, dto);
        if (!success) return NotFound(new { message = $"Không tìm thấy mặt hàng {id} để cập nhật" });
        return Ok(new { message = "Cập nhật mặt hàng thành công" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var success = await _productService.DeleteProductAsync(id);
        if (!success) return NotFound(new { message = $"Không tìm thấy mặt hàng {id} để xóa" });
        return Ok(new { message = "Xóa mặt hàng thành công" });
    }
}
