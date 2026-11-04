using PhoneStoreAPI.Features.Products.DTOs;

namespace PhoneStoreAPI.Features.Products;

public interface IProductService
{
    Task<(List<ProductDto> Items, int TotalCount)> GetProductsAsync(ProductQueryParameters query);
    Task<ProductDetailDto> GetProductByMaAsync(string maHang);
    Task<ProductDto> CreateProductAsync(CreateProductRequest request);
    Task<ProductDto> UpdateProductAsync(string maHang, UpdateProductRequest request);
    Task DeleteProductAsync(string maHang);
    Task<List<ProductSpecDto>> GetProductSpecsAsync(string maHang);
    Task<List<ProductSpecDto>> UpdateProductSpecsAsync(string maHang, List<UpdateProductSpecRequest> specs);
}
