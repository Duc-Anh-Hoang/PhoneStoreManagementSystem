using PhoneStoreAPI.Features.Products.DTOs;

namespace PhoneStoreAPI.Features.Products;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(string maHang);
    Task<List<ProductSpecDto>> GetProductSpecsAsync(string maHang);
    Task<ProductDto> CreateProductAsync(ProductDto dto);
    Task<bool> UpdateProductAsync(string maHang, ProductDto dto);
    Task<bool> DeleteProductAsync(string maHang);
}
