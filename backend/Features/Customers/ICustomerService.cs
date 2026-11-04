using PhoneStoreAPI.Features.Customers.DTOs;

namespace PhoneStoreAPI.Features.Customers;

public interface ICustomerService
{
    Task<(List<CustomerDto> Items, int TotalCount)> GetAllAsync(string? keyword = null, int? page = null, int? pageSize = null);
    Task<CustomerDto> GetByIdAsync(string id);
    Task<CustomerDto> GetByPhoneAsync(string sdt);
    Task<CustomerDto> CreateAsync(CreateCustomerRequest request);
    Task<CustomerDto> UpdateAsync(string id, UpdateCustomerRequest request);
    Task DeleteAsync(string id);
}
