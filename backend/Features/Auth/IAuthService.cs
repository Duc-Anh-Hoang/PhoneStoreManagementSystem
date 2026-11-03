using PhoneStoreAPI.Features.Auth.DTOs;

namespace PhoneStoreAPI.Features.Auth;

public interface IAuthService
{
    Task<LoginResponse> LoginStaffAsync(LoginRequest request);
    Task<CustomerLoginResponse> LoginCustomerAsync(CustomerLoginRequest request);
}
