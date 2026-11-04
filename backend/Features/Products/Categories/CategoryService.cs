using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Products.Categories.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Products.Categories;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    #region NHÓM HÀNG

    public async Task<List<NhomHangDto>> GetAllNhomHangAsync()
    {
        var productCounts = await _context.MatHangs
            .GroupBy(m => m.MaNhom)
            .Select(g => new { MaNhom = g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.MaNhom, g => g.Count);

        var list = await _context.NhomHangs
            .AsNoTracking()
            .OrderBy(n => n.MaNhom)
            .ToListAsync();

        return list.Select(n => new NhomHangDto
        {
            MaNhom = n.MaNhom,
            TenNhom = n.TenNhom,
            SoLuongMatHang = productCounts.TryGetValue(n.MaNhom, out var c) ? c : 0
        }).ToList();
    }

    public async Task<NhomHangDto> GetNhomHangByMaAsync(string maNhom)
    {
        var entity = await _context.NhomHangs.FindAsync(maNhom);
        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy nhóm hàng với mã '{maNhom}'");
        }

        var count = await _context.MatHangs.CountAsync(m => m.MaNhom == maNhom);

        return new NhomHangDto
        {
            MaNhom = entity.MaNhom,
            TenNhom = entity.TenNhom,
            SoLuongMatHang = count
        };
    }

    public async Task<NhomHangDto> CreateNhomHangAsync(CreateNhomHangRequest request)
    {
        var maNhom = request.MaNhom.Trim().ToUpper();
        var tenNhom = request.TenNhom.Trim();

        // Kiểm tra trùng mã
        if (await _context.NhomHangs.AnyAsync(n => n.MaNhom == maNhom))
        {
            throw new ConflictException($"Mã nhóm hàng '{maNhom}' đã tồn tại");
        }

        // Kiểm tra trùng tên (duy nhất)
        if (await _context.NhomHangs.AnyAsync(n => n.TenNhom.ToLower() == tenNhom.ToLower()))
        {
            throw new ConflictException($"Tên nhóm hàng '{tenNhom}' đã tồn tại");
        }

        var entity = new NhomHang
        {
            MaNhom = maNhom,
            TenNhom = tenNhom
        };

        _context.NhomHangs.Add(entity);
        await _context.SaveChangesAsync();

        return new NhomHangDto
        {
            MaNhom = entity.MaNhom,
            TenNhom = entity.TenNhom,
            SoLuongMatHang = 0
        };
    }

    public async Task<NhomHangDto> UpdateNhomHangAsync(string maNhom, UpdateNhomHangRequest request)
    {
        var entity = await _context.NhomHangs.FindAsync(maNhom);
        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy nhóm hàng với mã '{maNhom}'");
        }

        var tenNhom = request.TenNhom.Trim();
        // Kiểm tra trùng tên với nhóm hàng khác (409)
        if (await _context.NhomHangs.AnyAsync(n => n.MaNhom != maNhom && n.TenNhom.ToLower() == tenNhom.ToLower()))
        {
            throw new ConflictException($"Tên nhóm hàng '{tenNhom}' đã được sử dụng bởi nhóm khác");
        }

        entity.TenNhom = tenNhom;
        await _context.SaveChangesAsync();

        var count = await _context.MatHangs.CountAsync(m => m.MaNhom == maNhom);

        return new NhomHangDto
        {
            MaNhom = entity.MaNhom,
            TenNhom = entity.TenNhom,
            SoLuongMatHang = count
        };
    }

    public async Task DeleteNhomHangAsync(string maNhom)
    {
        var entity = await _context.NhomHangs.FindAsync(maNhom);
        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy nhóm hàng với mã '{maNhom}'");
        }

        // DELETE trả 409 nếu còn mặt hàng thuộc nhóm
        if (await _context.MatHangs.AnyAsync(m => m.MaNhom == maNhom))
        {
            throw new ConflictException($"Không thể xóa nhóm hàng '{maNhom}' vì vẫn còn mặt hàng trực thuộc");
        }

        _context.NhomHangs.Remove(entity);
        await _context.SaveChangesAsync();
    }

    #endregion

    #region LOẠI HÀNG

    public async Task<List<LoaiHangDto>> GetAllLoaiHangAsync()
    {
        var productCounts = await _context.MatHangs
            .GroupBy(m => m.MaLoai)
            .Select(g => new { MaLoai = g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.MaLoai, g => g.Count);

        var list = await _context.LoaiHangs
            .AsNoTracking()
            .OrderBy(l => l.MaLoai)
            .ToListAsync();

        return list.Select(l => new LoaiHangDto
        {
            MaLoai = l.MaLoai,
            TenLoai = l.TenLoai,
            SoLuongMatHang = productCounts.TryGetValue(l.MaLoai, out var c) ? c : 0
        }).ToList();
    }

    public async Task<LoaiHangDto> GetLoaiHangByMaAsync(string maLoai)
    {
        var entity = await _context.LoaiHangs.FindAsync(maLoai);
        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy loại hàng với mã '{maLoai}'");
        }

        var count = await _context.MatHangs.CountAsync(m => m.MaLoai == maLoai);

        return new LoaiHangDto
        {
            MaLoai = entity.MaLoai,
            TenLoai = entity.TenLoai,
            SoLuongMatHang = count
        };
    }

    public async Task<LoaiHangDto> CreateLoaiHangAsync(CreateLoaiHangRequest request)
    {
        var maLoai = request.MaLoai.Trim().ToUpper();
        var tenLoai = request.TenLoai.Trim();

        // Kiểm tra trùng mã
        if (await _context.LoaiHangs.AnyAsync(l => l.MaLoai == maLoai))
        {
            throw new ConflictException($"Mã loại hàng '{maLoai}' đã tồn tại");
        }

        // Kiểm tra trùng tên
        if (await _context.LoaiHangs.AnyAsync(l => l.TenLoai.ToLower() == tenLoai.ToLower()))
        {
            throw new ConflictException($"Tên loại hàng '{tenLoai}' đã tồn tại");
        }

        var entity = new LoaiHang
        {
            MaLoai = maLoai,
            TenLoai = tenLoai
        };

        _context.LoaiHangs.Add(entity);
        await _context.SaveChangesAsync();

        return new LoaiHangDto
        {
            MaLoai = entity.MaLoai,
            TenLoai = entity.TenLoai,
            SoLuongMatHang = 0
        };
    }

    public async Task<LoaiHangDto> UpdateLoaiHangAsync(string maLoai, UpdateLoaiHangRequest request)
    {
        var entity = await _context.LoaiHangs.FindAsync(maLoai);
        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy loại hàng với mã '{maLoai}'");
        }

        var tenLoai = request.TenLoai.Trim();
        if (await _context.LoaiHangs.AnyAsync(l => l.MaLoai != maLoai && l.TenLoai.ToLower() == tenLoai.ToLower()))
        {
            throw new ConflictException($"Tên loại hàng '{tenLoai}' đã được sử dụng bởi loại khác");
        }

        entity.TenLoai = tenLoai;
        await _context.SaveChangesAsync();

        var count = await _context.MatHangs.CountAsync(m => m.MaLoai == maLoai);

        return new LoaiHangDto
        {
            MaLoai = entity.MaLoai,
            TenLoai = entity.TenLoai,
            SoLuongMatHang = count
        };
    }

    public async Task DeleteLoaiHangAsync(string maLoai)
    {
        var entity = await _context.LoaiHangs.FindAsync(maLoai);
        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy loại hàng với mã '{maLoai}'");
        }

        // DELETE trả 409 nếu còn mặt hàng thuộc loại
        if (await _context.MatHangs.AnyAsync(m => m.MaLoai == maLoai))
        {
            throw new ConflictException($"Không thể xóa loại hàng '{maLoai}' vì vẫn còn mặt hàng trực thuộc");
        }

        _context.LoaiHangs.Remove(entity);
        await _context.SaveChangesAsync();
    }

    #endregion
}
