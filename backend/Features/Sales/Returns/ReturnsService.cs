using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Sales.Returns.DTOs;
using PhoneStoreAPI.Features.SystemSettings;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Sales.Returns;

public class ReturnsService : IReturnsService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly ICodeGenerator _codeGenerator;
    private readonly ISystemSettingService _systemSettingService;

    public ReturnsService(
        AppDbContext context,
        ICurrentUser currentUser,
        ICodeGenerator codeGenerator,
        ISystemSettingService systemSettingService)
    {
        _context = context;
        _currentUser = currentUser;
        _codeGenerator = codeGenerator;
        _systemSettingService = systemSettingService;
    }

    public async Task<TraHangDto> CreateTraHangAsync(CreateTraHangRequest request)
    {
        var maNV = _currentUser.MaNV;
        if (string.IsNullOrWhiteSpace(maNV))
        {
            maNV = await _context.NhanViens.Select(n => n.MaNV).FirstOrDefaultAsync() ?? "NV001";
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Xác định IMEI record trước
            IMEI? imeiRecord = null;
            if (!string.IsNullOrWhiteSpace(request.MaIMEI))
            {
                imeiRecord = await _context.IMEIs.FirstOrDefaultAsync(i => i.MaIMEI == request.MaIMEI);
            }
            else if (!string.IsNullOrWhiteSpace(request.SoIMEI))
            {
                imeiRecord = await _context.IMEIs.FirstOrDefaultAsync(i => i.SoIMEI == request.SoIMEI);
            }

            if (imeiRecord == null)
            {
                throw new NotFoundException("Không tìm thấy thông tin IMEI yêu cầu trả.");
            }

            // 2. Tìm hoặc xác minh số hóa đơn chứa IMEI này
            string soHD = (request.SoHD ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(soHD))
            {
                // Tự động truy vết hóa đơn bán của IMEI này nếu client không truyền
                var ctGoc = await _context.ChiTietHoaDons
                    .OrderByDescending(c => c.MaCTHD)
                    .FirstOrDefaultAsync(ct => ct.MaIMEI == imeiRecord.MaIMEI);
                if (ctGoc == null)
                {
                    throw new BadRequestException($"IMEI {imeiRecord.SoIMEI} chưa từng được xuất bán trên hệ thống.");
                }
                soHD = ctGoc.SoHD;
            }

            var hoaDon = await _context.HoaDons.FirstOrDefaultAsync(h => h.SoHD == soHD);
            if (hoaDon == null)
            {
                throw new NotFoundException($"Không tìm thấy hóa đơn {soHD}");
            }

            // 3. Kiểm tra hạn đổi trả (HAN_DOITRA_NGAY)
            decimal hanDoiTraNgay = await _systemSettingService.GetAsync("HAN_DOITRA_NGAY");
            if (hanDoiTraNgay <= 0) hanDoiTraNgay = 30m;

            var now = DateTime.UtcNow;
            var daysPassed = (now - hoaDon.NgayLap).TotalDays;
            if (daysPassed > (double)hanDoiTraNgay)
            {
                throw new BadRequestException($"Đã quá hạn đổi trả ({hanDoiTraNgay} ngày kể từ ngày lập hóa đơn {soHD}).");
            }

            // 4. Kiểm tra IMEI có thuộc hóa đơn đó không
            var thuocHoaDon = await _context.ChiTietHoaDons
                .AnyAsync(ct => ct.SoHD == soHD && ct.MaIMEI == imeiRecord.MaIMEI);
            if (!thuocHoaDon)
            {
                throw new BadRequestException($"IMEI {imeiRecord.SoIMEI} không thuộc hóa đơn {soHD}.");
            }

            // 5. Kiểm tra IMEI đã từng trả chưa
            var daTra = await _context.TraHangs.AnyAsync(t => t.MaIMEI == imeiRecord.MaIMEI);
            if (daTra || imeiRecord.TrangThai == "tra_hang")
            {
                throw new ConflictException($"IMEI {imeiRecord.SoIMEI} đã được trả hàng trước đó.");
            }

            // 6. Đổi trạng thái IMEI sang tra_hang trong cùng transaction
            imeiRecord.TrangThai = "tra_hang";
            _context.IMEIs.Update(imeiRecord);

            // 7. Tạo bản ghi TraHang
            var maTR = await _codeGenerator.NextAsync("TR");
            var traHang = new TraHang
            {
                MaTR = maTR,
                SoHD = soHD,
                MaIMEI = imeiRecord.MaIMEI,
                NgayTR = now,
                LyDo = request.LyDo.Trim(),
                HinhThucHoanTien = request.HinhThucHoanTien,
                MaNV = maNV
            };

            _context.TraHangs.Add(traHang);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            var matHang = await _context.MatHangs.AsNoTracking().FirstOrDefaultAsync(m => m.MaHang == imeiRecord.MaHang);
            var nhanVien = await _context.NhanViens.AsNoTracking().FirstOrDefaultAsync(n => n.MaNV == maNV);

            return new TraHangDto
            {
                MaTR = traHang.MaTR,
                SoHD = traHang.SoHD,
                MaIMEI = traHang.MaIMEI,
                SoIMEI = imeiRecord.SoIMEI,
                TenHang = matHang?.TenHang,
                NgayTR = traHang.NgayTR,
                LyDo = traHang.LyDo,
                HinhThucHoanTien = traHang.HinhThucHoanTien,
                MaNV = traHang.MaNV,
                TenNV = nhanVien?.HoTen
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<(List<TraHangDto> Items, int Total)> GetTraHangsAsync(DateOnly? tuNgay, DateOnly? denNgay, int page, int pageSize)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 20 : (pageSize > 100 ? 100 : pageSize);

        var query = _context.TraHangs.AsNoTracking()
            .AsQueryable();

        if (tuNgay.HasValue)
        {
            var fromUtc = tuNgay.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            query = query.Where(t => t.NgayTR >= fromUtc);
        }

        if (denNgay.HasValue)
        {
            var toUtc = denNgay.Value.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);
            query = query.Where(t => t.NgayTR <= toUtc);
        }

        int total = await query.CountAsync();

        var list = await query
            .OrderByDescending(t => t.NgayTR)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var imeiIds = list.Select(l => l.MaIMEI).Distinct().ToList();
        var imeis = await _context.IMEIs.AsNoTracking()
            .Include(i => i.MatHang)
            .Where(i => imeiIds.Contains(i.MaIMEI))
            .ToDictionaryAsync(i => i.MaIMEI);

        var nvIds = list.Select(l => l.MaNV).Distinct().ToList();
        var nvs = await _context.NhanViens.AsNoTracking()
            .Where(n => nvIds.Contains(n.MaNV))
            .ToDictionaryAsync(n => n.MaNV);

        var dtos = list.Select(t =>
        {
            imeis.TryGetValue(t.MaIMEI, out var imei);
            nvs.TryGetValue(t.MaNV, out var nv);
            return new TraHangDto
            {
                MaTR = t.MaTR,
                SoHD = t.SoHD,
                MaIMEI = t.MaIMEI,
                SoIMEI = imei?.SoIMEI,
                TenHang = imei?.MatHang?.TenHang,
                NgayTR = t.NgayTR,
                LyDo = t.LyDo,
                HinhThucHoanTien = t.HinhThucHoanTien,
                MaNV = t.MaNV,
                TenNV = nv?.HoTen
            };
        }).ToList();

        return (dtos, total);
    }
}
