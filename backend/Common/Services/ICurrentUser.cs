namespace PhoneStoreAPI.Common.Services;

public interface ICurrentUser
{
    string? UserName { get; }
    string? MaNV { get; }
    string? Role { get; }
    string? MaKH { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}
