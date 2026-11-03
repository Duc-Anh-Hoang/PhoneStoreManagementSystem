using System.Security.Claims;

namespace PhoneStoreAPI.Common.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public string? UserName => User?.Identity?.Name 
        ?? User?.FindFirst(ClaimTypes.Name)?.Value 
        ?? User?.FindFirst("username")?.Value;

    public string? MaNV => User?.FindFirst("manv")?.Value 
        ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? Role => User?.FindFirst(ClaimTypes.Role)?.Value 
        ?? User?.FindFirst("role")?.Value;

    public string? MaKH => User?.FindFirst("makh")?.Value;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role)
    {
        if (string.IsNullOrEmpty(role)) return false;
        return string.Equals(Role, role, StringComparison.OrdinalIgnoreCase) 
            || (User?.IsInRole(role) ?? false);
    }
}
