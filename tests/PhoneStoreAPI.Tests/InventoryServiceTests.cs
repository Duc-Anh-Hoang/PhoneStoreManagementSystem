using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Inventory;
using PhoneStoreAPI.Features.Inventory.DTOs;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace PhoneStoreAPI.Tests;

public class InventoryServiceTests
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
    public async Task GetAvailableImeisAsync_OrdersByNgayNhapAscending_FIFO()
    {
        // Arrange
        var context = CreateInMemoryDbContext(nameof(GetAvailableImeisAsync_OrdersByNgayNhapAscending_FIFO));
        var maHang = "IP15";
        context.MatHangs.Add(new MatHang { MaHang = maHang, TenHang = "iPhone 15", DonGia = 20000000 });

        // Tạo 3 phiếu nhập với các ngày nhập khác nhau
        context.PhieuNhaps.AddRange(
            new PhieuNhap { SoPN = "PN003", NgayNhap = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc), MaNV = "NV001", MaNCC = "NCC01" },
            new PhieuNhap { SoPN = "PN001", NgayNhap = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), MaNV = "NV001", MaNCC = "NCC01" },
            new PhieuNhap { SoPN = "PN002", NgayNhap = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc), MaNV = "NV001", MaNCC = "NCC01" }
        );

        // Tạo IMEI
        context.IMEIs.AddRange(
            new IMEI { MaIMEI = "IMEI3", SoIMEI = "333333333333333", MaHang = maHang, SoPN = "PN003", TrangThai = "con_hang" },
            new IMEI { MaIMEI = "IMEI1", SoIMEI = "111111111111111", MaHang = maHang, SoPN = "PN001", TrangThai = "con_hang" },
            new IMEI { MaIMEI = "IMEI2", SoIMEI = "222222222222222", MaHang = maHang, SoPN = "PN002", TrangThai = "con_hang" },
            new IMEI { MaIMEI = "IMEI_SOLD", SoIMEI = "999999999999999", MaHang = maHang, SoPN = "PN001", TrangThai = "da_ban" }
        );
        await context.SaveChangesAsync();

        var service = new InventoryService(context);

        // Act
        var availableImeis = await service.GetAvailableImeisAsync(maHang);

        // Assert
        Assert.Equal(3, availableImeis.Count);
        // Kiểm tra thứ tự FIFO: PN001 (tháng 1) -> PN002 (tháng 2) -> PN003 (tháng 3)
        Assert.Equal("111111111111111", availableImeis[0].SoIMEI);
        Assert.Equal("222222222222222", availableImeis[1].SoIMEI);
        Assert.Equal("333333333333333", availableImeis[2].SoIMEI);
    }
}
