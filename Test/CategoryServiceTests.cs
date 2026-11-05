using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Products.Categories;
using PhoneStoreAPI.Features.Products.Categories.DTOs;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace PhoneStoreAPI.Tests;

public class CategoryServiceTests
{
    private AppDbContext CreateInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task CreateNhomHangAsync_DuplicateTenNhom_ThrowsConflictException()
    {
        // Arrange
        var context = CreateInMemoryDbContext(nameof(CreateNhomHangAsync_DuplicateTenNhom_ThrowsConflictException));
        context.NhomHangs.Add(new NhomHang { MaNhom = "DIENTHOAI", TenNhom = "Điện thoại" });
        await context.SaveChangesAsync();

        var service = new CategoryService(context);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => service.CreateNhomHangAsync(new CreateNhomHangRequest
        {
            MaNhom = "DT_NEW",
            TenNhom = "Điện thoại" // Trùng tên
        }));
    }

    [Fact]
    public async Task DeleteNhomHangAsync_WhenHasProducts_ThrowsConflictException()
    {
        // Arrange
        var context = CreateInMemoryDbContext(nameof(DeleteNhomHangAsync_WhenHasProducts_ThrowsConflictException));
        context.NhomHangs.Add(new NhomHang { MaNhom = "DIENTHOAI", TenNhom = "Điện thoại" });
        context.LoaiHangs.Add(new LoaiHang { MaLoai = "APPLE", TenLoai = "Apple" });
        context.MatHangs.Add(new MatHang
        {
            MaHang = "IP15",
            TenHang = "iPhone 15",
            DonGia = 20000000,
            MaNhom = "DIENTHOAI",
            MaLoai = "APPLE"
        });
        await context.SaveChangesAsync();

        var service = new CategoryService(context);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => service.DeleteNhomHangAsync("DIENTHOAI"));
    }
}
