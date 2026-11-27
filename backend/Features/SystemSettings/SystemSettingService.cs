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
                MaThamSo = t.MaHienThi ?? t.MaThamSo,
                TenThamSo = t.TenThamSo,
                GiaTri = t.GiaTri,
                DonVi = t.DonVi
            })
            .ToListAsync();
    }

    public async Task<ThamSoDto> GetByMaAsync(string ma)
    {
        var code = ma.Trim().ToUpperInvariant();
        var entity = await _context.ThamSos
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.MaThamSo == code || t.MaHienThi == code);

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy tham số '{ma}'.");
        }

        return new ThamSoDto
        {
            MaThamSo = entity.MaHienThi ?? entity.MaThamSo,
            TenThamSo = entity.TenThamSo,
            GiaTri = entity.GiaTri,
            DonVi = entity.DonVi
        };
    }

    public async Task<ThamSoDto> UpdateAsync(string ma, UpdateThamSoRequest request)
    {
        if (_updateValidator != null) await _updateValidator.ValidateAndThrowAsync(request);

        var currentCode = ma.Trim().ToUpperInvariant();
        var entity = await _context.ThamSos
            .FirstOrDefaultAsync(t => t.MaThamSo == currentCode || t.MaHienThi == currentCode);

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy tham số '{ma}'.");
        }

        var newCode = request.MaThamSo?.Trim().ToUpperInvariant();
        var name = request.TenThamSo?.Trim();
        var unit = request.DonVi?.Trim();
        if ((newCode != null && !System.Text.RegularExpressions.Regex.IsMatch(newCode, "^[A-Z][A-Z0-9_]{0,29}$")) ||
            (name != null && name.Length is < 1 or > 100) || unit?.Length > 20 ||
            request.GiaTri < 0 || request.GiaTri > 9999999999999999.99m || decimal.Round(request.GiaTri, 2) != request.GiaTri)
            throw new BadRequestException("Mã, tên, đơn vị hoặc giá trị tham số không hợp lệ.");

        if (newCode != null)
        {
            if (await _context.ThamSos.AnyAsync(t => t.MaThamSo != entity.MaThamSo &&
                (t.MaThamSo == newCode || t.MaHienThi == newCode)))
                throw new ConflictException("Mã tham số đã tồn tại.");
            entity.MaHienThi = newCode == entity.MaThamSo ? null : newCode;
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
            MaThamSo = entity.MaHienThi ?? entity.MaThamSo,
            TenThamSo = entity.TenThamSo,
            GiaTri = entity.GiaTri,
            DonVi = entity.DonVi
        };
    }
}
