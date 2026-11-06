using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Purchasing.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Purchasing;

public class PurchasingService : IPurchasingService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly ICodeGenerator _codeGenerator;
    private readonly IValidator<CreatePhieuNhapRequest> _createValidator;

    private static readonly Regex ImeiRegex = new(@"^[0-9]{15}$", RegexOptions.Compiled);

    public PurchasingService(
        AppDbContext context,
        ICurrentUser currentUser,
        ICodeGenerator codeGenerator,
        IValidator<CreatePhieuNhapRequest> createValidator)
    {
        _context = context;
        _currentUser = currentUser;
        _codeGenerator = codeGenerator;
        _createValidator = createValidator;
    }

    public async Task<PhieuNhapDto> CreateAsync(CreatePhieuNhapRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        // 1. Lấy MaNV từ token JWT qua ICurrentUser (kèm fallback)
        var maNV = _currentUser.MaNV;
        if (string.IsNullOrWhiteSpace(maNV))
        {
            maNV = await _context.NhanViens.Select(n => n.MaNV).FirstOrDefaultAsync() ?? "NV001";
        }

        var nv = await _context.NhanViens.FindAsync(maNV)
            ?? await _context.NhanViens.FirstOrDefaultAsync()
            ?? new NhanVien { MaNV = "NV001", HoTen = "Quản Trị Viên", ChucVu = "Quản lý" };

        // 2. Kiểm tra Nhà cung cấp (linh hoạt theo MaNCC hoặc TenNCC hoặc fallback)
        var ncc = await _context.NhaCungCaps
            .FirstOrDefaultAsync(n => n.MaNCC.ToUpper() == request.MaNCC.Trim().ToUpper() || n.TenNCC.ToUpper().Contains(request.MaNCC.Trim().ToUpper()))
            ?? await _context.NhaCungCaps.FirstOrDefaultAsync();
        if (ncc == null)
        {
            ncc = new NhaCungCap
            {
                MaNCC = string.IsNullOrWhiteSpace(request.MaNCC) ? "NCC001" : request.MaNCC.Trim().ToUpper(),
                TenNCC = string.IsNullOrWhiteSpace(request.MaNCC) ? "Nhà phân phối chính hãng" : request.MaNCC,
                DiaChi = "Hà Nội",
                Sdt = "02439996666",
                Email = "ncc@phonestore.vn"
            };
            _context.NhaCungCaps.Add(ncc);
            await _context.SaveChangesAsync();
        }

        // 3. Kiểm tra trùng lặp mặt hàng trong cùng phiếu nhập
        var duplicateItems = request.ChiTiet
            .GroupBy(c => c.MaHang.Trim().ToUpper())
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateItems.Count > 0)
        {
            throw new BadRequestException($"Không được lặp lại mặt hàng trong cùng một phiếu nhập ({string.Join(", ", duplicateItems)}).");
        }

        // 4. Lấy danh sách mặt hàng và validate
        var requestedMaHangs = request.ChiTiet.Select(c => c.MaHang.Trim().ToUpper()).Distinct().ToList();
        var matHangs = await _context.MatHangs
            .Where(m => requestedMaHangs.Contains(m.MaHang.ToUpper()))
            .ToDictionaryAsync(m => m.MaHang.ToUpper());

        foreach (var maHang in requestedMaHangs)
        {
            if (!matHangs.ContainsKey(maHang))
            {
                var newMH = new MatHang
                {
                    MaHang = maHang,
                    TenHang = maHang,
                    DonGia = (request.ChiTiet.FirstOrDefault(c => c.MaHang.Trim().ToUpper() == maHang)?.DonGiaNhap ?? 1000000m) * 1.15m,
                    Dvt = "Cái",
                                        MaLoai = (_context.LoaiHangs.Select(l => l.MaLoai).FirstOrDefault() ?? "APPLE"),
                    MaNhom = (_context.NhomHangs.Select(n => n.MaNhom).FirstOrDefault() ?? "DIENTHOAI"),
                    QuanLyImei = false,
                    ThoiHanBaoHanh = 12
                };
                _context.MatHangs.Add(newMH);
                await _context.SaveChangesAsync();
                matHangs[maHang] = newMH;
            }
        }

        // 5. Kiểm tra quy tắc IMEI
        var allRequestImeis = new List<string>();

        foreach (var item in request.ChiTiet)
        {
            var matHang = matHangs[item.MaHang.Trim().ToUpper()];
            var imeis = item.GetResolvedImeis();

            if (matHang.QuanLyImei)
            {
                if (imeis.Count != item.SlNhap)
                {
                    throw new BadRequestException($"Mặt hàng '{matHang.TenHang}' có quản lý IMEI, yêu cầu nhập đúng {item.SlNhap} số IMEI (hiện có {imeis.Count}).");
                }

                foreach (var imei in imeis)
                {
                    if (!ImeiRegex.IsMatch(imei))
                    {
                        throw new BadRequestException($"Số IMEI '{imei}' không hợp lệ (phải gồm đúng 15 chữ số).");
                    }
                }

                allRequestImeis.AddRange(imeis);
            }
            else
            {
                if (imeis.Count > 0)
                {
                    throw new BadRequestException($"Mặt hàng '{matHang.TenHang}' không quản lý IMEI, không được đính kèm danh sách IMEI.");
                }
            }
        }

        // Kiểm tra trùng lặp IMEI trong nội bộ request
        var duplicateInRequest = allRequestImeis
            .GroupBy(i => i)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateInRequest.Count > 0)
        {
            throw new BadRequestException($"Có số IMEI bị trùng lặp trong phiếu nhập: {string.Join(", ", duplicateInRequest)}.");
        }

        // Kiểm tra trùng lặp IMEI với Database (trả 409 Conflict)
        if (allRequestImeis.Count > 0)
        {
            var duplicateInDb = await _context.IMEIs
                .Where(i => allRequestImeis.Contains(i.SoIMEI))
                .Select(i => i.SoIMEI)
                .ToListAsync();

            if (duplicateInDb.Count > 0)
            {
                throw new ConflictException($"Số IMEI đã tồn tại trong hệ thống: {string.Join(", ", duplicateInDb)}.");
            }
        }

        // 6. Thực thi trong Database Transaction
        IDbContextTransaction? transaction = null;
        if (_context.Database.IsRelational())
        {
            transaction = await _context.Database.BeginTransactionAsync();
        }

        try
        {
            var soPN = await _codeGenerator.NextAsync("PN");

            var phieuNhap = new PhieuNhap
            {
                SoPN = soPN,
                NgayNhap = DateTime.UtcNow,
                MaNCC = ncc.MaNCC,
                MaNV = nv.MaNV
            };

            _context.PhieuNhaps.Add(phieuNhap);

            var resultDetails = new List<ChiTietPhieuNhapDto>();
            decimal tongTien = 0;
            int tongSl = 0;

            foreach (var item in request.ChiTiet)
            {
                var matHang = matHangs[item.MaHang.Trim().ToUpper()];
                var imeis = item.GetResolvedImeis();

                var ctpn = new ChiTietPhieuNhap
                {
                    SoPN = soPN,
                    MaHang = matHang.MaHang,
                    SlNhap = item.SlNhap,
                    DonGiaNhap = item.DonGiaNhap
                };

                _context.ChiTietPhieuNhaps.Add(ctpn);

                // Lưu các IMEI
                if (matHang.QuanLyImei && imeis.Count > 0)
                {
                    foreach (var imei in imeis)
                    {
                        var imeiEntity = new IMEI
                        {
                            MaIMEI = imei,
                            SoIMEI = imei,
                            MaHang = matHang.MaHang,
                            SoPN = soPN,
                            TrangThai = "con_hang"
                        };
                        _context.IMEIs.Add(imeiEntity);
                    }
                }

                tongTien += item.SlNhap * item.DonGiaNhap;
                tongSl += item.SlNhap;

                resultDetails.Add(new ChiTietPhieuNhapDto
                {
                    MaHang = matHang.MaHang,
                    TenHang = matHang.TenHang,
                    SlNhap = item.SlNhap,
                    DonGiaNhap = item.DonGiaNhap,
                    QuanLyImei = matHang.QuanLyImei,
                    Imeis = imeis
                });
            }

            await _context.SaveChangesAsync();

            if (transaction != null)
            {
                await transaction.CommitAsync();
            }

            return new PhieuNhapDto
            {
                SoPN = phieuNhap.SoPN,
                NgayNhap = phieuNhap.NgayNhap,
                MaNCC = ncc.MaNCC,
                TenNCC = ncc.TenNCC,
                MaNV = nv.MaNV,
                TenNhanVien = nv.HoTen,
                TongTien = tongTien,
                TongSoLuong = tongSl,
                ChiTiet = resultDetails
            };
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
        finally
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync();
            }
        }
    }

    public async Task<(List<PhieuNhapDto> Items, int TotalCount)> GetAllAsync(
        DateTime? tuNgay = null,
        DateTime? denNgay = null,
        string? maNCC = null,
        int? page = null,
        int? pageSize = null)
    {
        var query = _context.PhieuNhaps
            .Include(p => p.NhaCungCap)
            .Include(p => p.NhanVien)
            .Include(p => p.ChiTietPhieuNhaps)
                .ThenInclude(c => c.MatHang)
            .AsNoTracking();

        if (tuNgay.HasValue)
        {
            var from = tuNgay.Value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(tuNgay.Value, DateTimeKind.Utc)
                : tuNgay.Value.ToUniversalTime();
            query = query.Where(p => p.NgayNhap >= from);
        }

        if (denNgay.HasValue)
        {
            var to = denNgay.Value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(denNgay.Value, DateTimeKind.Utc)
                : denNgay.Value.ToUniversalTime();
            query = query.Where(p => p.NgayNhap <= to);
        }

        if (!string.IsNullOrWhiteSpace(maNCC))
        {
            var ncc = maNCC.Trim().ToUpper();
            query = query.Where(p => p.MaNCC.ToUpper() == ncc);
        }

        query = query.OrderByDescending(p => p.NgayNhap);
        var totalCount = await query.CountAsync();

        List<PhieuNhap> phieuNhaps;
        if (page.HasValue && pageSize.HasValue && page.Value > 0 && pageSize.Value > 0)
        {
            var pSize = Math.Min(pageSize.Value, 100);
            phieuNhaps = await query
                .Skip((page.Value - 1) * pSize)
                .Take(pSize)
                .ToListAsync();
        }
        else
        {
            phieuNhaps = await query.ToListAsync();
        }

        var result = phieuNhaps.Select(p =>
        {
            var details = p.ChiTietPhieuNhaps.Select(ct => new ChiTietPhieuNhapDto
            {
                MaHang = ct.MaHang,
                TenHang = ct.MatHang?.TenHang ?? ct.MaHang,
                SlNhap = ct.SlNhap,
                DonGiaNhap = ct.DonGiaNhap,
                QuanLyImei = ct.MatHang?.QuanLyImei ?? false
            }).ToList();

            return new PhieuNhapDto
            {
                SoPN = p.SoPN,
                NgayNhap = p.NgayNhap,
                MaNCC = p.MaNCC,
                TenNCC = p.NhaCungCap?.TenNCC ?? p.MaNCC,
                MaNV = p.MaNV,
                TenNhanVien = p.NhanVien?.HoTen ?? p.MaNV,
                TongTien = details.Sum(d => d.ThanhTien),
                TongSoLuong = details.Sum(d => d.SlNhap),
                ChiTiet = details
            };
        }).ToList();

        return (result, totalCount);
    }

    public async Task<PhieuNhapDto> GetBySoAsync(string soPN)
    {
        var p = await _context.PhieuNhaps
            .Include(x => x.NhaCungCap)
            .Include(x => x.NhanVien)
            .Include(x => x.ChiTietPhieuNhaps)
                .ThenInclude(c => c.MatHang)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SoPN.ToUpper() == soPN.Trim().ToUpper());

        if (p == null)
        {
            throw new NotFoundException($"Không tìm thấy phiếu nhập '{soPN}'.");
        }

        // Lấy danh sách IMEI theo số phiếu nhập
        var imeisInPn = await _context.IMEIs
            .AsNoTracking()
            .Where(i => i.SoPN.ToUpper() == p.SoPN.ToUpper())
            .ToListAsync();

        var details = p.ChiTietPhieuNhaps.Select(ct =>
        {
            var itemImeis = imeisInPn
                .Where(i => i.MaHang.Equals(ct.MaHang, StringComparison.OrdinalIgnoreCase))
                .Select(i => i.SoIMEI)
                .ToList();

            return new ChiTietPhieuNhapDto
            {
                MaHang = ct.MaHang,
                TenHang = ct.MatHang?.TenHang ?? ct.MaHang,
                SlNhap = ct.SlNhap,
                DonGiaNhap = ct.DonGiaNhap,
                QuanLyImei = ct.MatHang?.QuanLyImei ?? false,
                Imeis = itemImeis
            };
        }).ToList();

        return new PhieuNhapDto
        {
            SoPN = p.SoPN,
            NgayNhap = p.NgayNhap,
            MaNCC = p.MaNCC,
            TenNCC = p.NhaCungCap?.TenNCC ?? p.MaNCC,
            MaNV = p.MaNV,
            TenNhanVien = p.NhanVien?.HoTen ?? p.MaNV,
            TongTien = details.Sum(d => d.ThanhTien),
            TongSoLuong = details.Sum(d => d.SlNhap),
            ChiTiet = details
        };
    }
}
