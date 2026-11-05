using System.Net;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Warranty;
using PhoneStoreAPI.Features.Warranty.DTOs;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class WarrantyServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private async Task SeedDataAsync(AppDbContext context)
    {
        var nhom = new NhomHang { MaNhom = "DT", TenNhom = "Điện thoại" };
        var loai = new LoaiHang { MaLoai = "IP", TenLoai = "iPhone" };
        context.NhomHangs.Add(nhom);
        context.LoaiHangs.Add(loai);

        var nvTech = new NhanVien
        {
            MaNV = "NV_TECH",
            HoTen = "Kỹ thuật viên 1",
            ChucVu = "Kỹ thuật & Bảo hành",
            TrangThai = AppConstants.EmployeeStatus.DangLam
        };
        var nvSales = new NhanVien
        {
            MaNV = "NV_SALES",
            HoTen = "Bán hàng 1",
            ChucVu = "Bán hàng",
            TrangThai = AppConstants.EmployeeStatus.DangLam
        };
        context.NhanViens.AddRange(nvTech, nvSales);

        var matHang = new MatHang
        {
            MaHang = "MH01",
            TenHang = "iPhone 15",
            DonGia = 20000000,
            Dvt = "Cái",
            MaNhom = "DT",
            MaLoai = "IP",
            QuanLyImei = true,
            ThoiHanBaoHanh = 12
        };
        var matHangKhongBH = new MatHang
        {
            MaHang = "MH_NO_BH",
            TenHang = "Ốp lưng",
            DonGia = 100000,
            Dvt = "Cái",
            MaNhom = "DT",
            MaLoai = "IP",
            QuanLyImei = false,
            ThoiHanBaoHanh = 0
        };
        context.MatHangs.AddRange(matHang, matHangKhongBH);

        var pn = new PhieuNhap { SoPN = "PN01", MaNCC = "NCC01", MaNV = "NV_SALES" };
        context.PhieuNhaps.Add(pn);

        var ctpn = new ChiTietPhieuNhap { SoPN = "PN01", MaHang = "MH01", SlNhap = 2, DonGiaNhap = 15000000 };
        context.ChiTietPhieuNhaps.Add(ctpn);

        var imei1 = new IMEI
        {
            MaIMEI = "IMEI01",
            SoIMEI = "860123456789001",
            MaHang = "MH01",
            SoPN = "PN01",
            TrangThai = "da_ban"
        };
        var imeiHetHan = new IMEI
        {
            MaIMEI = "IMEI_EXPIRED",
            SoIMEI = "860123456789099",
            MaHang = "MH01",
            SoPN = "PN01",
            TrangThai = "da_ban"
        };
        context.IMEIs.AddRange(imei1, imeiHetHan);

        var hd = new HoaDon
        {
            SoHD = "HD01",
            MaNV = "NV_SALES",
            NgayLap = DateTime.UtcNow.AddMonths(-1),
            PhuongThucThanhToan = "tien_mat",
            TongTien = 20000000
        };
        context.HoaDons.Add(hd);

        var cthd = new ChiTietHoaDon
        {
            SoHD = "HD01",
            MaHang = "MH01",
            MaIMEI = "IMEI01",
            SlBan = 1,
            DonGiaBan = 20000000
        };
        context.ChiTietHoaDons.Add(cthd);

        var bhConHan = new BaoHanh
        {
            MaBH = "BH01",
            SoHD = "HD01",
            MaIMEI = "IMEI01",
            NgayBD = DateTime.UtcNow.AddMonths(-1),
            ThoiHanThang = 12,
            NgayKT = DateTime.UtcNow.AddMonths(11),
            DieuKien = "Chính hãng"
        };
        var bhHetHan = new BaoHanh
        {
            MaBH = "BH_EXPIRED",
            SoHD = "HD01",
            MaIMEI = "IMEI_EXPIRED",
            NgayBD = DateTime.UtcNow.AddMonths(-14),
            ThoiHanThang = 12,
            NgayKT = DateTime.UtcNow.AddMonths(-2),
            DieuKien = "Chính hãng"
        };
        context.BaoHanhs.AddRange(bhConHan, bhHetHan);

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateForInvoiceAsync_ValidWarrantyProduct_CreatesBaoHanhRecord()
    {
        // Arrange
        using var context = CreateDbContext();
        await SeedDataAsync(context);

        var imei2 = new IMEI
        {
            MaIMEI = "IMEI02",
            SoIMEI = "860123456789002",
            MaHang = "MH01",
            SoPN = "PN01",
            TrangThai = "con_hang"
        };
        context.IMEIs.Add(imei2);
        await context.SaveChangesAsync();

        var codeGen = new CodeGenerator(context);
        var currentUser = new TestCurrentUser { MaNV = "NV_SALES" };
        var service = new WarrantyService(context, codeGen, currentUser);

        // Act
        await service.CreateForInvoiceAsync("HD01", "IMEI02", "MH01");

        // Assert
        var created = await context.BaoHanhs.FirstOrDefaultAsync(b => b.MaIMEI == "IMEI02");
        Assert.NotNull(created);
        Assert.Equal("HD01", created.SoHD);
        Assert.Equal(12, created.ThoiHanThang);
        Assert.NotNull(created.NgayKT);
        Assert.True(created.NgayKT > created.NgayBD);
    }

    [Fact]
    public async Task CreateForInvoiceAsync_ProductZeroWarranty_SkipsCreation()
    {
        // Arrange
        using var context = CreateDbContext();
        await SeedDataAsync(context);

        var codeGen = new CodeGenerator(context);
        var currentUser = new TestCurrentUser { MaNV = "NV_SALES" };
        var service = new WarrantyService(context, codeGen, currentUser);

        // Act
        await service.CreateForInvoiceAsync("HD01", "IMEI_NONE", "MH_NO_BH");

        // Assert
        var created = await context.BaoHanhs.FirstOrDefaultAsync(b => b.MaIMEI == "IMEI_NONE");
        Assert.Null(created);
    }

    [Fact]
    public async Task GetByImeiAsync_ExistingImei_ReturnsDtoWithoutCustomerPII()
    {
        // Arrange
        using var context = CreateDbContext();
        await SeedDataAsync(context);

        var codeGen = new CodeGenerator(context);
        var currentUser = new TestCurrentUser();
        var service = new WarrantyService(context, codeGen, currentUser);

        // Act
        var result = await service.GetByImeiAsync("860123456789001");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("860123456789001", result.SoIMEI);
        Assert.Equal("iPhone 15", result.TenHang);
        Assert.Equal("con_han", result.TrangThai);
        Assert.True(result.SoNgayConLai > 0);
    }

    [Fact]
    public async Task GetByImeiAsync_NonExistentImei_ThrowsNotFoundException()
    {
        // Arrange
        using var context = CreateDbContext();
        await SeedDataAsync(context);

        var codeGen = new CodeGenerator(context);
        var currentUser = new TestCurrentUser();
        var service = new WarrantyService(context, codeGen, currentUser);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByImeiAsync("999999999999999"));
    }

    [Fact]
    public async Task CreateTiepNhanAsync_ValidRequest_CreatesTicketWithInitialState()
    {
        // Arrange
        using var context = CreateDbContext();
        await SeedDataAsync(context);

        var codeGen = new CodeGenerator(context);
        var currentUser = new TestCurrentUser { MaNV = "NV_SALES" };
        var service = new WarrantyService(context, codeGen, currentUser);

        var request = new CreateTiepNhanRequest
        {
            SoIMEI = "860123456789001",
            TinhTrangLoi = "Màn hình bị sọc xanh",
            NgayHenTra = DateTime.UtcNow.AddDays(3)
        };

        // Act
        var result = await service.CreateTiepNhanAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("TN", result.MaTN);
        Assert.Equal("tiep_nhan", result.TrangThai);
        Assert.Equal("NV_SALES", result.MaNVTiepNhan);
    }

    [Fact]
    public async Task CreateTiepNhanAsync_ExpiredWarranty_ThrowsBadRequestException()
    {
        // Arrange
        using var context = CreateDbContext();
        await SeedDataAsync(context);

        var codeGen = new CodeGenerator(context);
        var currentUser = new TestCurrentUser { MaNV = "NV_SALES" };
        var service = new WarrantyService(context, codeGen, currentUser);

        var request = new CreateTiepNhanRequest
        {
            SoIMEI = "860123456789099",
            TinhTrangLoi = "Hỏng pin",
            NgayHenTra = DateTime.UtcNow.AddDays(3)
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.CreateTiepNhanAsync(request));
        Assert.Contains("hết hạn", ex.Message);
    }

    [Fact]
    public async Task UpdateTiepNhanAsync_AssignNonTechnician_ThrowsBadRequestException()
    {
        // Arrange
        using var context = CreateDbContext();
        await SeedDataAsync(context);

        var ticket = new PhieuTiepNhanBaoHanh
        {
            MaTN = "TN01",
            MaBH = "BH01",
            TinhTrangLoi = "Loa rè",
            MaNVTiepNhan = "NV_SALES",
            TrangThai = "tiep_nhan"
        };
        context.PhieuTiepNhanBaoHanhs.Add(ticket);
        await context.SaveChangesAsync();

        var codeGen = new CodeGenerator(context);
        var currentUser = new TestCurrentUser { MaNV = "NV_SALES" };
        var service = new WarrantyService(context, codeGen, currentUser);

        var request = new UpdateTiepNhanRequest
        {
            MaNVKyThuat = "NV_SALES", // Nhân viên bán hàng, không phải kỹ thuật
            ChiPhi = 100000,
            KetQua = "Kiểm tra"
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateTiepNhanAsync("TN01", request));
        Assert.Contains("Kỹ thuật", ex.Message);
    }

    [Theory]
    [InlineData("tiep_nhan", "dang_sua")]
    [InlineData("dang_sua", "cho_linh_kien")]
    [InlineData("cho_linh_kien", "dang_sua")]
    [InlineData("dang_sua", "hoan_thanh")]
    [InlineData("cho_linh_kien", "hoan_thanh")]
    [InlineData("hoan_thanh", "da_tra")]
    public async Task UpdateTrangThaiAsync_ValidTransitions_Succeeds(string fromState, string toState)
    {
        // Arrange
        using var context = CreateDbContext();
        await SeedDataAsync(context);

        string maTN = $"TN_{fromState}_{toState}";
        var ticket = new PhieuTiepNhanBaoHanh
        {
            MaTN = maTN,
            MaBH = "BH01",
            TinhTrangLoi = "Sửa chữa",
            MaNVTiepNhan = "NV_SALES",
            TrangThai = fromState
        };
        context.PhieuTiepNhanBaoHanhs.Add(ticket);
        await context.SaveChangesAsync();

        var codeGen = new CodeGenerator(context);
        var currentUser = new TestCurrentUser { MaNV = "NV_TECH" };
        var service = new WarrantyService(context, codeGen, currentUser);

        // Act
        var result = await service.UpdateTrangThaiAsync(maTN, new UpdateTrangThaiRequest { TrangThai = toState });

        // Assert
        Assert.Equal(toState, result.TrangThai);
        if (toState == "da_tra")
        {
            Assert.NotNull(result.NgayTra);
        }
    }

    [Theory]
    [InlineData("tiep_nhan", "hoan_thanh")]
    [InlineData("tiep_nhan", "da_tra")]
    [InlineData("hoan_thanh", "dang_sua")]
    [InlineData("da_tra", "tiep_nhan")]
    public async Task UpdateTrangThaiAsync_InvalidTransitions_ThrowsConflictException(string fromState, string toState)
    {
        // Arrange
        using var context = CreateDbContext();
        await SeedDataAsync(context);

        string maTN = $"TN_INVALID_{fromState}_{toState}";
        var ticket = new PhieuTiepNhanBaoHanh
        {
            MaTN = maTN,
            MaBH = "BH01",
            TinhTrangLoi = "Lỗi",
            MaNVTiepNhan = "NV_SALES",
            TrangThai = fromState
        };
        context.PhieuTiepNhanBaoHanhs.Add(ticket);
        await context.SaveChangesAsync();

        var codeGen = new CodeGenerator(context);
        var currentUser = new TestCurrentUser { MaNV = "NV_TECH" };
        var service = new WarrantyService(context, codeGen, currentUser);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() =>
            service.UpdateTrangThaiAsync(maTN, new UpdateTrangThaiRequest { TrangThai = toState }));
    }
}
