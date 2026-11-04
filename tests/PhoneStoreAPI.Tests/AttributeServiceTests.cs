using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Products.Attributes;
using PhoneStoreAPI.Features.Products.Attributes.DTOs;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace PhoneStoreAPI.Tests;

public class AttributeServiceTests
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
    public async Task DeleteThuocTinhAsync_WhenAssignedToProduct_ThrowsConflictException()
    {
        // Arrange
        var context = CreateInMemoryDbContext(nameof(DeleteThuocTinhAsync_WhenAssignedToProduct_ThrowsConflictException));
        context.ThuocTinhs.Add(new ThuocTinh { MaThuocTinh = "RAM", TenThuocTinh = "Dung lượng RAM" });
        context.MatHangs.Add(new MatHang { MaHang = "IP15", TenHang = "iPhone 15", DonGia = 20000000 });
        context.ChiTietThuocTinhs.Add(new ChiTietThuocTinh { MaHang = "IP15", MaThuocTinh = "RAM", GiaTri = "8GB" });
        await context.SaveChangesAsync();

        var service = new AttributeService(context);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => service.DeleteThuocTinhAsync("RAM"));
    }
}
