using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Promotions.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Promotions;

public class PromotionService : IPromotionService
{
    private readonly AppDbContext _context;
    private readonly IValidator<CreatePromotionRequest> _createValidator;
    private readonly IValidator<UpdatePromotionRequest> _updateValidator;

    public PromotionService(
        AppDbContext context,
        IValidator<CreatePromotionRequest> createValidator,
        IValidator<UpdatePromotionRequest> updateValidator)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public static string ComputeTrangThai(bool tamDung, DateTime ngayBD, DateTime ngayKT, DateTime now)
    {
        if (tamDung) return "tam_dung";
        if (now < ngayBD) return "chua_ap_dung";
        if (now > ngayKT) return "het_han";
        return "dang_ap_dung";
    }

    public async Task<List<PromotionDto>> GetAllPromotionsAsync()
    {
        var now = DateTime.UtcNow;
        var list = await _context.KhuyenMais
            .AsNoTracking()
            .OrderByDescending(k => k.NgayBD)
            .ToListAsync();

        return list.Select(k => new PromotionDto
        {
            MaKM = k.MaKM,
            TenKM = k.TenKM,
            PhanTramGiam = k.PhanTramGiam,
            NgayBD = k.NgayBD,
            NgayKT = k.NgayKT,
            TamDung = k.TamDung,
            TrangThai = ComputeTrangThai(k.TamDung, k.NgayBD, k.NgayKT, now)
        }).ToList();
    }

    public async Task<List<ActivePromotionDto>> GetActivePromotionsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.KhuyenMais
            .AsNoTracking()
            .Where(k => !k.TamDung && k.NgayBD <= now && k.NgayKT >= now)
            .OrderBy(k => k.NgayKT)
            .Select(k => new ActivePromotionDto
            {
                MaKM = k.MaKM,
                TenKM = k.TenKM,
                PhanTramGiam = k.PhanTramGiam,
                NgayBD = k.NgayBD,
                NgayKT = k.NgayKT
            })
            .ToListAsync();
    }

    public async Task<PromotionDto> GetPromotionByIdAsync(string id)
    {
        var km = await _context.KhuyenMais
            .AsNoTracking()
            .FirstOrDefaultAsync(k => k.MaKM.ToUpper() == id.Trim().ToUpper());

        if (km == null)
        {
            throw new NotFoundException($"Không tìm thấy khuyến mãi '{id}'.");
        }

        var now = DateTime.UtcNow;
        return new PromotionDto
        {
            MaKM = km.MaKM,
            TenKM = km.TenKM,
            PhanTramGiam = km.PhanTramGiam,
            NgayBD = km.NgayBD,
            NgayKT = km.NgayKT,
            TamDung = km.TamDung,
            TrangThai = ComputeTrangThai(km.TamDung, km.NgayBD, km.NgayKT, now)
        };
    }

    public async Task<PromotionDto> CreatePromotionAsync(CreatePromotionRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var maKM = request.MaKM.Trim().ToUpper();
        var exists = await _context.KhuyenMais.AnyAsync(k => k.MaKM.ToUpper() == maKM);
        if (exists)
        {
            throw new ConflictException($"Mã khuyến mãi '{request.MaKM}' đã tồn tại.");
        }

        var entity = new KhuyenMai
        {
            MaKM = maKM,
            TenKM = request.TenKM.Trim(),
            PhanTramGiam = request.PhanTramGiam,
            NgayBD = EnsureUtc(request.NgayBD),
            NgayKT = EnsureUtc(request.NgayKT),
            TamDung = request.TamDung
        };

        _context.KhuyenMais.Add(entity);
        await _context.SaveChangesAsync();

        var now = DateTime.UtcNow;
        return new PromotionDto
        {
            MaKM = entity.MaKM,
            TenKM = entity.TenKM,
            PhanTramGiam = entity.PhanTramGiam,
            NgayBD = entity.NgayBD,
            NgayKT = entity.NgayKT,
            TamDung = entity.TamDung,
            TrangThai = ComputeTrangThai(entity.TamDung, entity.NgayBD, entity.NgayKT, now)
        };
    }

    public async Task<PromotionDto> UpdatePromotionAsync(string id, UpdatePromotionRequest request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var entity = await _context.KhuyenMais
            .FirstOrDefaultAsync(k => k.MaKM.ToUpper() == id.Trim().ToUpper());

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy khuyến mãi '{id}'.");
        }

        entity.TenKM = request.TenKM.Trim();
        entity.PhanTramGiam = request.PhanTramGiam;
        entity.NgayBD = EnsureUtc(request.NgayBD);
        entity.NgayKT = EnsureUtc(request.NgayKT);
        entity.TamDung = request.TamDung;

        await _context.SaveChangesAsync();

        var now = DateTime.UtcNow;
        return new PromotionDto
        {
            MaKM = entity.MaKM,
            TenKM = entity.TenKM,
            PhanTramGiam = entity.PhanTramGiam,
            NgayBD = entity.NgayBD,
            NgayKT = entity.NgayKT,
            TamDung = entity.TamDung,
            TrangThai = ComputeTrangThai(entity.TamDung, entity.NgayBD, entity.NgayKT, now)
        };
    }

    public async Task<bool> UpdateStatusAsync(string id, bool tamDung)
    {
        var entity = await _context.KhuyenMais
            .FirstOrDefaultAsync(k => k.MaKM.ToUpper() == id.Trim().ToUpper());

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy khuyến mãi '{id}'.");
        }

        entity.TamDung = tamDung;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeletePromotionAsync(string id)
    {
        var entity = await _context.KhuyenMais
            .FirstOrDefaultAsync(k => k.MaKM.ToUpper() == id.Trim().ToUpper());

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy khuyến mãi '{id}'.");
        }

        var hasInvoice = await _context.HoaDons.AnyAsync(h => h.MaKM != null && h.MaKM.ToUpper() == id.Trim().ToUpper());
        if (hasInvoice)
        {
            throw new ConflictException($"Không thể xóa khuyến mãi '{id}' vì đã được gắn vào hóa đơn bán hàng.");
        }

        _context.KhuyenMais.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static DateTime EnsureUtc(DateTime dt)
    {
        if (dt.Kind == DateTimeKind.Unspecified)
        {
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }
        return dt.ToUniversalTime();
    }
}
