using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Suppliers.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Suppliers;

public class SupplierService : ISupplierService
{
    private readonly AppDbContext _context;
    private readonly IValidator<CreateSupplierRequest> _createValidator;
    private readonly IValidator<UpdateSupplierRequest> _updateValidator;

    public SupplierService(
        AppDbContext context,
        IValidator<CreateSupplierRequest> createValidator,
        IValidator<UpdateSupplierRequest> updateValidator)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<(List<SupplierDto> Items, int TotalCount)> GetAllAsync(int? page = null, int? pageSize = null)
    {
        var query = _context.NhaCungCaps.AsNoTracking().OrderBy(n => n.MaNCC);
        var totalCount = await query.CountAsync();

        if (page.HasValue && pageSize.HasValue && page.Value > 0 && pageSize.Value > 0)
        {
            var pSize = Math.Min(pageSize.Value, 100);
            var items = await query
                .Skip((page.Value - 1) * pSize)
                .Take(pSize)
                .Select(n => new SupplierDto
                {
                    MaNCC = n.MaNCC,
                    TenNCC = n.TenNCC,
                    DiaChi = n.DiaChi,
                    Sdt = n.Sdt,
                    Email = n.Email
                })
                .ToListAsync();

            return (items, totalCount);
        }

        var allItems = await query
            .Select(n => new SupplierDto
            {
                MaNCC = n.MaNCC,
                TenNCC = n.TenNCC,
                DiaChi = n.DiaChi,
                Sdt = n.Sdt,
                Email = n.Email
            })
            .ToListAsync();

        return (allItems, totalCount);
    }

    public async Task<SupplierDto> GetByIdAsync(string id)
    {
        var ncc = await _context.NhaCungCaps
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.MaNCC.ToUpper() == id.Trim().ToUpper());

        if (ncc == null)
        {
            throw new NotFoundException($"Không tìm thấy nhà cung cấp '{id}'.");
        }

        return new SupplierDto
        {
            MaNCC = ncc.MaNCC,
            TenNCC = ncc.TenNCC,
            DiaChi = ncc.DiaChi,
            Sdt = ncc.Sdt,
            Email = ncc.Email
        };
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var maNCC = request.MaNCC.Trim().ToUpper();
        var exists = await _context.NhaCungCaps.AnyAsync(n => n.MaNCC.ToUpper() == maNCC);
        if (exists)
        {
            throw new ConflictException($"Mã nhà cung cấp '{request.MaNCC}' đã tồn tại trong hệ thống.");
        }

        var entity = new NhaCungCap
        {
            MaNCC = maNCC,
            TenNCC = request.TenNCC.Trim(),
            DiaChi = request.DiaChi?.Trim(),
            Sdt = request.Sdt?.Trim(),
            Email = request.Email?.Trim()
        };

        _context.NhaCungCaps.Add(entity);
        await _context.SaveChangesAsync();

        return new SupplierDto
        {
            MaNCC = entity.MaNCC,
            TenNCC = entity.TenNCC,
            DiaChi = entity.DiaChi,
            Sdt = entity.Sdt,
            Email = entity.Email
        };
    }

    public async Task<SupplierDto> UpdateAsync(string id, UpdateSupplierRequest request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var entity = await _context.NhaCungCaps
            .FirstOrDefaultAsync(n => n.MaNCC.ToUpper() == id.Trim().ToUpper());

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy nhà cung cấp '{id}'.");
        }

        entity.TenNCC = request.TenNCC.Trim();
        entity.DiaChi = request.DiaChi?.Trim();
        entity.Sdt = request.Sdt?.Trim();
        entity.Email = request.Email?.Trim();

        await _context.SaveChangesAsync();

        return new SupplierDto
        {
            MaNCC = entity.MaNCC,
            TenNCC = entity.TenNCC,
            DiaChi = entity.DiaChi,
            Sdt = entity.Sdt,
            Email = entity.Email
        };
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _context.NhaCungCaps
            .FirstOrDefaultAsync(n => n.MaNCC.ToUpper() == id.Trim().ToUpper());

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy nhà cung cấp '{id}'.");
        }

        // Ràng buộc 409: đã có phiếu nhập
        var hasPhieuNhap = await _context.PhieuNhaps.AnyAsync(p => p.MaNCC.ToUpper() == id.Trim().ToUpper());
        if (hasPhieuNhap)
        {
            throw new ConflictException($"Không thể xóa nhà cung cấp '{id}' vì đã phát sinh phiếu nhập kho liên quan.");
        }

        _context.NhaCungCaps.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
