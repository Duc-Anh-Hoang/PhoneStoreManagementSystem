using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Sales.DTOs;
using PhoneStoreAPI.Features.SystemSettings;
using PhoneStoreAPI.Features.Warranty;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Sales;

public class SalesService : ISalesService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly ICodeGenerator _codeGenerator;
    private readonly ISystemSettingService _systemSettingService;
    private readonly IWarrantyService _warrantyService;

    public SalesService(
        AppDbContext context,
        ICurrentUser currentUser,
        ICodeGenerator codeGenerator,
        ISystemSettingService systemSettingService,
        IWarrantyService warrantyService)
    {
        _context = context;
        _currentUser = currentUser;
        _codeGenerator = codeGenerator;
        _systemSettingService = systemSettingService;
        _warrantyService = warrantyService;
    }

    public async Task<HoaDonDto> CreateHoaDonAsync(CreateHoaDonRequest request)
    {
        var maNV = _currentUser.MaNV;
        if (string.IsNullOrWhiteSpace(maNV))
        {
            // Fallback nếu token không có hoặc là unit test
            maNV = await _context.NhanViens.Select(n => n.MaNV).FirstOrDefaultAsync() ?? "NV001";
        }

        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Kiểm tra khuyến mãi nếu có
            decimal phanTramGiam = 0m;
            string? validMaKM = null;
            if (!string.IsNullOrWhiteSpace(request.MaKM))
            {
                var now = DateTime.UtcNow;
                var km = await _context.KhuyenMais.FirstOrDefaultAsync(k => k.MaKM == request.MaKM);
                if (km == null || km.TamDung || now < km.NgayBD || now > km.NgayKT)
                {
                    throw new BadRequestException("Khuyến mãi không hợp lệ, đã hết hạn hoặc đang tạm dừng.");
                }
                phanTramGiam = km.PhanTramGiam;
                validMaKM = km.MaKM;
            }

            // 2. Kiểm tra khách hàng nếu có
            if (!string.IsNullOrWhiteSpace(request.MaKH))
            {
                var khExist = await _context.KhachHangs.AnyAsync(k => k.MaKH == request.MaKH);
                if (!khExist)
                {
                    throw new BadRequestException($"Khách hàng {request.MaKH} không tồn tại.");
                }
            }

            // 3. Chuẩn bị danh sách chi tiết và kiểm tra hàng hóa, IMEI
            var chiTietEntities = new List<ChiTietHoaDon>();
            var imeiUpdates = new List<string>(); // Danh sách MaIMEI cần cập nhật
            decimal tongTien = 0m;

            foreach (var item in request.ChiTiet)
            {
                var matHang = await _context.MatHangs.FirstOrDefaultAsync(m => m.MaHang == item.MaHang);
                if (matHang == null)
                {
                    throw new NotFoundException($"Mặt hàng {item.MaHang} không tồn tại.");
                }

                decimal donGia = matHang.DonGia; // Lấy từ DB, bỏ qua giá client

                if (matHang.QuanLyImei)
                {
                    if (item.SoLuong != 1)
                    {
                        throw new BadRequestException($"Mặt hàng quản lý IMEI ({matHang.TenHang}) mỗi dòng chỉ được bán số lượng 1.");
                    }

                    // Tìm IMEI theo MaIMEI hoặc SoIMEI
                    IMEI? imeiRecord = null;
                    if (!string.IsNullOrWhiteSpace(item.MaIMEI))
                    {
                        imeiRecord = await _context.IMEIs.FirstOrDefaultAsync(i => i.MaIMEI == item.MaIMEI);
                    }
                    else if (!string.IsNullOrWhiteSpace(item.SoIMEI))
                    {
                        imeiRecord = await _context.IMEIs.FirstOrDefaultAsync(i => i.SoIMEI == item.SoIMEI);
                    }

                    if (imeiRecord == null || imeiRecord.MaHang != matHang.MaHang)
                    {
                        throw new BadRequestException($"IMEI không hợp lệ hoặc không thuộc mặt hàng {matHang.TenHang}.");
                    }

                    if (imeiRecord.TrangThai != "con_hang")
                    {
                        throw new ConflictException($"IMEI {imeiRecord.SoIMEI} đã được bán hoặc không còn trong kho.");
                    }

                    // Cập nhật trạng thái IMEI sang da_ban trực tiếp trong C# Service (nguồn sự thật duy nhất)
                    imeiRecord.TrangThai = "da_ban";
                    _context.IMEIs.Update(imeiRecord);

                    chiTietEntities.Add(new ChiTietHoaDon
                    {
                        MaHang = matHang.MaHang,
                        MaIMEI = imeiRecord.MaIMEI,
                        SlBan = 1,
                        DonGiaBan = donGia
                    });

                    tongTien += donGia;
                }
                else
                {
                    // Mặt hàng không quản lý IMEI: Kiểm tra tồn kho (tổng nhập - tổng bán)
                    var tongNhap = await _context.ChiTietPhieuNhaps
                        .Where(p => p.MaHang == matHang.MaHang)
                        .SumAsync(p => (int?)p.SlNhap) ?? 0;

                    var tongBan = await _context.ChiTietHoaDons
                        .Where(h => h.MaHang == matHang.MaHang)
                        .SumAsync(h => (int?)h.SlBan) ?? 0;

                    int tonKho = tongNhap - tongBan;
                    if (tonKho < item.SoLuong)
                    {
                        throw new BadRequestException($"Mặt hàng {matHang.TenHang} không đủ số lượng tồn kho (còn {tonKho}).");
                    }

                    chiTietEntities.Add(new ChiTietHoaDon
                    {
                        MaHang = matHang.MaHang,
                        MaIMEI = null,
                        SlBan = item.SoLuong,
                        DonGiaBan = donGia
                    });

                    tongTien += donGia * item.SoLuong;
                }
            }

            // 4. Tính toán tiền giảm giá
            decimal tienGiamGia = 0m;
            if (phanTramGiam > 0)
            {
                tienGiamGia = Math.Round(tongTien * phanTramGiam / 100m, 2);
            }

            // 5. Tạo hóa đơn
            var soHD = await _codeGenerator.NextAsync("HD");
            var hoaDon = new HoaDon
            {
                SoHD = soHD,
                NgayLap = DateTime.UtcNow,
                MaKH = request.MaKH,
                MaNV = maNV,
                MaKM = validMaKM,
                KenhBan = string.IsNullOrWhiteSpace(request.KenhBan) ? "tai_quay" : request.KenhBan,
                TongTien = tongTien,
                TienGiamGia = tienGiamGia,
                PhuongThucThanhToan = request.PhuongThucThanhToan
            };

            _context.HoaDons.Add(hoaDon);
            await _context.SaveChangesAsync();

            // 6. Gán SoHD cho chi tiết và lưu
            foreach (var ct in chiTietEntities)
            {
                ct.SoHD = soHD;
                _context.ChiTietHoaDons.Add(ct);
            }
            await _context.SaveChangesAsync();

            // 7. Tạo bảo hành cho các sản phẩm có IMEI
            foreach (var ct in chiTietEntities.Where(c => !string.IsNullOrEmpty(c.MaIMEI)))
            {
                await _warrantyService.CreateForInvoiceAsync(soHD, ct.MaIMEI!, ct.MaHang);
            }

                await transaction.CommitAsync();

                // 8. Đọc lại hóa đơn để lấy giá trị ThanhTien sinh từ DB
                return await GetHoaDonChiTietAsync(soHD);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    public async Task<(List<HoaDonDto> Items, int Total)> GetHoaDonsAsync(
        DateOnly? tuNgay, DateOnly? denNgay, string? kenhBan, string? maKH, string? keyword, int page, int pageSize)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 20 : (pageSize > 100 ? 100 : pageSize);

        var query = _context.HoaDons.AsNoTracking()
            .Include(h => h.KhachHang)
            .Include(h => h.NhanVien)
            .Include(h => h.KhuyenMai)
            .Include(h => h.ChiTietHoaDons)
                .ThenInclude(c => c.MatHang)
            .Include(h => h.ChiTietHoaDons)
                .ThenInclude(c => c.IMEI)
            .AsQueryable();

        if (tuNgay.HasValue)
        {
            var fromUtc = tuNgay.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            query = query.Where(h => h.NgayLap >= fromUtc);
        }

        if (denNgay.HasValue)
        {
            var toUtc = denNgay.Value.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);
            query = query.Where(h => h.NgayLap <= toUtc);
        }

        if (!string.IsNullOrWhiteSpace(kenhBan))
        {
            query = query.Where(h => h.KenhBan == kenhBan);
        }

        if (!string.IsNullOrWhiteSpace(maKH))
        {
            query = query.Where(h => h.MaKH == maKH);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(h => h.SoHD.ToLower().Contains(kw) 
                || (h.KhachHang != null && (h.KhachHang.HoTen.ToLower().Contains(kw) || (h.KhachHang.Sdt != null && h.KhachHang.Sdt.Contains(kw)))));
        }

        int total = await query.CountAsync();

        var list = await query
            .OrderByDescending(h => h.NgayLap)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        decimal vatPercent = await _systemSettingService.GetAsync("VAT_PHANTRAM");
        if (vatPercent <= 0) vatPercent = 10m;

        var dtos = list.Select(h => MapToDto(h, vatPercent)).ToList();
        return (dtos, total);
    }

    public async Task<HoaDonDto> GetHoaDonChiTietAsync(string soHD)
    {
        var h = await _context.HoaDons.AsNoTracking()
            .Include(x => x.KhachHang)
            .Include(x => x.NhanVien)
            .Include(x => x.KhuyenMai)
            .Include(x => x.ChiTietHoaDons)
                .ThenInclude(c => c.MatHang)
            .Include(x => x.ChiTietHoaDons)
                .ThenInclude(c => c.IMEI)
            .FirstOrDefaultAsync(x => x.SoHD == soHD);

        if (h == null)
        {
            throw new NotFoundException($"Không tìm thấy hóa đơn {soHD}");
        }

        decimal vatPercent = await _systemSettingService.GetAsync("VAT_PHANTRAM");
        if (vatPercent <= 0) vatPercent = 10m;

        return MapToDto(h, vatPercent);
    }

    private static HoaDonDto MapToDto(HoaDon h, decimal vatPercent)
    {
        decimal thanhTien = h.ThanhTien > 0 ? h.ThanhTien : (h.TongTien - h.TienGiamGia);
        decimal tienVatDaGom = Math.Round(thanhTien - thanhTien / (1m + vatPercent / 100m), 2);

        return new HoaDonDto
        {
            SoHD = h.SoHD,
            NgayLap = h.NgayLap,
            MaKH = h.MaKH,
            TenKhachHang = h.KhachHang?.HoTen,
            TenKH = h.KhachHang?.HoTen ?? "Khách lẻ",
            MaNV = h.MaNV,
            TenNV = h.NhanVien?.HoTen,
            MaKM = h.MaKM,
            TenKM = h.KhuyenMai?.TenKM,
            PhanTramGiam = h.KhuyenMai?.PhanTramGiam,
            KenhBan = h.KenhBan,
            TongTien = h.TongTien,
            TienGiamGia = h.TienGiamGia,
            ThanhTien = thanhTien,
            TienVatDaGom = tienVatDaGom,
            PhuongThucThanhToan = h.PhuongThucThanhToan,
            ChiTiet = h.ChiTietHoaDons.Select(ct => new ChiTietHoaDonDto
            {
                MaCTHD = ct.MaCTHD,
                MaHang = ct.MaHang,
                TenHang = ct.MatHang?.TenHang ?? ct.MaHang,
                MaIMEI = ct.MaIMEI,
                SoIMEI = ct.IMEI?.SoIMEI,
                SlBan = ct.SlBan,
                DonGiaBan = ct.DonGiaBan,
                ThanhTienDong = ct.SlBan * ct.DonGiaBan
            }).ToList()
        };
    }
}
