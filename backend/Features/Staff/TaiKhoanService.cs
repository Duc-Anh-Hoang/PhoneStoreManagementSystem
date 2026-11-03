using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Staff.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Staff;

public class TaiKhoanService : ITaiKhoanService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TaiKhoanService> _logger;

    public TaiKhoanService(AppDbContext context, ILogger<TaiKhoanService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<TaiKhoanDto>> GetAllAsync()
    {
        return await _context.TaiKhoans.AsNoTracking()
            .Include(t => t.NhanVien)
            .Include(t => t.PhanQuyen)
            .OrderBy(t => t.UserName)
            .Select(t => new TaiKhoanDto
            {
                Username = t.UserName,
                MaNV = t.MaNV,
                HoTen = t.NhanVien != null ? t.NhanVien.HoTen : string.Empty,
                MaQuyen = t.MaQuyen,
                TenQuyen = t.PhanQuyen != null ? t.PhanQuyen.TenQuyen : t.MaQuyen,
                TrangThai = t.TrangThai,
                NgayTao = t.NgayTao
            })
            .ToListAsync();
    }

    public async Task<TaiKhoanDto> GetByUsernameAsync(string username)
    {
        var account = await _context.TaiKhoans.AsNoTracking()
            .Include(t => t.NhanVien)
            .Include(t => t.PhanQuyen)
            .FirstOrDefaultAsync(t => t.UserName.ToLower() == username.ToLower());

        if (account == null)
        {
            throw new NotFoundException($"Không tìm thấy tài khoản '{username}'.");
        }

        return new TaiKhoanDto
        {
            Username = account.UserName,
            MaNV = account.MaNV,
            HoTen = account.NhanVien != null ? account.NhanVien.HoTen : string.Empty,
            MaQuyen = account.MaQuyen,
            TenQuyen = account.PhanQuyen != null ? account.PhanQuyen.TenQuyen : account.MaQuyen,
            TrangThai = account.TrangThai,
            NgayTao = account.NgayTao
        };
    }

    public async Task<TaiKhoanDto> CreateAsync(CreateTaiKhoanRequest request)
    {
        string username = request.Username.Trim().ToLower();

        // 1. Kiểm tra username tồn tại
        if (await _context.TaiKhoans.AnyAsync(t => t.UserName.ToLower() == username))
        {
            throw new ConflictException($"Tên đăng nhập '{username}' đã tồn tại.");
        }

        // 2. Kiểm tra nhân viên tồn tại và chưa có tài khoản
        var staff = await _context.NhanViens.FirstOrDefaultAsync(n => n.MaNV == request.MaNV.Trim());
        if (staff == null)
        {
            throw new NotFoundException($"Nhân viên với mã '{request.MaNV}' không tồn tại.");
        }

        if (await _context.TaiKhoans.AnyAsync(t => t.MaNV == staff.MaNV))
        {
            throw new ConflictException($"Nhân viên '{staff.MaNV}' ({staff.HoTen}) đã có tài khoản hệ thống.");
        }

        // 3. Kiểm tra quyền tồn tại
        var role = await _context.PhanQuyens.FirstOrDefaultAsync(p => p.MaQuyen == request.MaQuyen.Trim().ToLower());
        if (role == null)
        {
            throw new BadRequestException($"Mã quyền '{request.MaQuyen}' không hợp lệ.");
        }

        // 4. Hash mật khẩu bằng BCrypt
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, 11);

        var newAccount = new TaiKhoan
        {
            UserName = username,
            PasswordHash = passwordHash,
            MaNV = staff.MaNV,
            MaQuyen = role.MaQuyen,
            TrangThai = AppConstants.AccountStatus.HoatDong,
            NgayTao = DateTime.UtcNow
        };

        _context.TaiKhoans.Add(newAccount);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Đã tạo tài khoản mới: '{Username}' cho nhân viên '{MaNV}' với quyền '{MaQuyen}'.",
            newAccount.UserName, staff.MaNV, role.MaQuyen);

        return new TaiKhoanDto
        {
            Username = newAccount.UserName,
            MaNV = newAccount.MaNV,
            HoTen = staff.HoTen,
            MaQuyen = newAccount.MaQuyen,
            TenQuyen = role.TenQuyen,
            TrangThai = newAccount.TrangThai,
            NgayTao = newAccount.NgayTao
        };
    }

    public async Task<TaiKhoanDto> UpdateAsync(string username, UpdateTaiKhoanRequest request, string? currentUsername)
    {
        username = username.Trim().ToLower();
        var account = await _context.TaiKhoans
            .Include(t => t.NhanVien)
            .Include(t => t.PhanQuyen)
            .FirstOrDefaultAsync(t => t.UserName.ToLower() == username);

        if (account == null)
        {
            throw new NotFoundException($"Không tìm thấy tài khoản '{username}'.");
        }

        // Kiểm tra quyền
        if (!string.IsNullOrWhiteSpace(request.MaQuyen))
        {
            string newRole = request.MaQuyen.Trim().ToLower();
            if (account.MaQuyen == AppConstants.Roles.Admin && newRole != AppConstants.Roles.Admin)
            {
                // Không cho đổi quyền của admin cuối cùng
                int otherActiveAdmins = await _context.TaiKhoans.CountAsync(t => 
                    t.MaQuyen == AppConstants.Roles.Admin && 
                    t.TrangThai == AppConstants.AccountStatus.HoatDong && 
                    t.UserName.ToLower() != username);

                if (otherActiveAdmins == 0)
                {
                    throw new BadRequestException("Không thể thay đổi quyền của tài khoản admin đang hoạt động cuối cùng trong hệ thống.");
                }
            }

            var role = await _context.PhanQuyens.FirstOrDefaultAsync(p => p.MaQuyen == newRole);
            if (role == null)
            {
                throw new BadRequestException($"Mã quyền '{request.MaQuyen}' không hợp lệ.");
            }

            account.MaQuyen = role.MaQuyen;
        }

        // Đổi mật khẩu nếu có yêu cầu
        if (!string.IsNullOrWhiteSpace(request.NewPassword))
        {
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, 11);
            _logger.LogInformation("Đã đặt lại mật khẩu cho tài khoản '{Username}'.", username);
        }

        await _context.SaveChangesAsync();

        return new TaiKhoanDto
        {
            Username = account.UserName,
            MaNV = account.MaNV,
            HoTen = account.NhanVien?.HoTen ?? string.Empty,
            MaQuyen = account.MaQuyen,
            TenQuyen = account.PhanQuyen?.TenQuyen ?? account.MaQuyen,
            TrangThai = account.TrangThai,
            NgayTao = account.NgayTao
        };
    }

    public async Task<TaiKhoanDto> UpdateStatusAsync(string username, string trangThai, string? currentUsername)
    {
        username = username.Trim().ToLower();
        trangThai = (trangThai ?? string.Empty).Trim().ToLower();

        if (trangThai != AppConstants.AccountStatus.HoatDong && trangThai != AppConstants.AccountStatus.BiKhoa)
        {
            throw new BadRequestException("Trạng thái phải là 'hoat_dong' hoặc 'bi_khoa'.");
        }

        var account = await _context.TaiKhoans
            .Include(t => t.NhanVien)
            .Include(t => t.PhanQuyen)
            .FirstOrDefaultAsync(t => t.UserName.ToLower() == username);

        if (account == null)
        {
            throw new NotFoundException($"Không tìm thấy tài khoản '{username}'.");
        }

        // 1. Không cho tự khóa chính mình
        if (string.Equals(username, currentUsername, StringComparison.OrdinalIgnoreCase) && 
            trangThai == AppConstants.AccountStatus.BiKhoa)
        {
            throw new BadRequestException("Không thể tự khóa tài khoản của chính mình.");
        }

        // 2. Không cho khóa admin cuối cùng
        if (account.MaQuyen == AppConstants.Roles.Admin && trangThai == AppConstants.AccountStatus.BiKhoa)
        {
            int otherActiveAdmins = await _context.TaiKhoans.CountAsync(t =>
                t.MaQuyen == AppConstants.Roles.Admin &&
                t.TrangThai == AppConstants.AccountStatus.HoatDong &&
                t.UserName.ToLower() != username);

            if (otherActiveAdmins == 0)
            {
                throw new BadRequestException("Không thể khóa tài khoản admin đang hoạt động cuối cùng trong hệ thống.");
            }
        }

        account.TrangThai = trangThai;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Đã cập nhật trạng thái tài khoản '{Username}' thành '{TrangThai}'.", username, trangThai);

        return new TaiKhoanDto
        {
            Username = account.UserName,
            MaNV = account.MaNV,
            HoTen = account.NhanVien?.HoTen ?? string.Empty,
            MaQuyen = account.MaQuyen,
            TenQuyen = account.PhanQuyen?.TenQuyen ?? account.MaQuyen,
            TrangThai = account.TrangThai,
            NgayTao = account.NgayTao
        };
    }

    public async Task<List<PhanQuyenDto>> GetAllRolesAsync()
    {
        return await _context.PhanQuyens.AsNoTracking()
            .OrderBy(p => p.MaQuyen)
            .Select(p => new PhanQuyenDto
            {
                MaQuyen = p.MaQuyen,
                TenQuyen = p.TenQuyen,
                MoTa = p.MoTa
            })
            .ToListAsync();
    }
}
