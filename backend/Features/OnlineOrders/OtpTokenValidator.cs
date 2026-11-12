using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PhoneStoreAPI.Features.OnlineOrders;

/// <summary>
/// Xác thực JWT OTP token (short-lived, claim "phone_lookup") và trích xuất số điện thoại.
/// Dùng cùng key/issuer/audience với JWT chính nhưng claim riêng để phân biệt mục đích.
/// </summary>
public static class OtpTokenValidator
{
    public const string PhoneLookupClaim = "phone_lookup";

    /// <summary>
    /// Xác thực token OTP và trả số điện thoại nếu hợp lệ, null nếu không hợp lệ.
    /// </summary>
    public static string? ValidateAndExtractPhone(string token, IServiceProvider services)
    {
        var config = services.GetRequiredService<IConfiguration>();
        var jwtSettings = config.GetSection("Jwt");
        var keyBytes = Encoding.UTF8.GetBytes(
            jwtSettings["Key"] ?? "PhoneStoreManagementSystem_SuperSecretKey_2026_AspNetCore10_StrongSecretKey!");

        var handler = new JwtSecurityTokenHandler();
        var validationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"] ?? "PhoneStoreAPI",
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"] ?? "PhoneStoreClient",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = handler.ValidateToken(token, validationParams, out _);
            // Xác nhận token dùng đúng mục đích phone_lookup (bảo mật — ngăn dùng token admin)
            var phone = principal.FindFirst(PhoneLookupClaim)?.Value;
            return string.IsNullOrWhiteSpace(phone) ? null : phone;
        }
        catch
        {
            // Token không hợp lệ, hết hạn hoặc chữ ký sai
            return null;
        }
    }
}
