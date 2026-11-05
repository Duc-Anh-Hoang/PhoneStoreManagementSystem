using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Auth;
using PhoneStoreAPI.Features.Auth.DTOs;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class AuthServiceTests
{
    private readonly IConfiguration _configuration;
    private readonly Mock<ICodeGenerator> _mockCodeGenerator;

    public AuthServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Jwt:Key", "super_secret_test_jwt_key_that_is_at_least_32_bytes_long_12345" },
            { "Jwt:Issuer", "PhoneStoreAPI" },
            { "Jwt:Audience", "PhoneStoreClient" },
            { "Jwt:ExpireMinutes", "60" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _mockCodeGenerator = new Mock<ICodeGenerator>();
    }

    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task LoginStaffAsync_WithValidCredentials_ReturnsLoginResponseWithValidJwtToken()
    {
        // Arrange
        using var context = CreateDbContext();
        var hashed = BCrypt.Net.BCrypt.HashPassword("admin123", 11);

        var role = new PhanQuyen { MaQuyen = AppConstants.Roles.Admin, TenQuyen = "Quản trị viên" };
        var staff = new NhanVien
        {
            MaNV = "NV001",
            HoTen = "Admin Test",
            ChucVu = "Admin",
            Email = "admin@phonestore.vn",
            TrangThai = AppConstants.EmployeeStatus.DangLam
        };
        var account = new TaiKhoan
        {
            UserName = "admin",
            PasswordHash = hashed,
            MaNV = "NV001",
            MaQuyen = AppConstants.Roles.Admin,
            TrangThai = AppConstants.AccountStatus.HoatDong,
            NgayTao = DateTime.UtcNow
        };

        context.PhanQuyens.Add(role);
        context.NhanViens.Add(staff);
        context.TaiKhoans.Add(account);
        await context.SaveChangesAsync();

        var service = new AuthService(context, _configuration, _mockCodeGenerator.Object, NullLogger<AuthService>.Instance);

        // Act
        var result = await service.LoginStaffAsync(new LoginRequest
        {
            Username = "admin",
            Password = "admin123"
        });

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.User);
        Assert.Equal("admin", result.User.Username);
        Assert.Equal("NV001", result.User.MaNV);
        Assert.Equal(AppConstants.Roles.Admin, result.User.Role);

        // Verify token is valid JWT
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(result.Token);
        Assert.NotNull(jwtToken);
        Assert.Equal("admin", jwtToken.Claims.First(c => c.Type == "username").Value);
        Assert.Equal(AppConstants.Roles.Admin, jwtToken.Claims.First(c => c.Type == "role").Value);
    }

    [Fact]
    public async Task LoginStaffAsync_WithInvalidPassword_ThrowsUnauthorizedException()
    {
        // Arrange
        using var context = CreateDbContext();
        var hashed = BCrypt.Net.BCrypt.HashPassword("correctpassword", 11);

        var account = new TaiKhoan
        {
            UserName = "testuser",
            PasswordHash = hashed,
            MaNV = "NV002",
            MaQuyen = AppConstants.Roles.Sales,
            TrangThai = AppConstants.AccountStatus.HoatDong,
            NgayTao = DateTime.UtcNow
        };
        context.TaiKhoans.Add(account);
        await context.SaveChangesAsync();

        var service = new AuthService(context, _configuration, _mockCodeGenerator.Object, NullLogger<AuthService>.Instance);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<UnauthorizedException>(() =>
            service.LoginStaffAsync(new LoginRequest
            {
                Username = "testuser",
                Password = "wrongpassword"
            }));

        Assert.Equal(HttpStatusCode.Unauthorized, ex.StatusCode);
    }

    [Fact]
    public async Task LoginStaffAsync_WithNonExistentUsername_ThrowsUnauthorizedException()
    {
        // Arrange
        using var context = CreateDbContext();
        var service = new AuthService(context, _configuration, _mockCodeGenerator.Object, NullLogger<AuthService>.Instance);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<UnauthorizedException>(() =>
            service.LoginStaffAsync(new LoginRequest
            {
                Username = "nonexistent",
                Password = "anyPassword123"
            }));

        Assert.Equal(HttpStatusCode.Unauthorized, ex.StatusCode);
    }

    [Fact]
    public async Task LoginStaffAsync_WithLockedAccount_ThrowsForbiddenException()
    {
        // Arrange
        using var context = CreateDbContext();
        var hashed = BCrypt.Net.BCrypt.HashPassword("secret123", 11);

        var role = new PhanQuyen { MaQuyen = AppConstants.Roles.Sales, TenQuyen = "Nhân viên bán hàng" };
        var staff = new NhanVien
        {
            MaNV = "NV003",
            HoTen = "Locked Staff",
            ChucVu = "Sales",
            Email = "locked@phonestore.vn",
            TrangThai = AppConstants.EmployeeStatus.DangLam
        };
        var account = new TaiKhoan
        {
            UserName = "lockeduser",
            PasswordHash = hashed,
            MaNV = "NV003",
            MaQuyen = AppConstants.Roles.Sales,
            TrangThai = AppConstants.AccountStatus.BiKhoa,
            NgayTao = DateTime.UtcNow
        };

        context.PhanQuyens.Add(role);
        context.NhanViens.Add(staff);
        context.TaiKhoans.Add(account);
        await context.SaveChangesAsync();

        var service = new AuthService(context, _configuration, _mockCodeGenerator.Object, NullLogger<AuthService>.Instance);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ForbiddenException>(() =>
            service.LoginStaffAsync(new LoginRequest
            {
                Username = "lockeduser",
                Password = "secret123"
            }));

        Assert.Equal(HttpStatusCode.Forbidden, ex.StatusCode);
        Assert.Contains("bị khóa", ex.Message);
    }

    [Fact]
    public async Task LoginStaffAsync_WithResignedEmployee_ThrowsForbiddenException()
    {
        // Arrange
        using var context = CreateDbContext();
        var hashed = BCrypt.Net.BCrypt.HashPassword("secret123", 11);

        var role = new PhanQuyen { MaQuyen = AppConstants.Roles.Sales, TenQuyen = "Nhân viên bán hàng" };
        var staff = new NhanVien
        {
            MaNV = "NV004",
            HoTen = "Resigned Staff",
            ChucVu = "Sales",
            Email = "resigned@phonestore.vn",
            TrangThai = AppConstants.EmployeeStatus.DaNghi
        };

        var account = new TaiKhoan
        {
            UserName = "resigneduser",
            PasswordHash = hashed,
            MaNV = "NV004",
            MaQuyen = AppConstants.Roles.Sales,
            TrangThai = AppConstants.AccountStatus.HoatDong,
            NgayTao = DateTime.UtcNow
        };

        context.PhanQuyens.Add(role);
        context.NhanViens.Add(staff);
        context.TaiKhoans.Add(account);
        await context.SaveChangesAsync();

        var service = new AuthService(context, _configuration, _mockCodeGenerator.Object, NullLogger<AuthService>.Instance);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ForbiddenException>(() =>
            service.LoginStaffAsync(new LoginRequest
            {
                Username = "resigneduser",
                Password = "secret123"
            }));

        Assert.Equal(HttpStatusCode.Forbidden, ex.StatusCode);
        Assert.Contains("nghỉ việc", ex.Message);
    }

    [Fact]
    public async Task LoginGoogleAsync_WithEmptyToken_ThrowsBadRequestException()
    {
        using var context = CreateDbContext();
        var service = new AuthService(context, _configuration, _mockCodeGenerator.Object, NullLogger<AuthService>.Instance);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.LoginGoogleAsync(new GoogleLoginRequest { IdToken = "" }));
    }

    [Fact]
    public async Task LoginGoogleAsync_WithMockToken_CreatesCustomerAndReturnsJwt()
    {
        using var context = CreateDbContext();
        _mockCodeGenerator.Setup(c => c.NextAsync("KH")).ReturnsAsync("KH009");

        var service = new AuthService(context, _configuration, _mockCodeGenerator.Object, NullLogger<AuthService>.Instance);

        var result = await service.LoginGoogleAsync(new GoogleLoginRequest
        {
            IdToken = "mock-google-token:googleuser@gmail.com:Nguyễn Văn Google"
        });

        Assert.NotNull(result);
        Assert.NotEmpty(result.Token);
        Assert.Equal("KH009", result.User.MaKH);
        Assert.Equal("Nguyễn Văn Google", result.User.HoTen);
        Assert.Equal("googleuser@gmail.com", result.User.Email);

        var savedCustomer = await context.KhachHangs.FirstOrDefaultAsync(k => k.MaKH == "KH009");
        Assert.NotNull(savedCustomer);
        Assert.Equal("googleuser@gmail.com", savedCustomer.Email);
    }

    [Fact]
    public async Task LoginGoogleAsync_WithExistingEmail_ReusesCustomerAndReturnsJwt()
    {
        using var context = CreateDbContext();
        var existing = new KhachHang
        {
            MaKH = "KH001",
            HoTen = "Đã Tồn Tại",
            Email = "existing@gmail.com",
            Sdt = "0901234567"
        };
        context.KhachHangs.Add(existing);
        await context.SaveChangesAsync();

        var service = new AuthService(context, _configuration, _mockCodeGenerator.Object, NullLogger<AuthService>.Instance);

        var result = await service.LoginGoogleAsync(new GoogleLoginRequest
        {
            IdToken = "mock-google-token:existing@gmail.com:Đã Tồn Tại Mới"
        });

        Assert.NotNull(result);
        Assert.Equal("KH001", result.User.MaKH);
        Assert.Equal("existing@gmail.com", result.User.Email);
    }
}
