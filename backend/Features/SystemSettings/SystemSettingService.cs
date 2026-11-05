using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.SystemSettings.DTOs;

namespace PhoneStoreAPI.Features.SystemSettings;

public class SystemSettingService : ISystemSettingService
{
    private readonly AppDbContext _context;
    private readonly IThamSoProvider? _thamSoProvider;
    private readonly IValidator<UpdateThamSoRequest>? _updateValidator;

    public SystemSettingService(
        AppDbContext context, 
        IThamSoProvider? thamSoProvider = null,
        IValidator<UpdateThamSoRequest>? updateValidator = null)
    {
        _context = context;
        _thamSoProvider = thamSoProvider;
        _updateValidator = updateValidator;
    }

    public async Task<decimal> GetAsync(string maThamSo)
    {
        if (_thamSoProvider != null)
        {
            return await _thamSoProvider.GetAsync(maThamSo);
        }
        var ts = await _context.ThamSos.AsNoTracking().FirstOrDefaultAsync(t => t.MaThamSo == maThamSo);
        return ts?.GiaTri ?? 0m;
    }

    public async Task<List<ThamSoDto>> GetAllAsync()
    {
        return await _context.ThamSos
            .AsNoTracking()
            .OrderBy(t => t.MaThamSo)
            .Select(t => new ThamSoDto
            {
                MaThamSo = t.MaThamSo,
                TenThamSo = t.TenThamSo,
                GiaTri = t.GiaTri,
                DonVi = t.DonVi
            })
            .ToListAsync();
    }

    public async Task<ThamSoDto> GetByMaAsync(string ma)
    {
        var entity = await _context.ThamSos
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.MaThamSo.ToUpper() == ma.Trim().ToUpper());

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy tham số '{ma}'.");
        }

        return new ThamSoDto
        {
            MaThamSo = entity.MaThamSo,
            TenThamSo = entity.TenThamSo,
            GiaTri = entity.GiaTri,
            DonVi = entity.DonVi
        };
    }

    public async Task<ThamSoDto> UpdateAsync(string ma, UpdateThamSoRequest request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var entity = await _context.ThamSos
            .FirstOrDefaultAsync(t => t.MaThamSo.ToUpper() == ma.Trim().ToUpper());

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy tham số '{ma}'.");
        }

        entity.GiaTri = request.GiaTri;
        if (!string.IsNullOrWhiteSpace(request.TenThamSo))
        {
            entity.TenThamSo = request.TenThamSo.Trim();
        }
        if (request.DonVi != null)
        {
            entity.DonVi = request.DonVi.Trim();
        }

        await _context.SaveChangesAsync();

        // Xóa cache ngay khi PUT
        _thamSoProvider?.InvalidateCache(entity.MaThamSo);

        return new ThamSoDto
        {
            MaThamSo = entity.MaThamSo,
            TenThamSo = entity.TenThamSo,
            GiaTri = entity.GiaTri,
            DonVi = entity.DonVi
        };
    }
}
