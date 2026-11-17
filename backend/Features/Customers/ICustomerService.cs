using PhoneStoreAPI.Features.Customers.DTOs;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;

namespace PhoneStoreAPI.Features.Customers;

public interface ICustomerService
{
    Task<(List<CustomerDto> Items, int TotalCount)> GetAllAsync(string? keyword = null, int? page = null, int? pageSize = null);
    Task<CustomerDto> GetByIdAsync(string id);
    Task<CustomerDto> GetByPhoneAsync(string sdt);
    Task<CustomerDto> CreateAsync(CreateCustomerRequest request);
    Task<CustomerDto> UpdateAsync(string id, UpdateCustomerRequest request);
    Task DeleteAsync(string id);

    // Nghiệp vụ tra cứu VIP & ưu đãi theo số điện thoại (bảo mật, masked data, chống lạm dụng)
    Task<MembershipLookupResponse> LookupMembershipAsync(MembershipLookupRequest request, string clientIp);
    Task<OtpRequestResult> SendMembershipOtpAsync(string sdt, string clientIp);
    Task<OtpVerifyResult> VerifyMembershipOtpAsync(string sdt, string code);
}
