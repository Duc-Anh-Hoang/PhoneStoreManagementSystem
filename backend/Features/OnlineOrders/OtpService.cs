using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using PhoneStoreAPI.Features.Customers.Helpers;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;

namespace PhoneStoreAPI.Features.OnlineOrders;

/// <summary>
/// State lưu trong IMemoryCache cho mỗi OTP request (key: "otp:{normalizedPhone}").
/// Không lưu OTP plaintext — chỉ lưu hash để bảo vệ nếu cache bị lộ.
/// </summary>
internal sealed class OtpState
{
    public string CodeHash { get; init; } = string.Empty;      // SHA256(code + phone)
    public string Phone { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }                   // Hết hạn sau 5 phút
    public int Attempts { get; set; }                          // Số lần nhập sai (max 5)
    public DateTime CreatedAt { get; init; }
    public int RequestCount { get; set; }                      // Số lần request trong cửa sổ hiện tại (max 3/giờ)
    public DateTime RequestWindowStart { get; set; }           // Thời điểm bắt đầu cửa sổ đếm request
}

public class OtpService : IOtpService
{
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _config;
    private readonly ILogger<OtpService> _logger;

    private const int OtpExpiryMinutes = 5;
    private const int MaxAttempts = 5;
    private const int MaxRequestsPerHour = 3;

    public OtpService(IMemoryCache cache, IConfiguration config, ILogger<OtpService> logger)
    {
        _cache = cache;
        _config = config;
        _logger = logger;
    }

    private static string NormalizePhone(string phone)
        => PhoneHelper.NormalizeVietnamesePhone(phone) ?? phone.Trim().Replace(" ", "").Replace(".", "").Replace("-", "");

    private static string CacheKey(string normalizedPhone) => $"otp:{normalizedPhone}";

    /// <summary>
    /// Hash OTP code kèm phone để tránh rainbow table attack.
    /// </summary>
    private static string ComputeHash(string code, string phone)
    {
        var data = Encoding.UTF8.GetBytes(code + phone);
        var hash = SHA256.HashData(data);
        return Convert.ToHexString(hash);
    }

    private string GetOtpMode()
        => _config["OTP_MODE"] ?? _config["OtpMode"] ?? Environment.GetEnvironmentVariable("OTP_MODE") ?? "demo";

    public async Task<OtpRequestResult> RequestOtpAsync(string phone)
    {
        var normalized = NormalizePhone(phone);
        var key = CacheKey(normalized);

        // Kiểm tra giới hạn số lần request (tối đa 3 lần/giờ/SĐT — chống spam OTP)
        if (_cache.TryGetValue<OtpState>(key, out var existing) && existing != null)
        {
            var windowAge = DateTime.UtcNow - existing.RequestWindowStart;
            if (windowAge < TimeSpan.FromHours(1) && existing.RequestCount >= MaxRequestsPerHour)
            {
                _logger.LogWarning("OTP request vượt giới hạn {Max}/giờ cho SĐT {Phone}", MaxRequestsPerHour, normalized);
                return new OtpRequestResult
                {
                    Blocked = true,
                    Message = "Bạn đã yêu cầu mã OTP quá 3 lần trong 1 giờ. Vui lòng thử lại sau."
                };
            }
        }

        // Sinh OTP 6 chữ số bằng RandomNumberGenerator — nguồn ngẫu nhiên mật mã học
        int otpInt = RandomNumberGenerator.GetInt32(0, 1_000_000);
        string otpCode = otpInt.ToString("D6");

        var now = DateTime.UtcNow;
        var state = new OtpState
        {
            CodeHash = ComputeHash(otpCode, normalized),
            Phone = normalized,
            ExpiresAt = now.AddMinutes(OtpExpiryMinutes),
            Attempts = 0,
            CreatedAt = now,
            RequestCount = (existing != null && (now - existing.RequestWindowStart) < TimeSpan.FromHours(1))
                           ? existing.RequestCount + 1 : 1,
            RequestWindowStart = (existing != null && (now - existing.RequestWindowStart) < TimeSpan.FromHours(1))
                                 ? existing.RequestWindowStart : now
        };

        // Lưu vào cache hết hạn sau 1 giờ (cửa sổ request)
        _cache.Set(key, state, TimeSpan.FromHours(1));

        // Gửi OTP qua hàm tích hợp duy nhất
        await SendOtpAsync(normalized, otpCode);

        var isDemo = string.Equals(GetOtpMode(), "demo", StringComparison.OrdinalIgnoreCase);

        // Luôn trả cùng một message để tránh lộ SĐT có đơn hàng hay không (bảo mật)
        // Chỉ trả code về client khi OTP_MODE=demo
        return new OtpRequestResult
        {
            Blocked = false,
            Message = "Nếu số điện thoại hợp lệ, mã OTP đã được gửi.",
            Code = isDemo ? otpCode : null
        };
    }

    public Task<OtpVerifyResult> VerifyOtpAsync(string phone, string code)
    {
        var normalized = NormalizePhone(phone);
        var key = CacheKey(normalized);

        if (!_cache.TryGetValue<OtpState>(key, out var state) || state == null)
        {
            // Không có OTP hoặc đã hết hạn
            return Task.FromResult(new OtpVerifyResult { Success = false, Error = "Mã OTP không hợp lệ hoặc đã hết hạn." });
        }

        // Kiểm tra hết hạn thời gian
        if (DateTime.UtcNow > state.ExpiresAt)
        {
            _cache.Remove(key);
            return Task.FromResult(new OtpVerifyResult { Success = false, Error = "Mã OTP đã hết hạn." });
        }

        // Kiểm tra số lần thử (bảo mật — chống brute-force OTP)
        if (state.Attempts >= MaxAttempts)
        {
            _cache.Remove(key);
            return Task.FromResult(new OtpVerifyResult { Success = false, Error = "Đã vượt quá số lần thử. Vui lòng yêu cầu mã mới." });
        }

        var inputHash = ComputeHash(code.Trim(), normalized);
        if (!string.Equals(inputHash, state.CodeHash, StringComparison.Ordinal))
        {
            // Tăng số lần thử sai; nếu đạt 5 lần thì hủy OTP ngay
            state.Attempts++;
            _logger.LogWarning("OTP sai lần {Attempt}/{Max} cho SĐT {Phone}", state.Attempts, MaxAttempts, normalized);
            if (state.Attempts >= MaxAttempts)
            {
                // Bảo mật: xóa CodeHash để hủy OTP hiện tại nhưng giữ lại RequestCount trong cửa sổ 1 giờ
                _cache.Set(key, new OtpState
                {
                    CodeHash = string.Empty,
                    Phone = normalized,
                    ExpiresAt = DateTime.MinValue,
                    Attempts = MaxAttempts,
                    CreatedAt = state.CreatedAt,
                    RequestCount = state.RequestCount,
                    RequestWindowStart = state.RequestWindowStart
                }, TimeSpan.FromHours(1));
                return Task.FromResult(new OtpVerifyResult { Success = false, Error = "Đã nhập sai quá 5 lần. Mã OTP đã bị hủy, vui lòng yêu cầu mã mới." });
            }
            return Task.FromResult(new OtpVerifyResult { Success = false, Error = $"Mã OTP không đúng (còn {MaxAttempts - state.Attempts} lần thử)." });
        }

        // OTP đúng → xóa khỏi cache (single-use) và phát JWT short-lived
        _cache.Remove(key);

        var token = GeneratePhoneLookupToken(normalized);
        _logger.LogInformation("OTP xác thực thành công cho SĐT {Phone}", normalized);

        return Task.FromResult(new OtpVerifyResult
        {
            Success = true,
            Token = token.Token,
            ExpiresAt = token.ExpiresAt
        });
    }

    /// <summary>
    /// Phát JWT short-lived (10 phút) với claim "phone_lookup" để tra cứu đơn hàng theo SĐT.
    /// </summary>
    private (string Token, DateTime ExpiresAt) GeneratePhoneLookupToken(string phone)
    {
        var jwtSettings = _config.GetSection("Jwt");
        var keyBytes = Encoding.UTF8.GetBytes(
            jwtSettings["Key"] ?? "PhoneStoreManagementSystem_SuperSecretKey_2026_AspNetCore10_StrongSecretKey!");

        var expiresAt = DateTime.UtcNow.AddMinutes(10);
        var claims = new[]
        {
            new Claim(OtpTokenValidator.PhoneLookupClaim, phone),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"] ?? "PhoneStoreAPI",
            audience: jwtSettings["Audience"] ?? "PhoneStoreClient",
            claims: claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256)
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    /// <summary>
    /// Hàm tích hợp gửi OTP duy nhất: sendOtp(phone, code).
    /// OTP_MODE=demo (mặc định): log ra console và trả code về giao diện.
    /// OTP_MODE=sms: TODO gắn eSMS/SpeedSMS/Zalo ZNS (không trả code về client).
    /// </summary>
    private Task SendOtpAsync(string phone, string code)
    {
        var mode = GetOtpMode();

        if (string.Equals(mode, "demo", StringComparison.OrdinalIgnoreCase))
        {
            // DEMO MODE: log OTP ra console (chỉ dùng trong môi trường phát triển)
            _logger.LogInformation("[OTP DEMO] SĐT: {Phone} — Mã OTP: {Code}", phone, code);
            Console.WriteLine($"[OTP DEMO] SĐT: {phone} | OTP: {code}");
        }
        else
        {
            // TODO: Tích hợp eSMS / SpeedSMS / Zalo ZNS tại đây khi chuyển sang OTP_MODE=sms
            _logger.LogWarning("[OTP] SMS gateway chưa được cấu hình (OTP_MODE={Mode}). SĐT: {Phone}", mode, phone);
        }

        return Task.CompletedTask;
    }
}
