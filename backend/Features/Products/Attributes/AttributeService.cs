using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Products.Attributes.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Products.Attributes;

public class AttributeService : IAttributeService
{
    private readonly AppDbContext _context;

    public AttributeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ThuocTinhDto>> GetAllThuocTinhAsync()
    {
        var productCounts = await _context.ChiTietThuocTinhs
            .GroupBy(c => c.MaThuocTinh)
            .Select(g => new { MaThuocTinh = g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.MaThuocTinh, g => g.Count);

        var list = await _context.ThuocTinhs
            .AsNoTracking()
            .OrderBy(t => t.MaThuocTinh)
            .ToListAsync();

        return list.Select(t => new ThuocTinhDto
        {
            MaThuocTinh = t.MaThuocTinh,
            TenThuocTinh = t.TenThuocTinh,
            SoLuongMatHang = productCounts.TryGetValue(t.MaThuocTinh, out var c) ? c : 0
        }).ToList();
    }

    public async Task<ThuocTinhDto> GetThuocTinhByMaAsync(string maThuocTinh)
    {
        var entity = await _context.ThuocTinhs.FindAsync(maThuocTinh);
        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy thuộc tính với mã '{maThuocTinh}'");
        }

        var count = await _context.ChiTietThuocTinhs.CountAsync(c => c.MaThuocTinh == maThuocTinh);

        return new ThuocTinhDto
        {
            MaThuocTinh = entity.MaThuocTinh,
            TenThuocTinh = entity.TenThuocTinh,
            SoLuongMatHang = count
        };
    }

    public async Task<ThuocTinhDto> CreateThuocTinhAsync(CreateThuocTinhRequest request)
    {
        var maTT = request.MaThuocTinh.Trim().ToUpper();
        var tenTT = request.TenThuocTinh.Trim();

        // Kiểm tra trùng mã
        if (await _context.ThuocTinhs.AnyAsync(t => t.MaThuocTinh == maTT))
        {
            throw new ConflictException($"Mã thuộc tính '{maTT}' đã tồn tại");
        }

        // Kiểm tra trùng tên (duy nhất)
        if (await _context.ThuocTinhs.AnyAsync(t => t.TenThuocTinh.ToLower() == tenTT.ToLower()))
        {
            throw new ConflictException($"Tên thuộc tính '{tenTT}' đã tồn tại");
        }

        var entity = new ThuocTinh
        {
            MaThuocTinh = maTT,
            TenThuocTinh = tenTT
        };

        _context.ThuocTinhs.Add(entity);
        await _context.SaveChangesAsync();

        return new ThuocTinhDto
        {
            MaThuocTinh = entity.MaThuocTinh,
            TenThuocTinh = entity.TenThuocTinh,
            SoLuongMatHang = 0
        };
    }

    public async Task<ThuocTinhDto> UpdateThuocTinhAsync(string maThuocTinh, UpdateThuocTinhRequest request)
    {
        var entity = await _context.ThuocTinhs.FindAsync(maThuocTinh);
        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy thuộc tính với mã '{maThuocTinh}'");
        }

        var tenTT = request.TenThuocTinh.Trim();
        if (await _context.ThuocTinhs.AnyAsync(t => t.MaThuocTinh != maThuocTinh && t.TenThuocTinh.ToLower() == tenTT.ToLower()))
        {
            throw new ConflictException($"Tên thuộc tính '{tenTT}' đã được sử dụng bởi thuộc tính khác");
        }

        entity.TenThuocTinh = tenTT;
        await _context.SaveChangesAsync();

        var count = await _context.ChiTietThuocTinhs.CountAsync(c => c.MaThuocTinh == maThuocTinh);

        return new ThuocTinhDto
        {
            MaThuocTinh = entity.MaThuocTinh,
            TenThuocTinh = entity.TenThuocTinh,
            SoLuongMatHang = count
        };
    }

    public async Task DeleteThuocTinhAsync(string maThuocTinh)
    {
        var entity = await _context.ThuocTinhs.FindAsync(maThuocTinh);
        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy thuộc tính với mã '{maThuocTinh}'");
        }

        // DELETE trả 409 nếu thuộc tính đang được gán cho mặt hàng
        if (await _context.ChiTietThuocTinhs.AnyAsync(c => c.MaThuocTinh == maThuocTinh))
        {
            throw new ConflictException($"Không thể xóa thuộc tính '{maThuocTinh}' vì đang được gán cho mặt hàng");
        }

        _context.ThuocTinhs.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
