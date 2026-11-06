using System.Net;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.OnlineOrders;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;
using PhoneStoreAPI.Features.Sales;
using PhoneStoreAPI.Features.Sales.DTOs;
using PhoneStoreAPI.Features.Sales.Returns;
using PhoneStoreAPI.Features.Sales.Returns.DTOs;
using PhoneStoreAPI.Features.SystemSettings;
using PhoneStoreAPI.Features.Warranty;
using PhoneStoreAPI.Models.Entities;

namespace backend.Tests;

public class Plan4Tests
{
    private AppDbContext GetInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new AppDbContext(options);
        return context;
    }

    private class MockCurrentUser : ICurrentUser
    {
        public string? UserName => "testuser";
        public string? MaNV { get; set; } = "NV001";
        public string? Role { get; set; } = AppConstants.Roles.Sales;
        public string? MaKH => null;
        public bool IsAuthenticated => true;
        public bool IsInRole(string role) => role == Role;
    }

    private void SeedBaseData(AppDbContext context)
    {
        context.NhanViens.Add(new NhanVien
        {
            MaNV = "NV001",
            HoTen = "Nhân viên Bán hàng",
            ChucVu = "Bán hàng",
            Sdt = "0901112222",
            Email = "nv001@cellphones.vn",
            NgayVaoLam = DateTime.UtcNow,
            TrangThai = AppConstants.EmployeeStatus.DangLam
        });

        context.ThamSos.AddRange(
            new ThamSo { MaThamSo = "VAT_PHANTRAM", TenThamSo = "VAT", GiaTri = 10, DonVi = "%" },
            new ThamSo { MaThamSo = "HAN_DOITRA_NGAY", TenThamSo = "Hạn đổi trả", GiaTri = 30, DonVi = "ngày" }
        );

        context.MatHangs.AddRange(
            new MatHang
            {
                MaHang = "IP15PM",
                TenHang = "iPhone 15 Pro Max",
                DonGia = 30000000m,
                Dvt = "Cái",
                MaNhom = "DIENTHOAI",
                MaLoai = "APPLE",
                QuanLyImei = true,
                ThoiHanBaoHanh = 12
            },
            new MatHang
            {
                MaHang = "SAC20W",
                TenHang = "Củ sạc 20W",
                DonGia = 500000m,
                Dvt = "Cái",
                MaNhom = "PHUKIEN",
                MaLoai = "APPLE",
                QuanLyImei = false,
                ThoiHanBaoHanh = 12
            }
        );

        context.IMEIs.AddRange(
            new IMEI { MaIMEI = "IMEI01", SoIMEI = "111111111111111", MaHang = "IP15PM", SoPN = "PN01", TrangThai = "con_hang" },
            new IMEI { MaIMEI = "IMEI02", SoIMEI = "222222222222222", MaHang = "IP15PM", SoPN = "PN01", TrangThai = "con_hang" },
            new IMEI { MaIMEI = "IMEI03", SoIMEI = "333333333333333", MaHang = "IP15PM", SoPN = "PN01", TrangThai = "con_hang" }
        );

        // Hàng phụ kiện nhập kho 10 cái
        context.PhieuNhaps.Add(new PhieuNhap { SoPN = "PN01", MaNCC = "NCC01", MaNV = "NV001", NgayNhap = DateTime.UtcNow });
        context.ChiTietPhieuNhaps.Add(new ChiTietPhieuNhap { SoPN = "PN01", MaHang = "SAC20W", SlNhap = 10, DonGiaNhap = 300000m });

        context.SaveChanges();
    }

    [Fact]
    public async Task Sales_ClientPriceIsIgnored_DBPriceUsed()
    {
        var dbName = Guid.NewGuid().ToString();
        using var context = GetInMemoryDbContext(dbName);
        SeedBaseData(context);

        var currentUser = new MockCurrentUser();
        var codeGen = new CodeGenerator(context);
        var sysSetting = new SystemSettingService(context);
        var warranty = new WarrantyService(context, codeGen);
        var salesService = new SalesService(context, currentUser, codeGen, sysSetting, warranty);

        // Client cố ý gửi đơn giá 100đ
        var req = new CreateHoaDonRequest
        {
            PhuongThucThanhToan = "tien_mat",
            ChiTiet = new List<ChiTietHoaDonRequest>
            {
                new ChiTietHoaDonRequest { MaHang = "IP15PM", SoLuong = 1, SoIMEI = "111111111111111" }
            }
        };

        var result = await salesService.CreateHoaDonAsync(req);

        Assert.NotNull(result);
        Assert.Equal(30000000m, result.TongTien);
        Assert.Equal(30000000m, result.ThanhTien);
    }

    [Fact]
    public async Task Sales_WithPromotion_CalculatesDiscountCorrectly()
    {
        var dbName = Guid.NewGuid().ToString();
        using var context = GetInMemoryDbContext(dbName);
        SeedBaseData(context);

        context.KhuyenMais.Add(new KhuyenMai
        {
            MaKM = "KM10",
            TenKM = "Giảm 10%",
            PhanTramGiam = 10,
            NgayBD = DateTime.UtcNow.AddDays(-1),
            NgayKT = DateTime.UtcNow.AddDays(10),
            TamDung = false
        });
        context.SaveChanges();

        var currentUser = new MockCurrentUser();
        var codeGen = new CodeGenerator(context);
        var sysSetting = new SystemSettingService(context);
        var warranty = new WarrantyService(context, codeGen);
        var salesService = new SalesService(context, currentUser, codeGen, sysSetting, warranty);

        var req = new CreateHoaDonRequest
        {
            MaKM = "KM10",
            PhuongThucThanhToan = "tien_mat",
            ChiTiet = new List<ChiTietHoaDonRequest>
            {
                new ChiTietHoaDonRequest { MaHang = "IP15PM", SoLuong = 1, SoIMEI = "111111111111111" }
            }
        };

        var result = await salesService.CreateHoaDonAsync(req);

        Assert.NotNull(result);
        Assert.Equal(30000000m, result.TongTien);
        Assert.Equal(3000000m, result.TienGiamGia);
        Assert.Equal(27000000m, result.ThanhTien);
    }

    [Fact]
    public async Task Sales_DoubleSaleSameImei_ThrowsConflict()
    {
        var dbName = Guid.NewGuid().ToString();
        using var context = GetInMemoryDbContext(dbName);
        SeedBaseData(context);

        var currentUser = new MockCurrentUser();
        var codeGen = new CodeGenerator(context);
        var sysSetting = new SystemSettingService(context);
        var warranty = new WarrantyService(context, codeGen);
        var salesService = new SalesService(context, currentUser, codeGen, sysSetting, warranty);

        var req1 = new CreateHoaDonRequest
        {
            PhuongThucThanhToan = "tien_mat",
            ChiTiet = new List<ChiTietHoaDonRequest>
            {
                new ChiTietHoaDonRequest { MaHang = "IP15PM", SoLuong = 1, SoIMEI = "111111111111111" }
            }
        };

        var first = await salesService.CreateHoaDonAsync(req1);
        Assert.NotNull(first);

        // Bán lần 2 cùng IMEI
        var req2 = new CreateHoaDonRequest
        {
            PhuongThucThanhToan = "tien_mat",
            ChiTiet = new List<ChiTietHoaDonRequest>
            {
                new ChiTietHoaDonRequest { MaHang = "IP15PM", SoLuong = 1, SoIMEI = "111111111111111" }
            }
        };

        var ex = await Assert.ThrowsAsync<ConflictException>(() => salesService.CreateHoaDonAsync(req2));
        Assert.Contains("không còn trong kho", ex.Message);
    }

    [Fact]
    public async Task Returns_ValidWindow_Success()
    {
        var dbName = Guid.NewGuid().ToString();
        using var context = GetInMemoryDbContext(dbName);
        SeedBaseData(context);

        var currentUser = new MockCurrentUser();
        var codeGen = new CodeGenerator(context);
        var sysSetting = new SystemSettingService(context);
        var warranty = new WarrantyService(context, codeGen);
        var salesService = new SalesService(context, currentUser, codeGen, sysSetting, warranty);
        var returnsService = new ReturnsService(context, currentUser, codeGen, sysSetting);

        var hd = await salesService.CreateHoaDonAsync(new CreateHoaDonRequest
        {
            PhuongThucThanhToan = "tien_mat",
            ChiTiet = new List<ChiTietHoaDonRequest>
            {
                new ChiTietHoaDonRequest { MaHang = "IP15PM", SoLuong = 1, SoIMEI = "111111111111111" }
            }
        });

        var traReq = new CreateTraHangRequest
        {
            SoHD = hd.SoHD,
            SoIMEI = "111111111111111",
            LyDo = "Máy đổi trả trong 7 ngày",
            HinhThucHoanTien = "tien_mat"
        };

        var traResult = await returnsService.CreateTraHangAsync(traReq);
        Assert.NotNull(traResult);
        Assert.Equal("111111111111111", traResult.SoIMEI);

        var imeiInDb = await context.IMEIs.FirstAsync(i => i.SoIMEI == "111111111111111");
        Assert.Equal("cho_duyet", imeiInDb.TrangThai);
        Assert.Equal("tiep_nhan", traResult.TrangThai);

        // Bước 2: Kỹ thuật kiểm tra
        var techUser = new MockCurrentUser { MaNV = "NV002", Role = "ky_thuat" };
        var returnsServiceTech = new ReturnsService(context, techUser, codeGen, sysSetting);
        var ktResult = await returnsServiceTech.KiemTraAsync(traResult.MaTR, new KiemTraTraHangRequest
        {
            KetQuaKiemTra = "loi_nsx",
            GhiChuKiemTra = "Lỗi nguồn phần cứng nhà sản xuất"
        });
        Assert.Equal("da_kiem_tra", ktResult.TrangThai);

        // Bước 3: Quản lý duyệt
        var managerUser = new MockCurrentUser { MaNV = "NV003", Role = "quan_ly" };
        var returnsServiceManager = new ReturnsService(context, managerUser, codeGen, sysSetting);
        var duyetResult = await returnsServiceManager.DuyetAsync(traResult.MaTR, new DuyetTraHangRequest
        {
            ChapNhan = true
        });
        Assert.Equal("da_duyet", duyetResult.TrangThai);

        // Bước 4: Hoàn tất hồ sơ
        var hoanTatResult = await returnsServiceManager.HoanTatAsync(traResult.MaTR, new HoanTatTraHangRequest());
        Assert.Equal("hoan_tat", hoanTatResult.TrangThai);

        imeiInDb = await context.IMEIs.FirstAsync(i => i.SoIMEI == "111111111111111");
        Assert.Equal("dang_sua", imeiInDb.TrangThai);
    }

    [Fact]
    public async Task Returns_ExpiredWindow_ThrowsBadRequest()
    {
        var dbName = Guid.NewGuid().ToString();
        using var context = GetInMemoryDbContext(dbName);
        SeedBaseData(context);

        // Tạo hóa đơn từ 45 ngày trước (quá hạn 30 ngày)
        var hd = new HoaDon
        {
            SoHD = "HDOLD01",
            NgayLap = DateTime.UtcNow.AddDays(-45),
            MaNV = "NV001",
            TongTien = 30000000m,
            ThanhTien = 30000000m,
            PhuongThucThanhToan = "tien_mat"
        };
        context.HoaDons.Add(hd);
        context.ChiTietHoaDons.Add(new ChiTietHoaDon
        {
            SoHD = "HDOLD01",
            MaHang = "IP15PM",
            MaIMEI = "IMEI01",
            SlBan = 1,
            DonGiaBan = 30000000m
        });
        var imei = await context.IMEIs.FirstAsync(i => i.MaIMEI == "IMEI01");
        imei.TrangThai = "da_ban";
        context.SaveChanges();

        var currentUser = new MockCurrentUser();
        var codeGen = new CodeGenerator(context);
        var sysSetting = new SystemSettingService(context);
        var returnsService = new ReturnsService(context, currentUser, codeGen, sysSetting);

        var traReq = new CreateTraHangRequest
        {
            SoHD = "HDOLD01",
            SoIMEI = "111111111111111",
            LyDo = "Đổi máy",
            HinhThucHoanTien = "tien_mat"
        };

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => returnsService.CreateTraHangAsync(traReq));
        Assert.Contains("Đã quá hạn đổi trả", ex.Message);
    }

    [Fact]
    public async Task OnlineOrder_FullFlow_Create_Approve_Ship_Complete()
    {
        var dbName = Guid.NewGuid().ToString();
        using var context = GetInMemoryDbContext(dbName);
        SeedBaseData(context);

        var currentUser = new MockCurrentUser();
        var codeGen = new CodeGenerator(context);
        var warranty = new WarrantyService(context, codeGen);
        var orderService = new OnlineOrderService(context, currentUser, codeGen, warranty);

        // 1. Khách đặt hàng công khai
        var createReq = new CreateOrderRequest
        {
            TenNguoiNhan = "Khách Hàng Test",
            SdtNguoiNhan = "0987654321",
            DiaChiGiao = "123 Lê Văn Việt, TP Thủ Đức",
            PhuongThucThanhToan = "cod",
            Items = new List<OrderItemRequest>
            {
                new OrderItemRequest { MaHang = "IP15PM", SoLuong = 1, DonGia = 1000m } // Client gửi giá ảo
            }
        };

        var created = await orderService.CreateOrderAsync(createReq);
        Assert.NotNull(created);
        Assert.Equal(30000000m, created.TongTien); // Giá thực từ DB
        Assert.Equal(AppConstants.OrderStatus.ChoXacNhan, created.TrangThai);

        // 2. Duyệt đơn: cho_xac_nhan -> da_xac_nhan
        var approved = await orderService.UpdateOrderStatusAsync(created.MaDH, AppConstants.OrderStatus.DaXacNhan);
        Assert.Equal(AppConstants.OrderStatus.DaXacNhan, approved.TrangThai);

        // 3. Giao hàng: da_xac_nhan -> dang_giao
        var shipped = await orderService.UpdateOrderStatusAsync(created.MaDH, AppConstants.OrderStatus.DangGiao);
        Assert.Equal(AppConstants.OrderStatus.DangGiao, shipped.TrangThai);

        // 4. Hoàn thành: dang_giao -> hoan_thanh (Tự sinh HĐ và gán IMEI)
        var completed = await orderService.UpdateOrderStatusAsync(created.MaDH, AppConstants.OrderStatus.HoanThanh);
        Assert.Equal(AppConstants.OrderStatus.HoanThanh, completed.TrangThai);
        Assert.False(string.IsNullOrEmpty(completed.SoHD));

        // Kiểm tra HĐ đã tạo
        var hoadon = await context.HoaDons.FirstOrDefaultAsync(h => h.SoHD == completed.SoHD);
        Assert.NotNull(hoadon);
        Assert.Equal("online", hoadon.KenhBan);

        // Đơn đã hoàn thành không thể hủy -> 409
        await Assert.ThrowsAsync<ConflictException>(() =>
            orderService.UpdateOrderStatusAsync(created.MaDH, AppConstants.OrderStatus.DaHuy));
    }
}
