using System.Net;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Customers;
using PhoneStoreAPI.Features.Customers.DTOs;
using PhoneStoreAPI.Features.Customers.Helpers;
using PhoneStoreAPI.Features.Customers.Validators;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class MockThamSoProvider : IThamSoProvider
{
    private readonly decimal _val;
    public MockThamSoProvider(decimal val = 100000m) => _val = val;
    public Task<decimal> GetAsync(string maThamSo) => Task.FromResult(_val);
    public void InvalidateCache(string? maThamSo = null) { }
}

public class CustomerServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Theory]
    [InlineData("+84987654321", "0987654321")]
    [InlineData("84987654321", "0987654321")]
    [InlineData("098 765 4321", "0987654321")]
    [InlineData("098-765-4321", "0987654321")]
    public void PhoneHelper_NormalizeVietnamesePhone_ReturnsStandardFormat(string raw, string expected)
    {
        var normalized = PhoneHelper.NormalizeVietnamesePhone(raw);
        Assert.Equal(expected, normalized);
        Assert.True(PhoneHelper.IsValidVietnamesePhone(normalized));
    }

    [Fact]
    public async Task CreateAsync_WithoutMaKH_AutoGeneratesSequentialCode()
    {
        // Arrange
        using var context = CreateDbContext();
        var currentUser = new TestCurrentUser();
        var thamSoProvider = new MockThamSoProvider();
        var service = new CustomerService(
            context,
            currentUser,
            thamSoProvider,
            new CreateCustomerRequestValidator(),
            new UpdateCustomerRequestValidator());

        // Act
        var c1 = await service.CreateAsync(new CreateCustomerRequest
        {
            HoTen = "Khách Hàng 1",
            Sdt = "+84987654321"
        });

        var c2 = await service.CreateAsync(new CreateCustomerRequest
        {
            HoTen = "Khách Hàng 2",
            Sdt = "0978123456"
        });

        // Assert
        Assert.Equal("KH001", c1.MaKH);
        Assert.Equal("0987654321", c1.Sdt);
        Assert.Equal("KH002", c2.MaKH);
        Assert.Equal("0978123456", c2.Sdt);
    }

    [Fact]
    public async Task CreateAsync_DuplicatePhone_ThrowsConflictException()
    {
        // Arrange
        using var context = CreateDbContext();
        var currentUser = new TestCurrentUser();
        var thamSoProvider = new MockThamSoProvider();
        var service = new CustomerService(
            context,
            currentUser,
            thamSoProvider,
            new CreateCustomerRequestValidator(),
            new UpdateCustomerRequestValidator());

        await service.CreateAsync(new CreateCustomerRequest
        {
            HoTen = "Khách Hàng 1",
            Sdt = "0987654321"
        });

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ConflictException>(() => service.CreateAsync(new CreateCustomerRequest
        {
            HoTen = "Khách Hàng 2",
            Sdt = "+84987654321" // Same phone in different format
        }));

        Assert.Equal(HttpStatusCode.Conflict, ex.StatusCode);
        Assert.Contains("đã được đăng ký", ex.Message);
    }

    [Fact]
    public async Task GetByPhoneAsync_CustomerRoleAccessingOtherCustomer_ThrowsForbidden()
    {
        // Arrange
        using var context = CreateDbContext();
        context.KhachHangs.Add(new KhachHang
        {
            MaKH = "KH001",
            HoTen = "Khách Hàng A",
            Sdt = "0987654321"
        });
        await context.SaveChangesAsync();

        var currentUser = new TestCurrentUser
        {
            Role = AppConstants.Roles.Customer,
            MaKH = "KH002" // Different customer ID
        };
        var thamSoProvider = new MockThamSoProvider();
        var service = new CustomerService(
            context,
            currentUser,
            thamSoProvider,
            new CreateCustomerRequestValidator(),
            new UpdateCustomerRequestValidator());

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ForbiddenException>(() => service.GetByPhoneAsync("0987654321"));
        Assert.Equal(HttpStatusCode.Forbidden, ex.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_CustomerWithInvoices_ThrowsConflictException()
    {
        // Arrange
        using var context = CreateDbContext();
        context.KhachHangs.Add(new KhachHang
        {
            MaKH = "KH010",
            HoTen = "Khách Hàng Có Đơn",
            Sdt = "0912345678"
        });
        context.HoaDons.Add(new HoaDon
        {
            SoHD = "HD999",
            MaKH = "KH010",
            MaNV = "NV001",
            NgayLap = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var currentUser = new TestCurrentUser();
        var thamSoProvider = new MockThamSoProvider();
        var service = new CustomerService(
            context,
            currentUser,
            thamSoProvider,
            new CreateCustomerRequestValidator(),
            new UpdateCustomerRequestValidator());

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ConflictException>(() => service.DeleteAsync("KH010"));
        Assert.Equal(HttpStatusCode.Conflict, ex.StatusCode);
        Assert.Contains("hóa đơn", ex.Message);
    }

    [Fact]
    public async Task LookupMembershipAsync_SmallOrder_AppliesDiscountWithoutOtp()
    {
        // Arrange
        using var context = CreateDbContext();
        context.KhachHangs.Add(new KhachHang
        {
            MaKH = "KH_VIP1",
            HoTen = "Lê Hoàng Nam",
            Sdt = "0978123456"
        });
        context.HoaDons.Add(new HoaDon
        {
            SoHD = "HD_VIP1",
            MaKH = "KH_VIP1",
            MaNV = "NV001",
            TongTien = 60_000_000m,
            TienGiamGia = 0m,
            NgayLap = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = new CustomerService(
            context,
            new TestCurrentUser(),
            new MockThamSoProvider(),
            new CreateCustomerRequestValidator(),
            new UpdateCustomerRequestValidator());

        // Act: Đơn nhỏ 5 triệu (dưới 15M, mức giảm 3% = 150k dưới 500k)
        var result = await service.LookupMembershipAsync(new MembershipLookupRequest
        {
            Sdt = "0978123456",
            CartTotal = 5_000_000m,
            UsePoints = false
        }, "127.0.0.1");

        // Assert
        Assert.True(result.Exists);
        Assert.Equal("Vàng", result.HangThanhVien);
        Assert.Equal(3, result.DiscountPct);
        Assert.Equal(150_000m, result.DiscountAmount);
        Assert.False(result.RequiresOtp); // KHÔNG OTP cho đơn nhỏ & vừa
        Assert.Equal("L*** H*** N***", result.TenVietTat); // Dữ liệu bị che an toàn
    }

    [Fact]
    public async Task LookupMembershipAsync_HighValueOrder_RequiresOtp()
    {
        // Arrange
        using var context = CreateDbContext();
        context.KhachHangs.Add(new KhachHang
        {
            MaKH = "KH_VIP2",
            HoTen = "Hoàng Đức Anh",
            Sdt = "0387310342"
        });
        context.HoaDons.Add(new HoaDon
        {
            SoHD = "HD_VIP2",
            MaKH = "KH_VIP2",
            MaNV = "NV001",
            TongTien = 150_000_000m,
            TienGiamGia = 0m,
            NgayLap = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = new CustomerService(
            context,
            new TestCurrentUser(),
            new MockThamSoProvider(),
            new CreateCustomerRequestValidator(),
            new UpdateCustomerRequestValidator());

        // Act: Đơn hàng lớn 30 triệu (vượt ngưỡng 15M và giảm 5% = 1.5M vượt 500k)
        var result = await service.LookupMembershipAsync(new MembershipLookupRequest
        {
            Sdt = "0387310342",
            CartTotal = 30_000_000m,
            UsePoints = false
        }, "127.0.0.1");

        // Assert
        Assert.True(result.Exists);
        Assert.Equal("Kim Cương", result.HangThanhVien);
        Assert.Equal(5, result.DiscountPct);
        Assert.Equal(1_500_000m, result.DiscountAmount);
        Assert.True(result.RequiresOtp); // BẮT BUỘC OTP cho đơn hàng vượt ngưỡng
        Assert.NotNull(result.OtpReason);
    }

    [Fact]
    public async Task LookupMembershipAsync_UsePointsOrVoucher_AlwaysRequiresOtp()
    {
        // Arrange
        using var context = CreateDbContext();
        context.KhachHangs.Add(new KhachHang
        {
            MaKH = "KH_VIP3",
            HoTen = "Nguyễn Văn A",
            Sdt = "0901234567"
        });
        context.HoaDons.Add(new HoaDon
        {
            SoHD = "HD_VIP3",
            MaKH = "KH_VIP3",
            MaNV = "NV001",
            TongTien = 25_000_000m,
            TienGiamGia = 0m,
            NgayLap = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = new CustomerService(
            context,
            new TestCurrentUser(),
            new MockThamSoProvider(),
            new CreateCustomerRequestValidator(),
            new UpdateCustomerRequestValidator());

        // Act: Đơn chỉ 2 triệu nhưng chọn dùng điểm tích lũy / voucher
        var result = await service.LookupMembershipAsync(new MembershipLookupRequest
        {
            Sdt = "0901234567",
            CartTotal = 2_000_000m,
            UsePoints = true,
            PointsToUse = 100
        }, "127.0.0.1");

        // Assert
        Assert.True(result.Exists);
        Assert.Equal("Bạc", result.HangThanhVien);
        Assert.True(result.RequiresOtp); // Luôn bắt OTP khi đụng tới điểm / voucher
    }
}
