using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;

namespace PhoneStoreAPI.Common.Services;

public class ThamSoProvider : IThamSoProvider
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;
    private const int CacheDurationSeconds = 60;

    public ThamSoProvider(AppDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<decimal> GetAsync(string maThamSo)
    {
        if (string.IsNullOrWhiteSpace(maThamSo))
        {
            throw new BadRequestException("Mã tham số không được để trống.");
        }

        var key = GetCacheKey(maThamSo);

        if (_cache.TryGetValue(key, out decimal cachedValue))
        {
            return cachedValue;
        }

        var normalizedCode = maThamSo.Trim().ToUpperInvariant();
        var entity = await _context.ThamSos
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.MaThamSo.ToUpper() == normalizedCode);

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy tham số '{maThamSo}'.");
        }

        _cache.Set(key, entity.GiaTri, TimeSpan.FromSeconds(CacheDurationSeconds));
        return entity.GiaTri;
    }

    public void InvalidateCache(string? maThamSo = null)
    {
        if (string.IsNullOrWhiteSpace(maThamSo))
        {
            _cache.Remove(GetCacheKey("VAT_PHANTRAM"));
            _cache.Remove(GetCacheKey("DIEM_MOI_VND"));
            _cache.Remove(GetCacheKey("HAN_DOITRA_NGAY"));
        }
        else
        {
            _cache.Remove(GetCacheKey(maThamSo));
        }
    }

    private static string GetCacheKey(string maThamSo)
    {
        return $"ThamSo_{maThamSo.Trim().ToUpperInvariant()}";
    }
}

