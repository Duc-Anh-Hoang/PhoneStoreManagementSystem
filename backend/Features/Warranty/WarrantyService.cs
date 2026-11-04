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
    private readonly ICurrentUser _currentUser;

    public WarrantyService(AppDbContext context, ICodeGenerator codeGenerator, ICurrentUser currentUser)
    {
        _context = context;
        _codeGenerator = codeGenerator;
        _currentUser = currentUser;
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

    public async Task<(List<TiepNhanBaoHanhDto> Items, int TotalCount)> GetTiepNhanListAsync(
        string? trangThai,
        DateTime? tuNgay,
        DateTime? denNgay,
        string? maNVKyThuat,
        string? keyword,
        int page,
        int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 100) pageSize = 100;

        var query = _context.PhieuTiepNhanBaoHanhs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(trangThai))
        {
            query = query.Where(x => x.TrangThai == trangThai.Trim().ToLower());
        }

        if (!string.IsNullOrWhiteSpace(maNVKyThuat))
        {
            query = query.Where(x => x.MaNVKyThuat == maNVKyThuat.Trim());
        }

        if (tuNgay.HasValue)
        {
            query = query.Where(x => x.NgayNhan >= tuNgay.Value);
        }

        if (denNgay.HasValue)
        {
            query = query.Where(x => x.NgayNhan <= denNgay.Value);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string kw = keyword.Trim().ToLower();
            query = query.Where(x => x.MaTN.ToLower().Contains(kw) ||
                                     x.MaBH.ToLower().Contains(kw) ||
                                     x.TinhTrangLoi.ToLower().Contains(kw));
        }

        int totalCount = await query.CountAsync();

        var tickets = await query
            .OrderByDescending(x => x.NgayNhan)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var maBhList = tickets.Select(t => t.MaBH).Distinct().ToList();
        var staffIds = tickets.Select(t => t.MaNVTiepNhan)
            .Concat(tickets.Where(t => t.MaNVKyThuat != null).Select(t => t.MaNVKyThuat!))
            .Distinct()
            .ToList();

        var staffMap = await _context.NhanViens
            .AsNoTracking()
            .Where(nv => staffIds.Contains(nv.MaNV))
            .ToDictionaryAsync(nv => nv.MaNV, nv => nv.HoTen);

        var bhList = await _context.BaoHanhs
            .AsNoTracking()
            .Where(b => maBhList.Contains(b.MaBH))
            .ToListAsync();

        var imeiCodes = bhList.Select(b => b.MaIMEI).Distinct().ToList();
        var imeiMap = await _context.IMEIs
            .AsNoTracking()
            .Where(i => imeiCodes.Contains(i.MaIMEI))
            .ToDictionaryAsync(i => i.MaIMEI, i => i.SoIMEI);

        var bhToImeiMap = bhList.ToDictionary(
            b => b.MaBH,
            b => imeiMap.TryGetValue(b.MaIMEI, out var sImei) ? sImei : b.MaIMEI
        );

        var dtos = tickets.Select(t => new TiepNhanBaoHanhDto
        {
            MaTN = t.MaTN,
            MaBH = t.MaBH,
            SoIMEI = bhToImeiMap.TryGetValue(t.MaBH, out var imei) ? imei : string.Empty,
            NgayNhan = t.NgayNhan,
            TinhTrangLoi = t.TinhTrangLoi,
            MaNVTiepNhan = t.MaNVTiepNhan,
            TenNVTiepNhan = staffMap.TryGetValue(t.MaNVTiepNhan, out var tenTN) ? tenTN : t.MaNVTiepNhan,
            MaNVKyThuat = t.MaNVKyThuat,
            TenNVKyThuat = t.MaNVKyThuat != null && staffMap.TryGetValue(t.MaNVKyThuat, out var tenKT) ? tenKT : null,
            NgayHenTra = t.NgayHenTra,
            NgayTra = t.NgayTra,
            KetQua = t.KetQua,
            ChiPhi = t.ChiPhi,
            TrangThai = t.TrangThai
        }).ToList();

        return (dtos, totalCount);
    }

    public async Task<TiepNhanBaoHanhDto> GetTiepNhanByIdAsync(string maTN)
    {
        if (string.IsNullOrWhiteSpace(maTN))
        {
            throw new BadRequestException("Vui lòng cung cấp mã phiếu tiếp nhận bảo hành.");
        }

        var t = await _context.PhieuTiepNhanBaoHanhs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MaTN == maTN.Trim());

        if (t == null)
        {
            throw new NotFoundException("Không tìm thấy phiếu tiếp nhận bảo hành.");
        }

        var baoHanh = await _context.BaoHanhs
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.MaBH == t.MaBH);

        string soImei = string.Empty;
        if (baoHanh != null)
        {
            var imeiEntity = await _context.IMEIs
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.MaIMEI == baoHanh.MaIMEI);
            soImei = imeiEntity?.SoIMEI ?? baoHanh.MaIMEI;
        }

        var nvTiepNhan = await _context.NhanViens.FindAsync(t.MaNVTiepNhan);
        var nvKyThuat = t.MaNVKyThuat != null ? await _context.NhanViens.FindAsync(t.MaNVKyThuat) : null;

        return new TiepNhanBaoHanhDto
        {
            MaTN = t.MaTN,
            MaBH = t.MaBH,
            SoIMEI = soImei,
            NgayNhan = t.NgayNhan,
            TinhTrangLoi = t.TinhTrangLoi,
            MaNVTiepNhan = t.MaNVTiepNhan,
            TenNVTiepNhan = nvTiepNhan?.HoTen ?? t.MaNVTiepNhan,
            MaNVKyThuat = t.MaNVKyThuat,
            TenNVKyThuat = nvKyThuat?.HoTen,
            NgayHenTra = t.NgayHenTra,
            NgayTra = t.NgayTra,
            KetQua = t.KetQua,
            ChiPhi = t.ChiPhi,
            TrangThai = t.TrangThai
        };
    }

    public async Task<TiepNhanBaoHanhDto> CreateTiepNhanAsync(CreateTiepNhanRequest request)
    {
        string cleanImei = (request.SoIMEI ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(cleanImei))
        {
            throw new BadRequestException("Vui lòng cung cấp số IMEI cần tiếp nhận.");
        }

        var imeiRecord = await _context.IMEIs
            .AsNoTracking()
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

        var now = DateTime.UtcNow;
        var ngayKT = baoHanh.NgayKT ?? baoHanh.NgayBD.AddMonths(baoHanh.ThoiHanThang);
        if (ngayKT <= now)
        {
            throw new BadRequestException("Sản phẩm đã hết hạn bảo hành.");
        }

        if (!string.IsNullOrWhiteSpace(request.MaNVKyThuat))
        {
            await ValidateTechnicianAsync(request.MaNVKyThuat.Trim());
        }

        string? maNVTiepNhan = _currentUser.MaNV;
        if (string.IsNullOrWhiteSpace(maNVTiepNhan))
        {
            throw new UnauthorizedException("Không xác định được nhân viên lập phiếu tiếp nhận.");
        }

        string maTN = await _codeGenerator.NextAsync("TN");

        var ticket = new PhieuTiepNhanBaoHanh
        {
            MaTN = maTN,
            MaBH = baoHanh.MaBH,
            NgayNhan = now,
            TinhTrangLoi = request.TinhTrangLoi.Trim(),
            MaNVTiepNhan = maNVTiepNhan,
            MaNVKyThuat = string.IsNullOrWhiteSpace(request.MaNVKyThuat) ? null : request.MaNVKyThuat.Trim(),
            NgayHenTra = request.NgayHenTra.HasValue ? DateTime.SpecifyKind(request.NgayHenTra.Value, DateTimeKind.Utc) : null,
            ChiPhi = request.ChiPhi,
            TrangThai = "tiep_nhan"
        };

        _context.PhieuTiepNhanBaoHanhs.Add(ticket);
        await _context.SaveChangesAsync();

        return await GetTiepNhanByIdAsync(maTN);
    }

    public async Task<TiepNhanBaoHanhDto> UpdateTiepNhanAsync(string maTN, UpdateTiepNhanRequest request)
    {
        if (string.IsNullOrWhiteSpace(maTN))
        {
            throw new BadRequestException("Mã phiếu tiếp nhận không hợp lệ.");
        }

        var ticket = await _context.PhieuTiepNhanBaoHanhs.FirstOrDefaultAsync(x => x.MaTN == maTN.Trim());
        if (ticket == null)
        {
            throw new NotFoundException("Không tìm thấy phiếu tiếp nhận bảo hành.");
        }

        if (request.MaNVKyThuat != null)
        {
            if (!string.IsNullOrWhiteSpace(request.MaNVKyThuat))
            {
                await ValidateTechnicianAsync(request.MaNVKyThuat.Trim());
                ticket.MaNVKyThuat = request.MaNVKyThuat.Trim();
            }
            else
            {
                ticket.MaNVKyThuat = null;
            }
        }

        if (request.ChiPhi.HasValue)
        {
            if (request.ChiPhi.Value < 0)
            {
                throw new BadRequestException("Chi phí sửa chữa không được âm.");
            }
            ticket.ChiPhi = request.ChiPhi.Value;
        }

        if (request.KetQua != null)
        {
            ticket.KetQua = request.KetQua.Trim();
        }

        if (request.NgayHenTra.HasValue)
        {
            ticket.NgayHenTra = DateTime.SpecifyKind(request.NgayHenTra.Value, DateTimeKind.Utc);
        }

        await _context.SaveChangesAsync();
        return await GetTiepNhanByIdAsync(ticket.MaTN);
    }

    public async Task<TiepNhanBaoHanhDto> UpdateTrangThaiAsync(string maTN, UpdateTrangThaiRequest request)
    {
        if (string.IsNullOrWhiteSpace(maTN))
        {
            throw new BadRequestException("Mã phiếu tiếp nhận không hợp lệ.");
        }

        var ticket = await _context.PhieuTiepNhanBaoHanhs.FirstOrDefaultAsync(x => x.MaTN == maTN.Trim());
        if (ticket == null)
        {
            throw new NotFoundException("Không tìm thấy phiếu tiếp nhận bảo hành.");
        }

        string from = (ticket.TrangThai ?? string.Empty).Trim().ToLower();
        string to = (request.TrangThai ?? string.Empty).Trim().ToLower();

        if (from == to)
        {
            return await GetTiepNhanByIdAsync(ticket.MaTN);
        }

        // Kiểm tra State Machine nghiêm ngặt theo Plan 5:
        // tiep_nhan -> dang_sua
        // dang_sua <-> cho_linh_kien
        // dang_sua hoặc cho_linh_kien -> hoan_thanh
        // hoan_thanh -> da_tra
        bool isValid = (from == "tiep_nhan" && to == "dang_sua") ||
                       (from == "dang_sua" && (to == "cho_linh_kien" || to == "hoan_thanh")) ||
                       (from == "cho_linh_kien" && (to == "dang_sua" || to == "hoan_thanh")) ||
                       (from == "hoan_thanh" && to == "da_tra");

        if (!isValid)
        {
            throw new ConflictException("Chuyển trạng thái không hợp lệ.");
        }

        ticket.TrangThai = to;
        if (to == "da_tra")
        {
            // Npgsql bắt buộc DateTime phải là UTC khi ghi vào cột timestamptz
            ticket.NgayTra = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return await GetTiepNhanByIdAsync(ticket.MaTN);
    }

    private async Task ValidateTechnicianAsync(string maNV)
    {
        var staff = await _context.NhanViens
            .AsNoTracking()
            .FirstOrDefaultAsync(nv => nv.MaNV == maNV);

        if (staff == null)
        {
            throw new BadRequestException("Nhân viên kỹ thuật không tồn tại.");
        }

        if (staff.ChucVu != "Kỹ thuật & Bảo hành")
        {
            throw new BadRequestException("Nhân viên được phân công phải có chức vụ Kỹ thuật & Bảo hành.");
        }
    }
}
