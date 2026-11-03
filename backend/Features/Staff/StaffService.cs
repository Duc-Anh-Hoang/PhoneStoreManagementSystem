using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Staff.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Staff;

public class StaffService : IStaffService
{
    private readonly AppDbContext _context;
    private readonly ILogger<StaffService> _logger;

    public StaffService(AppDbContext context, ILogger<StaffService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<(List<NhanVienDto> items, int totalCount)> GetAllAsync(
        string? keyword,
        string? chucVu,
        string? trangThai,
        int page,
        int pageSize)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _context.NhanViens.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(n =>
                n.MaNV.ToLower().Contains(kw) ||
                n.HoTen.ToLower().Contains(kw) ||
                (n.Sdt != null && n.Sdt.Contains(kw)) ||
                (n.Email != null && n.Email.ToLower().Contains(kw)));
        }

        if (!string.IsNullOrWhiteSpace(chucVu))
        {
            query = query.Where(n => n.ChucVu == chucVu.Trim());
        }

        if (!string.IsNullOrWhiteSpace(trangThai))
        {
            query = query.Where(n => n.TrangThai == trangThai.Trim());
        }

        int totalCount = await query.CountAsync();

        var list = await query
            .OrderBy(n => n.MaNV)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NhanVienDto
            {
                MaNV = n.MaNV,
                HoTen = n.HoTen,
                ChucVu = n.ChucVu,
                Sdt = n.Sdt,
                Email = n.Email,
                NgayVaoLam = n.NgayVaoLam,
                TrangThai = n.TrangThai
            })
            .ToListAsync();

        return (list, totalCount);
    }

    public async Task<NhanVienDto> GetByIdAsync(string maNV)
    {
        var staff = await _context.NhanViens.AsNoTracking()
            .FirstOrDefaultAsync(n => n.MaNV == maNV);

        if (staff == null)
        {
            throw new NotFoundException($"Không tìm thấy nhân viên với mã '{maNV}'.");
        }

        return new NhanVienDto
        {
            MaNV = staff.MaNV,
            HoTen = staff.HoTen,
            ChucVu = staff.ChucVu,
            Sdt = staff.Sdt,
            Email = staff.Email,
            NgayVaoLam = staff.NgayVaoLam,
            TrangThai = staff.TrangThai
        };
    }

    public async Task<NhanVienDto> CreateAsync(CreateNhanVienRequest request)
    {
        // 1. Kiểm tra email trùng
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            string email = request.Email.Trim().ToLower();
            bool emailExists = await _context.NhanViens.AnyAsync(n => n.Email != null && n.Email.ToLower() == email);
            if (emailExists)
            {
                throw new ConflictException($"Email '{request.Email}' đã được sử dụng bởi nhân viên khác.");
            }
        }

        // 2. Xác định mã nhân viên
        string maNV = (request.MaNV ?? string.Empty).Trim().ToUpper();
        if (string.IsNullOrEmpty(maNV))
        {
            // Tự sinh mã NV + 3 chữ số (NV001, NV002, ...)
            var existingStaffCodes = await _context.NhanViens
                .Select(n => n.MaNV)
                .ToListAsync();

            int maxNum = 0;
            foreach (var code in existingStaffCodes)
            {
                if (code.StartsWith("NV", StringComparison.OrdinalIgnoreCase) && 
                    int.TryParse(code.Substring(2), out int num))
                {
                    if (num > maxNum) maxNum = num;
                }
            }
            maNV = $"NV{(maxNum + 1):D3}";
        }
        else
        {
            if (await _context.NhanViens.AnyAsync(n => n.MaNV == maNV))
            {
                throw new ConflictException($"Mã nhân viên '{maNV}' đã tồn tại trong hệ thống.");
            }
        }

        var newStaff = new NhanVien
        {
            MaNV = maNV,
            HoTen = request.HoTen.Trim(),
            ChucVu = request.ChucVu.Trim(),
            Sdt = request.Sdt?.Trim(),
            Email = request.Email?.Trim(),
            NgayVaoLam = DateTime.UtcNow,
            TrangThai = !string.IsNullOrWhiteSpace(request.TrangThai) 
                ? request.TrangThai.Trim() 
                : AppConstants.EmployeeStatus.DangLam
        };

        _context.NhanViens.Add(newStaff);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Đã thêm nhân viên mới: {MaNV} - {HoTen}", newStaff.MaNV, newStaff.HoTen);

        return new NhanVienDto
        {
            MaNV = newStaff.MaNV,
            HoTen = newStaff.HoTen,
            ChucVu = newStaff.ChucVu,
            Sdt = newStaff.Sdt,
            Email = newStaff.Email,
            NgayVaoLam = newStaff.NgayVaoLam,
            TrangThai = newStaff.TrangThai
        };
    }

    public async Task<NhanVienDto> UpdateAsync(string maNV, UpdateNhanVienRequest request)
    {
        var staff = await _context.NhanViens.FirstOrDefaultAsync(n => n.MaNV == maNV);
        if (staff == null)
        {
            throw new NotFoundException($"Không tìm thấy nhân viên với mã '{maNV}'.");
        }

        // Kiểm tra email trùng
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            string email = request.Email.Trim().ToLower();
            bool emailExists = await _context.NhanViens.AnyAsync(n => 
                n.Email != null && n.Email.ToLower() == email && n.MaNV != maNV);

            if (emailExists)
            {
                throw new ConflictException($"Email '{request.Email}' đã được sử dụng bởi nhân viên khác.");
            }
        }

        staff.HoTen = request.HoTen.Trim();
        staff.ChucVu = request.ChucVu.Trim();
        staff.Sdt = request.Sdt?.Trim();
        staff.Email = request.Email?.Trim();
        staff.TrangThai = request.TrangThai.Trim();

        await _context.SaveChangesAsync();

        _logger.LogInformation("Đã cập nhật nhân viên: {MaNV} - {HoTen}", staff.MaNV, staff.HoTen);

        return new NhanVienDto
        {
            MaNV = staff.MaNV,
            HoTen = staff.HoTen,
            ChucVu = staff.ChucVu,
            Sdt = staff.Sdt,
            Email = staff.Email,
            NgayVaoLam = staff.NgayVaoLam,
            TrangThai = staff.TrangThai
        };
    }

    public async Task<(bool isDeleted, string message)> DeleteAsync(string maNV)
    {
        var staff = await _context.NhanViens.FirstOrDefaultAsync(n => n.MaNV == maNV);
        if (staff == null)
        {
            throw new NotFoundException($"Không tìm thấy nhân viên với mã '{maNV}'.");
        }

        // Kiểm tra phát sinh chứng từ
        bool hasDocuments = 
            await _context.PhieuNhaps.AnyAsync(p => p.MaNV == maNV) ||
            await _context.HoaDons.AnyAsync(h => h.MaNV == maNV) ||
            await _context.TraHangs.AnyAsync(t => t.MaNV == maNV) ||
            await _context.PhieuTiepNhanBaoHanhs.AnyAsync(p => p.MaNVTiepNhan == maNV || p.MaNVKyThuat == maNV) ||
            await _context.DonHangOnlines.AnyAsync(d => d.MaNV == maNV) ||
            await _context.TaiKhoans.AnyAsync(t => t.MaNV == maNV);

        if (hasDocuments)
        {
            // Nhân viên đã có chứng từ liên quan -> chuyển trạng thái thành da_nghi
            staff.TrangThai = AppConstants.EmployeeStatus.DaNghi;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Nhân viên {MaNV} đã phát sinh chứng từ, đã chuyển trạng thái thành 'da_nghi'.", maNV);
            return (false, $"Nhân viên {maNV} đã phát sinh chứng từ hoặc tài khoản hệ thống. Đã chuyển trạng thái sang 'da_nghi' thay vì xóa.");
        }

        // Chưa phát sinh chứng từ -> xóa thật
        _context.NhanViens.Remove(staff);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Đã xóa hoàn toàn nhân viên {MaNV}.", maNV);
        return (true, $"Đã xóa nhân viên {maNV} thành công.");
    }
}
