namespace PhoneStoreAPI.Common.Services;

public interface IThamSoProvider
{
    Task<decimal> GetAsync(string maThamSo);
    void InvalidateCache(string? maThamSo = null);
}

