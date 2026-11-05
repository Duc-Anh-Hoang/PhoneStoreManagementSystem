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

public class StaffServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_WithoutMaNV_AutoGeneratesSequentialCode()
    {
        // Arrange
        using var context = CreateDbContext();
        var service = new StaffService(context, NullLogger<StaffService>.Instance);

        // Act
        var staff1 = await service.CreateAsync(new CreateNhanVienRequest
        {
            HoTen = "Nhân viên Một",
            ChucVu = "Bán hàng",
            Email = "nv1@phonestore.vn"
        });

        var staff2 = await service.CreateAsync(new CreateNhanVienRequest
        {
            HoTen = "Nhân viên Hai",
            ChucVu = "Thu ngân",
            Email = "nv2@phonestore.vn"
        });

        // Assert
        Assert.Equal("NV001", staff1.MaNV);
        Assert.Equal("NV002", staff2.MaNV);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateEmail_ThrowsConflictException()
    {
        // Arrange
        using var context = CreateDbContext();
        var service = new StaffService(context, NullLogger<StaffService>.Instance);

        await service.CreateAsync(new CreateNhanVienRequest
        {
            HoTen = "Nhân viên Một",
            ChucVu = "Bán hàng",
            Email = "duplicate@phonestore.vn"
        });

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ConflictException>(() =>
            service.CreateAsync(new CreateNhanVienRequest
            {
                HoTen = "Nhân viên Hai",
                ChucVu = "Thu ngân",
                Email = "duplicate@phonestore.vn"
            }));

        Assert.Equal(HttpStatusCode.Conflict, ex.StatusCode);
        Assert.Contains("đã được sử dụng", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_StaffWithNoDocuments_DeletesCompletely()
    {
        // Arrange
        using var context = CreateDbContext();
        var staff = new NhanVien
        {
            MaNV = "NV010",
            HoTen = "Nhân viên Mới",
            ChucVu = "Kho",
            Email = "nv10@phonestore.vn",
            TrangThai = AppConstants.EmployeeStatus.DangLam
        };
        context.NhanViens.Add(staff);
        await context.SaveChangesAsync();

        var service = new StaffService(context, NullLogger<StaffService>.Instance);

        // Act
        var (isDeleted, message) = await service.DeleteAsync("NV010");

        // Assert
        Assert.True(isDeleted);
        Assert.Contains("xóa", message);
        Assert.Null(await context.NhanViens.FirstOrDefaultAsync(n => n.MaNV == "NV010"));
    }

    [Fact]
    public async Task DeleteAsync_StaffWithInvoices_MarksAsDaNghi()
    {
        // Arrange
        using var context = CreateDbContext();
        var staff = new NhanVien
        {
            MaNV = "NV020",
            HoTen = "Nhân viên Lâu Năm",
            ChucVu = "Bán hàng",
            Email = "nv20@phonestore.vn",
            TrangThai = AppConstants.EmployeeStatus.DangLam
        };
        var invoice = new HoaDon
        {
            SoHD = "HD261100001",
            MaNV = "NV020",
            NgayLap = DateTime.UtcNow,
            KenhBan = AppConstants.SaleChannels.TaiQuay
        };

        context.NhanViens.Add(staff);
        context.HoaDons.Add(invoice);
        await context.SaveChangesAsync();

        var service = new StaffService(context, NullLogger<StaffService>.Instance);

        // Act
        var (isDeleted, message) = await service.DeleteAsync("NV020");

        // Assert
        Assert.False(isDeleted);
        Assert.Contains("da_nghi", message);

        var updatedStaff = await context.NhanViens.FirstOrDefaultAsync(n => n.MaNV == "NV020");
        Assert.NotNull(updatedStaff);
        Assert.Equal(AppConstants.EmployeeStatus.DaNghi, updatedStaff.TrangThai);
    }
}
