using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Products;
using PhoneStoreAPI.Features.Products.DTOs;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace PhoneStoreAPI.Tests;

public class ProductServiceTests
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
    public async Task GetProductsAsync_FilterAndPaging_ReturnsCorrectResult()
    {
        // Arrange
        var context = CreateInMemoryDbContext(nameof(GetProductsAsync_FilterAndPaging_ReturnsCorrectResult));
        context.NhomHangs.Add(new NhomHang { MaNhom = "DIENTHOAI", TenNhom = "Điện thoại" });
        context.LoaiHangs.Add(new LoaiHang { MaLoai = "APPLE", TenLoai = "Apple" });

        for (int i = 1; i <= 25; i++)
        {
            context.MatHangs.Add(new MatHang
            {
                MaHang = $"IPHONE_{i:D2}",
                TenHang = $"iPhone 15 Pro Model {i}",
                DonGia = 20000000 + i * 100000,
                MaNhom = "DIENTHOAI",
                MaLoai = "APPLE",
                Dvt = "Cái",
                QuanLyImei = true,
                ThoiHanBaoHanh = 12
            });
        }
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        // Act - Trang 1, pageSize = 10, tìm từ khóa "Model 1"
        var query = new ProductQueryParameters
        {
            Keyword = "Model 1",
            Page = 1,
            PageSize = 10,
            Sort = "giaTang"
        };
        var (items, totalCount) = await service.GetProductsAsync(query);

        // Assert
        Assert.True(totalCount > 0);
        Assert.True(items.Count <= 10);
        Assert.All(items, item => Assert.Contains("Model 1", item.TenHang));
    }

    [Fact]
    public async Task CreateProductAsync_DuplicateMaHang_ThrowsConflictException()
    {
        // Arrange
        var context = CreateInMemoryDbContext(nameof(CreateProductAsync_DuplicateMaHang_ThrowsConflictException));
        context.NhomHangs.Add(new NhomHang { MaNhom = "DIENTHOAI", TenNhom = "Điện thoại" });
        context.LoaiHangs.Add(new LoaiHang { MaLoai = "APPLE", TenLoai = "Apple" });
        context.MatHangs.Add(new MatHang
        {
            MaHang = "IP15PM",
            TenHang = "iPhone 15 Pro Max",
            DonGia = 30000000,
            MaNhom = "DIENTHOAI",
            MaLoai = "APPLE"
        });
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var request = new CreateProductRequest
        {
            MaHang = "IP15PM",
            TenHang = "iPhone 15 Pro Max New",
            DonGia = 29000000,
            MaNhom = "DIENTHOAI",
            MaLoai = "APPLE"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => service.CreateProductAsync(request));
    }

    [Fact]
    public async Task UpdateProductAsync_ChangeQuanLyImei_WhenHasImei_ThrowsConflictException()
    {
        // Arrange
        var context = CreateInMemoryDbContext(nameof(UpdateProductAsync_ChangeQuanLyImei_WhenHasImei_ThrowsConflictException));
        context.NhomHangs.Add(new NhomHang { MaNhom = "DIENTHOAI", TenNhom = "Điện thoại" });
        context.LoaiHangs.Add(new LoaiHang { MaLoai = "APPLE", TenLoai = "Apple" });
        context.MatHangs.Add(new MatHang
        {
            MaHang = "IP15",
            TenHang = "iPhone 15",
            DonGia = 20000000,
            MaNhom = "DIENTHOAI",
            MaLoai = "APPLE",
            QuanLyImei = true
        });
        context.IMEIs.Add(new IMEI
        {
            MaIMEI = "IMEI001",
            SoIMEI = "123456789012345",
            MaHang = "IP15",
            SoPN = "PN001",
            TrangThai = "con_hang"
        });
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var updateRequest = new UpdateProductRequest
        {
            TenHang = "iPhone 15",
            DonGia = 20000000,
            MaNhom = "DIENTHOAI",
            MaLoai = "APPLE",
            QuanLyImei = false // Đổi từ true -> false khi đã có IMEI
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => service.UpdateProductAsync("IP15", updateRequest));
    }

    [Fact]
    public async Task DeleteProductAsync_WhenReferencedInInvoice_ThrowsConflictException()
    {
        // Arrange
        var context = CreateInMemoryDbContext(nameof(DeleteProductAsync_WhenReferencedInInvoice_ThrowsConflictException));
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
        context.ChiTietHoaDons.Add(new ChiTietHoaDon
        {
            SoHD = "HD001",
            MaHang = "IP15",
            SlBan = 1,
            DonGiaBan = 20000000
        });
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => service.DeleteProductAsync("IP15"));
    }
}
