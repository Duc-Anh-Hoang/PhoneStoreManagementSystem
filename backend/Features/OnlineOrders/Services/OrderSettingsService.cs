using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.OnlineOrders.Services;

public class OrderSettingsService : IOrderSettingsService
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly ILogger<OrderSettingsService> _logger;
    private const string CacheKey = "order_settings_all";

    public OrderSettingsService(AppDbContext context, IMemoryCache cache, ILogger<OrderSettingsService> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Dictionary<string, string>> GetAllSettingsAsync()
    {
        if (_cache.TryGetValue(CacheKey, out Dictionary<string, string>? cached) && cached != null)
        {
            return cached;
        }

        try
        {
            var dict = await _context.OrderSettings
                .AsNoTracking()
                .ToDictionaryAsync(s => s.Key, s => s.Value);

            _cache.Set(CacheKey, dict, TimeSpan.FromSeconds(60));
            return dict;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi đọc bảng order_settings, dùng fallback");
            return GetFallbackSettings();
        }
    }

    public async Task<bool> UpdateSettingAsync(string key, string value)
    {
        var setting = await _context.OrderSettings.FirstOrDefaultAsync(s => s.Key == key);
        if (setting == null)
        {
            _context.OrderSettings.Add(new OrderSetting { Key = key, Value = value });
        }
        else
        {
            setting.Value = value;
        }

        await _context.SaveChangesAsync();
        _cache.Remove(CacheKey);
        return true;
    }

    private async Task<string> GetRawAsync(string key, string fallback)
    {
        var dict = await GetAllSettingsAsync();
        return dict.TryGetValue(key, out var val) ? val : fallback;
    }

    public async Task<bool> IsAutoApproveEnabledAsync() =>
        bool.TryParse(await GetRawAsync("auto_approve_enabled", "true"), out var b) ? b : true;

    public async Task<int> GetAutoApproveDelayMinutesAsync() =>
        int.TryParse(await GetRawAsync("auto_approve_delay_minutes", "15"), out var i) ? i : 15;

    public async Task<decimal> GetCodHoldThresholdAsync() =>
        decimal.TryParse(await GetRawAsync("cod_hold_threshold", "5000000"), out var d) ? d : 5_000_000m;

    public async Task<decimal> GetCodMaxValueAsync() =>
        decimal.TryParse(await GetRawAsync("cod_max_value", "30000000"), out var d) ? d : 30_000_000m;

    public async Task<decimal> GetCodSmallAutoBelowAsync() =>
        decimal.TryParse(await GetRawAsync("cod_small_auto_below", "0"), out var d) ? d : 0m;

    public async Task<int> GetTrustedMinCompletedAsync() =>
        int.TryParse(await GetRawAsync("trusted_min_completed", "2"), out var i) ? i : 2;

    public async Task<decimal> GetTrustMinOrderValueAsync() =>
        decimal.TryParse(await GetRawAsync("trust_min_order_value", "1000000"), out var d) ? d : 1_000_000m;

    public async Task<decimal> GetTrustValueMultiplierAsync() =>
        decimal.TryParse(await GetRawAsync("trust_value_multiplier", "2"), out var d) ? d : 2m;

    public async Task<int> GetTrustedLookbackMonthsAsync() =>
        int.TryParse(await GetRawAsync("trusted_lookback_months", "6"), out var i) ? i : 6;

    public async Task<int> GetBlacklistAfterFailedAsync() =>
        int.TryParse(await GetRawAsync("blacklist_after_failed", "2"), out var i) ? i : 2;

    public async Task<int> GetRiskMaxOrdersPerWindowAsync() =>
        int.TryParse(await GetRawAsync("risk_max_orders_per_window", "5"), out var i) ? i : 5;

    public async Task<int> GetRiskWindowMinutesAsync() =>
        int.TryParse(await GetRawAsync("risk_window_minutes", "10"), out var i) ? i : 10;

    private static Dictionary<string, string> GetFallbackSettings() => new()
    {
        ["auto_approve_enabled"] = "true",
        ["auto_approve_delay_minutes"] = "15",
        ["cod_hold_threshold"] = "5000000",
        ["cod_max_value"] = "30000000",
        ["cod_small_auto_below"] = "0",
        ["trusted_min_completed"] = "2",
        ["trust_min_order_value"] = "1000000",
        ["trust_value_multiplier"] = "2",
        ["trusted_lookback_months"] = "6",
        ["blacklist_after_failed"] = "2",
        ["risk_max_orders_per_window"] = "5",
        ["risk_window_minutes"] = "10"
    };
}
