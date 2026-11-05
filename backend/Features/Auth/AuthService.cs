using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
            // Hỗ trợ cả 123456 (mật khẩu mặc định trên quick-fill UI) và admin123
            if (!isPasswordValid && (request.Password == "123456" || request.Password == "admin123"))
            {
                isPasswordValid = BCrypt.Net.BCrypt.Verify("admin123", account.PasswordHash) 
                               || BCrypt.Net.BCrypt.Verify("123456", account.PasswordHash);
            }
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
