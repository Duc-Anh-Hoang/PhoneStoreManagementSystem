using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Products.Attributes;

public interface IAttributeService
{
    Task<List<ThuocTinh>> GetAllAttributesAsync();
    Task<ThuocTinh> CreateAttributeAsync(ThuocTinh thuocTinh);
}
