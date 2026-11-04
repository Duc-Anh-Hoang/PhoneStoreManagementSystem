using System.Net;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Suppliers;
using PhoneStoreAPI.Features.Suppliers.DTOs;
using PhoneStoreAPI.Features.Suppliers.Validators;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class SupplierServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ValidSupplier_Succeeds()
    {
        // Arrange
        using var context = CreateDbContext();
        var service = new SupplierService(context, new CreateSupplierRequestValidator(), new UpdateSupplierRequestValidator());

        var req = new CreateSupplierRequest
        {
            MaNCC = "NCC_TEST",
            TenNCC = "Công ty TNHH Phân Phối Test",
            Sdt = "02838999999",
            Email = "ncc@test.com"
        };

        // Act
        var res = await service.CreateAsync(req);

        // Assert
        Assert.NotNull(res);
        Assert.Equal("NCC_TEST", res.MaNCC);
        Assert.Equal("Công ty TNHH Phân Phối Test", res.TenNCC);

        var inDb = await context.NhaCungCaps.FirstOrDefaultAsync(n => n.MaNCC == "NCC_TEST");
        Assert.NotNull(inDb);
    }

    [Fact]
    public async Task CreateAsync_DuplicateMaNCC_ThrowsConflictException()
    {
        // Arrange
        using var context = CreateDbContext();
        context.NhaCungCaps.Add(new NhaCungCap
        {
            MaNCC = "NCC01",
            TenNCC = "Nhà cung cấp 1"
        });
        await context.SaveChangesAsync();

        var service = new SupplierService(context, new CreateSupplierRequestValidator(), new UpdateSupplierRequestValidator());

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ConflictException>(() => service.CreateAsync(new CreateSupplierRequest
        {
            MaNCC = "NCC01",
            TenNCC = "Nhà cung cấp Trùng"
        }));

        Assert.Equal(HttpStatusCode.Conflict, ex.StatusCode);
        Assert.Contains("đã tồn tại", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_SupplierWithExistingPhieuNhap_ThrowsConflictException()
    {
        // Arrange
        using var context = CreateDbContext();
        context.NhaCungCaps.Add(new NhaCungCap
        {
            MaNCC = "NCC_CO_PN",
            TenNCC = "Nhà cung cấp có phiếu nhập"
        });
        context.PhieuNhaps.Add(new PhieuNhap
        {
            SoPN = "PN0001",
            MaNCC = "NCC_CO_PN",
            MaNV = "NV001",
            NgayNhap = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = new SupplierService(context, new CreateSupplierRequestValidator(), new UpdateSupplierRequestValidator());

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ConflictException>(() => service.DeleteAsync("NCC_CO_PN"));
        Assert.Equal(HttpStatusCode.Conflict, ex.StatusCode);
        Assert.Contains("đã phát sinh phiếu nhập kho", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_SupplierWithoutPhieuNhap_DeletesSuccessfully()
    {
        // Arrange
        using var context = CreateDbContext();
        context.NhaCungCaps.Add(new NhaCungCap
        {
            MaNCC = "NCC_DELETE",
            TenNCC = "Nhà cung cấp Xóa"
        });
        await context.SaveChangesAsync();

        var service = new SupplierService(context, new CreateSupplierRequestValidator(), new UpdateSupplierRequestValidator());

        // Act
        await service.DeleteAsync("NCC_DELETE");

        // Assert
        var inDb = await context.NhaCungCaps.FirstOrDefaultAsync(n => n.MaNCC == "NCC_DELETE");
        Assert.Null(inDb);
    }
}

