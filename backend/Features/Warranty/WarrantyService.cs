using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Warranty.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Warranty;

public class WarrantyService : IWarrantyService
{
    private readonly AppDbContext _context;
    private readonly ICodeGenerator _codeGenerator;

    public WarrantyService(AppDbContext context, ICodeGenerator codeGenerator)
    {
        _context = context;
        _codeGenerator = codeGenerator;
    }

    public async Task CreateForInvoiceAsync(string soHD, string maIMEI, string maHang)
    {
        if (string.IsNullOrWhiteSpace(soHD) || string.IsNullOrWhiteSpace(maIMEI) || string.IsNullOrWhiteSpace(maHang))
        {
            return;
        }

        var matHang = await _context.MatHangs.FindAsync(maHang);
        if (matHang == null || matHang.ThoiHanBaoHanh <= 0)
        {
            // Bỏ qua tạo phiếu nếu thời hạn bảo hành = 0 theo quy định cửa hàng
            return;
        }

        string maBH = await _codeGenerator.NextAsync("BH");

        // Npgsql bắt buộc DateTime phải là UTC khi ghi vào cột timestamptz
        var ngayBD = DateTime.UtcNow;
        var ngayKT = ngayBD.AddMonths(matHang.ThoiHanBaoHanh);

        var baoHanh = new BaoHanh
        {
            MaBH = maBH,
            SoHD = soHD,
            MaIMEI = maIMEI,
            NgayBD = ngayBD,
            ThoiHanThang = matHang.ThoiHanBaoHanh,
            NgayKT = ngayKT,
            DieuKien = "Bảo hành chính hãng theo tiêu chuẩn nhà sản xuất"
        };

        _context.BaoHanhs.Add(baoHanh);
        await _context.SaveChangesAsync();
    }

    public async Task<WarrantyLookupDto> GetByImeiAsync(string imei)
    {
        if (string.IsNullOrWhiteSpace(imei))
        {
            throw new BadRequestException("Vui lòng cung cấp số IMEI hợp lệ.");
        }

        string cleanImei = imei.Trim();

        var imeiRecord = await _context.IMEIs
            .AsNoTracking()
            .Include(i => i.MatHang)
            .FirstOrDefaultAsync(i => i.SoIMEI == cleanImei || i.MaIMEI == cleanImei);

        var maImei = imeiRecord?.MaIMEI ?? cleanImei;
        var baoHanh = await _context.BaoHanhs
            .AsNoTracking()
            .Where(b => b.MaIMEI == maImei || b.MaIMEI == cleanImei)
            .OrderByDescending(b => b.NgayBD)
            .FirstOrDefaultAsync();

        if (baoHanh == null)
        {
            throw new NotFoundException("Không tìm thấy thông tin bảo hành cho IMEI này.");
        }

        string? tenHang = imeiRecord?.MatHang?.TenHang;
        if (string.IsNullOrEmpty(tenHang))
        {
            var hdItem = await _context.ChiTietHoaDons
                .AsNoTracking()
                .Include(ct => ct.MatHang)
                .FirstOrDefaultAsync(ct => ct.SoHD == baoHanh.SoHD);
            tenHang = hdItem?.MatHang?.TenHang ?? "Thiết bị điện tử";
        }

        var now = DateTime.UtcNow;
        var ngayKT = baoHanh.NgayKT ?? baoHanh.NgayBD.AddMonths(baoHanh.ThoiHanThang);
        var conHan = ngayKT > now;
        var soNgayConLai = conHan ? (int)Math.Ceiling((ngayKT - now).TotalDays) : 0;
        var trangThai = conHan ? "con_han" : "het_han";

        return new WarrantyLookupDto
        {
            TenHang = tenHang,
            SoIMEI = imeiRecord?.SoIMEI ?? cleanImei,
            NgayBD = baoHanh.NgayBD,
            NgayKT = ngayKT,
            TrangThai = trangThai,
            SoNgayConLai = soNgayConLai
        };
    }

    public async Task<WarrantyStatusDto> GetStatusByImeiAsync(string imei)
    {
        var lookup = await GetByImeiAsync(imei);

        return new WarrantyStatusDto
        {
            SoIMEI = lookup.SoIMEI,
            TrangThai = lookup.TrangThai,
            SoNgayConLai = lookup.SoNgayConLai,
            ConHan = lookup.TrangThai == "con_han"
        };
    }
}
