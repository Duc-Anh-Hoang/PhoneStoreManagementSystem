namespace PhoneStoreAPI.Features.Auth.DTOs;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserInfoDto
{
    public string Username { get; set; } = string.Empty;
    public string MaNV { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string TenNhanVien => HoTen;
    public string Role { get; set; } = string.Empty;
    public string MaQuyen => Role;
    public string TenQuyen { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserInfoDto User { get; set; } = new();
}

public class CustomerLoginRequest
{
    public string Phone { get; set; } = string.Empty;
    public string? HoTen { get; set; }
}

public class CustomerUserInfoDto
{
    public string MaKH { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string? Sdt { get; set; }
    public string? Email { get; set; }
    public string? DiaChi { get; set; }
}

public class CustomerLoginResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public CustomerUserInfoDto User { get; set; } = new();
}

public class GoogleLoginRequest
{
    public string IdToken { get; set; } = string.Empty;
}
