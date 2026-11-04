using PhoneStoreAPI.Features.Suppliers.DTOs;

namespace PhoneStoreAPI.Features.Suppliers;

public interface ISupplierService
{
    Task<(List<SupplierDto> Items, int TotalCount)> GetAllAsync(int? page = null, int? pageSize = null);
    Task<SupplierDto> GetByIdAsync(string id);
    Task<SupplierDto> CreateAsync(CreateSupplierRequest request);
    Task<SupplierDto> UpdateAsync(string id, UpdateSupplierRequest request);
    Task DeleteAsync(string id);
}
