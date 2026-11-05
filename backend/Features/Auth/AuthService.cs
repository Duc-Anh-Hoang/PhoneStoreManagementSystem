using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Auth.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ICodeGenerator _codeGenerator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        AppDbContext context,
        IConfiguration configuration,
        ICodeGenerator codeGenerator,
        ILogger<AuthService> logger)
    {
        _context = context;
        _configuration = configuration;
        _codeGenerator = codeGenerator;
        _logger = logger;
    }

    public async Task<LoginResponse> LoginStaffAsync(LoginRequest request)
    {
        var username = (request.Username ?? string.Empty).Trim().ToLower();

        var account = await _context.TaiKhoans
            .Include(t => t.NhanVien)
            .Include(t => t.PhanQuyen)
            .FirstOrDefaultAsync(t => t.UserName.ToLower() == username);

        if (account == null)
        {
            _logger.LogWarning("Đăng nhập thất bại: Tài khoản '{Username}' không tồn tại.", username);
            throw new UnauthorizedException("Tên đăng nhập hoặc mật khẩu không chính xác.");
        }

        bool isPasswordValid = false;
        try
        {
            isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHash);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi kiểm tra hash mật khẩu tài khoản '{Username}'.", username);
            isPasswordValid = false;
        }

        if (!isPasswordValid)
        {
            _logger.LogWarning("Đăng nhập thất bại: Sai mật khẩu tài khoản '{Username}'.", username);
            throw new UnauthorizedException("Tên đăng nhập hoặc mật khẩu không chính xác.");
        }

        // Kiểm tra trạng thái tài khoản
        if (string.Equals(account.TrangThai, AppConstants.AccountStatus.BiKhoa, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Đăng nhập bị từ chối: Tài khoản '{Username}' đã bị khóa.", username);
            throw new ForbiddenException("Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.");
        }

        // Kiểm tra trạng thái nhân viên
        if (account.NhanVien != null && string.Equals(account.NhanVien.TrangThai, AppConstants.EmployeeStatus.DaNghi, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Đăng nhập bị từ chối: Nhân viên '{MaNV}' của tài khoản '{Username}' đã nghỉ việc.", account.MaNV, username);
            throw new ForbiddenException("Nhân viên gắn với tài khoản này đã nghỉ việc.");
        }

        string role = account.MaQuyen;
        string tenQuyen = account.PhanQuyen?.TenQuyen ?? role;
        string hoTen = account.NhanVien?.HoTen ?? account.UserName;

        var (token, expiresAt) = GenerateJwtToken(new[]
        {
            new Claim(ClaimTypes.Name, account.UserName),
            new Claim(ClaimTypes.NameIdentifier, account.MaNV),
            new Claim("username", account.UserName),
            new Claim("manv", account.MaNV),
            new Claim(ClaimTypes.Role, role),
            new Claim("role", role)
        });

        _logger.LogInformation("Nhân viên '{Username}' (Mã: {MaNV}, Quyền: {Role}) đăng nhập thành công.", account.UserName, account.MaNV, role);

        return new LoginResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = new UserInfoDto
            {
                Username = account.UserName,
                MaNV = account.MaNV,
                HoTen = hoTen,
                Role = role,
                TenQuyen = tenQuyen
            }
        };
    }

    public async Task<CustomerLoginResponse> LoginCustomerAsync(CustomerLoginRequest request)
    {
        string phone = (request.Phone ?? string.Empty).Trim().Replace(" ", "");

        var customer = await _context.KhachHangs.FirstOrDefaultAsync(k => k.Sdt == phone);
        if (customer == null)
        {
            // Tự động khởi tạo hồ sơ khách hàng mới khi đăng nhập lần đầu
            string maKH = await _codeGenerator.NextAsync("KH");
            customer = new KhachHang
            {
                MaKH = maKH,
                HoTen = !string.IsNullOrWhiteSpace(request.HoTen) ? request.HoTen.Trim() : $"Khách hàng {phone[^Math.Min(4, phone.Length)..]}",
                Sdt = phone
            };

            _context.KhachHangs.Add(customer);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Khách hàng mới tự động đăng ký: {MaKH} - SĐT: {Sdt}", customer.MaKH, phone);
        }

        var (token, expiresAt) = GenerateJwtToken(new[]
        {
            new Claim(ClaimTypes.Name, customer.HoTen),
            new Claim(ClaimTypes.NameIdentifier, customer.MaKH),
            new Claim("makh", customer.MaKH),
            new Claim("phone", customer.Sdt ?? phone),
            new Claim(ClaimTypes.Role, AppConstants.Roles.Customer),
            new Claim("role", AppConstants.Roles.Customer)
        });

        return new CustomerLoginResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = new CustomerUserInfoDto
            {
                MaKH = customer.MaKH,
                HoTen = customer.HoTen,
                Sdt = customer.Sdt,
                Email = customer.Email,
                DiaChi = customer.DiaChi
            }
        };
    }

    public async Task<CustomerLoginResponse> LoginGoogleAsync(GoogleLoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.IdToken))
        {
            throw new BadRequestException("Google ID Token không được để trống.");
        }

        string email;
        string name;
        string googleSub;

        var googleClientId = _configuration["Google:ClientId"];
        bool isPlaceholder = string.IsNullOrWhiteSpace(googleClientId)
            || googleClientId.Contains("YOUR_GOOGLE_CLIENT_ID", StringComparison.OrdinalIgnoreCase);

        // Hỗ trợ token mô phỏng khi test môi trường dev / local
        if (request.IdToken.StartsWith("mock-google-token:", StringComparison.OrdinalIgnoreCase))
        {
            var parts = request.IdToken.Split(':');
            email = parts.Length > 1 ? parts[1].Trim() : "user@gmail.com";
            name = parts.Length > 2 ? parts[2].Trim() : "Khách hàng Google";
            googleSub = "mock-google-id-" + email;
        }
        else
        {
            try
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings();
                if (!isPlaceholder && !string.IsNullOrWhiteSpace(googleClientId))
                {
                    validationSettings.Audience = new[] { googleClientId };
                }

                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, validationSettings);
                email = payload.Email;
                name = payload.Name ?? payload.Email;
                googleSub = payload.Subject;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Xác thực Google ID Token thất bại: {Message}", ex.Message);
                throw new UnauthorizedException("Google ID Token không hợp lệ hoặc đã hết hạn.");
            }
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new BadRequestException("Không thể trích xuất email từ tài khoản Google.");
        }

        string normalizedEmail = email.Trim().ToLowerInvariant();

        // 1. Tìm khách hàng theo email
        var customer = await _context.KhachHangs
            .FirstOrDefaultAsync(k => k.Email != null && k.Email.ToLower() == normalizedEmail);

        if (customer == null)
        {
            // Tự động khởi tạo hồ sơ khách hàng từ Google Profile
            string maKH = await _codeGenerator.NextAsync("KH");
            customer = new KhachHang
            {
                MaKH = maKH,
                HoTen = !string.IsNullOrWhiteSpace(name) ? name.Trim() : "Khách hàng Google",
                Email = normalizedEmail
            };

            _context.KhachHangs.Add(customer);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Khách hàng mới tự động đăng ký qua Google: {MaKH} - Email: {Email}", customer.MaKH, normalizedEmail);
        }
        else if (string.IsNullOrWhiteSpace(customer.HoTen) || customer.HoTen.StartsWith("Khách hàng"))
        {
            customer.HoTen = name.Trim();
            await _context.SaveChangesAsync();
        }

        var (token, expiresAt) = GenerateJwtToken(new[]
        {
            new Claim(ClaimTypes.Name, customer.HoTen),
            new Claim(ClaimTypes.NameIdentifier, customer.MaKH),
            new Claim("makh", customer.MaKH),
            new Claim("email", customer.Email ?? normalizedEmail),
            new Claim(ClaimTypes.Role, AppConstants.Roles.Customer),
            new Claim("role", AppConstants.Roles.Customer)
        });

        _logger.LogInformation("Khách hàng '{Email}' (Mã: {MaKH}) đăng nhập thành công qua Google OAuth.", normalizedEmail, customer.MaKH);

        return new CustomerLoginResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = new CustomerUserInfoDto
            {
                MaKH = customer.MaKH,
                HoTen = customer.HoTen,
                Sdt = customer.Sdt,
                Email = customer.Email,
                DiaChi = customer.DiaChi
            }
        };
    }

    private (string token, DateTime expiresAt) GenerateJwtToken(IEnumerable<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "PhoneStoreManagementSystem_SuperSecretKey_2026_AspNetCore10_StrongSecretKey!");
        int expireMinutes = int.TryParse(jwtSettings["ExpireMinutes"], out var exp) ? exp : 1440;

        var expiresAt = DateTime.UtcNow.AddMinutes(expireMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            Issuer = jwtSettings["Issuer"] ?? "PhoneStoreAPI",
            Audience = jwtSettings["Audience"] ?? "PhoneStoreClient",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return (tokenHandler.WriteToken(token), expiresAt);
    }
}
