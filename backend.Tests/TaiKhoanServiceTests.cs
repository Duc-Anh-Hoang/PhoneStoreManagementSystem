using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Staff;
using PhoneStoreAPI.Features.Staff.DTOs;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class TaiKhoanServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task UpdateStatusAsync_SelfLocking_ThrowsBadRequestException()
    {
        // Arrange
        using var context = CreateDbContext();
        var adminRole = new PhanQuyen { MaQuyen = AppConstants.Roles.Admin, TenQuyen = "Quản trị viên" };
        var staff = new NhanVien
        {
            MaNV = "NV001",
            HoTen = "Admin Test",
            ChucVu = "Admin",
            Email = "admin@phonestore.vn",
            TrangThai = AppConstants.EmployeeStatus.DangLam
        };
        var admin = new TaiKhoan
        {
            UserName = "admin",
            PasswordHash = "hash",
            MaNV = "NV001",
            MaQuyen = AppConstants.Roles.Admin,
            TrangThai = AppConstants.AccountStatus.HoatDong,
            NgayTao = DateTime.UtcNow
        };
        context.PhanQuyens.Add(adminRole);
        context.NhanViens.Add(staff);
        context.TaiKhoans.Add(admin);
        await context.SaveChangesAsync();

        var service = new TaiKhoanService(context, NullLogger<TaiKhoanService>.Instance);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() =>
            service.UpdateStatusAsync("admin", AppConstants.AccountStatus.BiKhoa, currentUsername: "admin"));

        Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        Assert.Contains("tự khóa", ex.Message);
    }

    [Fact]
    public async Task UpdateStatusAsync_LockingLastActiveAdmin_ThrowsBadRequestException()
    {
        // Arrange
        using var context = CreateDbContext();
        var adminRole = new PhanQuyen { MaQuyen = AppConstants.Roles.Admin, TenQuyen = "Quản trị viên" };
        var staff = new NhanVien
        {
            MaNV = "NV001",
            HoTen = "Admin Test",
            ChucVu = "Admin",
            Email = "admin@phonestore.vn",
            TrangThai = AppConstants.EmployeeStatus.DangLam
        };
        var admin = new TaiKhoan
        {
            UserName = "admin",
            PasswordHash = "hash",
            MaNV = "NV001",
            MaQuyen = AppConstants.Roles.Admin,
            TrangThai = AppConstants.AccountStatus.HoatDong,
            NgayTao = DateTime.UtcNow
        };
        context.PhanQuyens.Add(adminRole);
        context.NhanViens.Add(staff);
        context.TaiKhoans.Add(admin);
        await context.SaveChangesAsync();

        var service = new TaiKhoanService(context, NullLogger<TaiKhoanService>.Instance);

        // Act & Assert (another user trying to lock the only active admin)
        var ex = await Assert.ThrowsAsync<BadRequestException>(() =>
            service.UpdateStatusAsync("admin", AppConstants.AccountStatus.BiKhoa, currentUsername: "otheruser"));

        Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        Assert.Contains("admin đang hoạt động cuối cùng", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_DemotingLastActiveAdmin_ThrowsBadRequestException()
    {
        // Arrange
        using var context = CreateDbContext();
        var adminRole = new PhanQuyen { MaQuyen = AppConstants.Roles.Admin, TenQuyen = "Quản trị viên" };
        var salesRole = new PhanQuyen { MaQuyen = AppConstants.Roles.Sales, TenQuyen = "Nhân viên bán hàng" };
        var staff = new NhanVien
        {
            MaNV = "NV001",
            HoTen = "Admin Test",
            ChucVu = "Admin",
            Email = "admin@phonestore.vn",
            TrangThai = AppConstants.EmployeeStatus.DangLam
        };
        var admin = new TaiKhoan
        {
            UserName = "admin",
            PasswordHash = "hash",
            MaNV = "NV001",
            MaQuyen = AppConstants.Roles.Admin,
            TrangThai = AppConstants.AccountStatus.HoatDong,
            NgayTao = DateTime.UtcNow
        };

        context.PhanQuyens.AddRange(adminRole, salesRole);
        context.NhanViens.Add(staff);
        context.TaiKhoans.Add(admin);
        await context.SaveChangesAsync();

        var service = new TaiKhoanService(context, NullLogger<TaiKhoanService>.Instance);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() =>
            service.UpdateAsync("admin", new UpdateTaiKhoanRequest
            {
                MaQuyen = AppConstants.Roles.Sales
            }, currentUsername: "otheradmin"));

        Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        Assert.Contains("admin đang hoạt động cuối cùng", ex.Message);
    }
}
