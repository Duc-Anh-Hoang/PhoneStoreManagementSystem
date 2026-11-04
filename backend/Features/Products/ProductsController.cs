using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Products.DTOs;

namespace PhoneStoreAPI.Features.Products;

[ApiController]
[Route("api/mathang")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// P2-02: Lấy danh sách mặt hàng (công khai) kèm lọc, tìm kiếm, phân trang
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParameters query)
    {
        var (items, totalCount) = await _productService.GetProductsAsync(query);
        Response.Headers["X-Total-Count"] = totalCount.ToString();
        return Ok(items);
    }

    /// <summary>
    /// P2-03: Xem chi tiết một mặt hàng
    /// </summary>
    [HttpGet("{ma}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductByMa([FromRoute] string ma)
    {
        var item = await _productService.GetProductByMaAsync(ma);
        return Ok(item);
    }

    /// <summary>
    /// P2-04: Thêm mới mặt hàng
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        var created = await _productService.CreateProductAsync(request);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    /// <summary>
    /// P2-04: Cập nhật thông tin mặt hàng
    /// </summary>
    [HttpPut("{ma}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] string ma, [FromBody] UpdateProductRequest request)
    {
        var updated = await _productService.UpdateProductAsync(ma, request);
        return Ok(updated);
    }

    /// <summary>
    /// P2-04: Xóa mặt hàng
    /// </summary>
    [HttpDelete("{ma}")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] string ma)
    {
        await _productService.DeleteProductAsync(ma);
        return Ok(new { success = true, message = $"Đã xóa thành công mặt hàng '{ma}'" });
    }

    /// <summary>
    /// P2-05: Lấy thông số kỹ thuật của mặt hàng
    /// </summary>
    [HttpGet("{ma}/thongso")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductSpecs([FromRoute] string ma)
    {
        var specs = await _productService.GetProductSpecsAsync(ma);
        return Ok(specs);
    }

    /// <summary>
    /// P2-05: Ghi đè toàn bộ thông số kỹ thuật của mặt hàng
    /// </summary>
    [HttpPut("{ma}/thongso")]
    [Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Stock}")]
    public async Task<IActionResult> UpdateProductSpecs([FromRoute] string ma, [FromBody] List<UpdateProductSpecRequest> specs)
    {
        var updated = await _productService.UpdateProductSpecsAsync(ma, specs);
        return Ok(updated);
    }
}
