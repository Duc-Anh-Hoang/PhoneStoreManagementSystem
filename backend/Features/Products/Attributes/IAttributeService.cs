using PhoneStoreAPI.Features.Products.Attributes.DTOs;

namespace PhoneStoreAPI.Features.Products.Attributes;

public interface IAttributeService
{
    Task<List<ThuocTinhDto>> GetAllThuocTinhAsync();
    Task<ThuocTinhDto> GetThuocTinhByMaAsync(string maThuocTinh);
    Task<ThuocTinhDto> CreateThuocTinhAsync(CreateThuocTinhRequest request);
    Task<ThuocTinhDto> UpdateThuocTinhAsync(string maThuocTinh, UpdateThuocTinhRequest request);
    Task DeleteThuocTinhAsync(string maThuocTinh);
}
