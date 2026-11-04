using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Dashboard;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class DashboardServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private IMemoryCache CreateCache()
    {
        return new MemoryCache(new MemoryCacheOptions());
    }

    private async Task SeedDashboardDataAsync(AppDbContext context)
    {
        var nhomDT = new NhomHang { MaNhom = "DT", TenNhom = "Điện thoại" };
        var nhomPK = new NhomHang { MaNhom = "PK", TenNhom = "Phụ kiện" };
        context.NhomHangs.AddRange(nhomDT, nhomPK);

        var loai = new LoaiHang { MaLoai = "IP", TenLoai = "iPhone" };
        context.LoaiHangs.Add(loai);

        var mh1 = new MatHang { MaHang = "MH01", TenHang = "iPhone 15", DonGia = 20000000, Dvt = "Cái", MaNhom = "DT", MaLoai = "IP" };
        var mh2 = new MatHang { MaHang = "MH02", TenHang = "Củ sạc 20W", DonGia = 500000, Dvt = "Cái", MaNhom = "PK", MaLoai = "IP" };
        context.MatHangs.AddRange(mh1, mh2);

        var now = DateTime.UtcNow;
        var hdHomNay = new HoaDon
        {
            SoHD = "HD_TODAY",
            MaNV = "NV01",
            NgayLap = now,
            KenhBan = "tai_quay",
            TongTien = 20000000,
            TienGiamGia = 0,
            ThanhTien = 20000000,
            PhuongThucThanhToan = "tien_mat"
        };
        var hdThangNay = new HoaDon
        {
            SoHD = "HD_MONTH",
            MaNV = "NV01",
            NgayLap = new DateTime(now.Year, now.Month, 1, 12, 0, 0, DateTimeKind.Utc),
            KenhBan = "online",
            TongTien = 500000,
            TienGiamGia = 0,
            ThanhTien = 500000,
            PhuongThucThanhToan = "chuyen_khoan"
        };
        context.HoaDons.AddRange(hdHomNay, hdThangNay);

        var ctpn1 = new ChiTietPhieuNhap { SoPN = "PN01", MaHang = "MH01", SlNhap = 10, DonGiaNhap = 15000000 };
        var ctpn2 = new ChiTietPhieuNhap { SoPN = "PN01", MaHang = "MH02", SlNhap = 5, DonGiaNhap = 300000 };
        context.ChiTietPhieuNhaps.AddRange(ctpn1, ctpn2);

        var cthd1 = new ChiTietHoaDon { SoHD = "HD_TODAY", MaHang = "MH01", SlBan = 1, DonGiaBan = 20000000 };
        var cthd2 = new ChiTietHoaDon { SoHD = "HD_MONTH", MaHang = "MH02", SlBan = 1, DonGiaBan = 500000 };
        context.ChiTietHoaDons.AddRange(cthd1, cthd2);

        var donOnline = new DonHangOnline
        {
            MaDH = "DH01",
            MaKH = "KH01",
            TenNguoiNhan = "Khách 1",
            SdtNguoiNhan = "0987654321",
            DiaChiGiao = "Hà Nội",
            PhuongThucThanhToan = "cod",
            TongTien = 500000,
            TrangThai = "cho_xac_nhan"
        };
        context.DonHangOnlines.Add(donOnline);

        var bh = new BaoHanh
        {
            MaBH = "BH01",
            SoHD = "HD_TODAY",
            MaIMEI = "IMEI01",
            NgayBD = now,
            ThoiHanThang = 12
        };
        context.BaoHanhs.Add(bh);

        var tiepNhan = new PhieuTiepNhanBaoHanh
        {
            MaTN = "TN01",
            MaBH = "BH01",
            TinhTrangLoi = "Lỗi mic",
            MaNVTiepNhan = "NV01",
            TrangThai = "dang_sua"
        };
        context.PhieuTiepNhanBaoHanhs.Add(tiepNhan);

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetSummaryAsync_ComputesCorrectMetrics()
    {
        // Arrange
        using var context = CreateDbContext();
        await SeedDashboardDataAsync(context);
        var cache = CreateCache();
        var service = new DashboardService(context, cache);

        // Act
        var summary = await service.GetSummaryAsync();

        // Assert
        Assert.NotNull(summary);
        Assert.Equal(20000000m, summary.DoanhThuHomNay);
        Assert.Equal(20500000m, summary.DoanhThuThang);
        Assert.Equal(20500000m, summary.TongDoanhThu);
        Assert.Equal(1, summary.SoHoaDonHomNay);
        Assert.Equal(2, summary.TongHoaDon);
        Assert.Equal(1, summary.DonOnlineChoXacNhan);
        Assert.Equal(1, summary.SoBaoHanhDangXuLy);
        Assert.Equal(2, summary.TongMatHang);
        Assert.Equal(13, summary.TongTonKho);
        Assert.Equal(1, summary.SoMatHangSapHet);
    }

    [Fact]
    public async Task GetRevenueByChannelAsync_GroupsCorrectly()
    {
        // Arrange
        using var context = CreateDbContext();
        await SeedDashboardDataAsync(context);
        var cache = CreateCache();
        var service = new DashboardService(context, cache);

        // Act
        var result = await service.GetRevenueByChannelAsync(null, null);

        // Assert
        Assert.Equal(2, result.Count);
        var taiQuay = result.FirstOrDefault(x => x.KenhBan == "tai_quay");
        Assert.NotNull(taiQuay);
        Assert.Equal(20000000m, taiQuay.DoanhThu);

        var online = result.FirstOrDefault(x => x.KenhBan == "online");
        Assert.NotNull(online);
        Assert.Equal(500000m, online.DoanhThu);
    }

    [Fact]
    public async Task GetRevenueByCategoryAsync_GroupsAndOrdersByRevenue()
    {
        // Arrange
        using var context = CreateDbContext();
        await SeedDashboardDataAsync(context);
        var cache = CreateCache();
        var service = new DashboardService(context, cache);

        // Act
        var result = await service.GetRevenueByCategoryAsync(null, null);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal("DT", result[0].MaNhom);
        Assert.Equal(20000000m, result[0].DoanhThu);
    }
}
