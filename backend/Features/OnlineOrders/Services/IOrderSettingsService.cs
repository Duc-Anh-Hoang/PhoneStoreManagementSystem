namespace PhoneStoreAPI.Features.OnlineOrders.Services;

public interface IOrderSettingsService
{
    Task<bool> IsAutoApproveEnabledAsync();
    Task<int> GetAutoApproveDelayMinutesAsync();
    Task<decimal> GetCodHoldThresholdAsync();
    Task<decimal> GetCodMaxValueAsync();
    Task<decimal> GetCodSmallAutoBelowAsync();
    Task<int> GetTrustedMinCompletedAsync();
    Task<decimal> GetTrustMinOrderValueAsync();
    Task<decimal> GetTrustValueMultiplierAsync();
    Task<int> GetTrustedLookbackMonthsAsync();
    Task<int> GetBlacklistAfterFailedAsync();
    Task<int> GetRiskMaxOrdersPerWindowAsync();
    Task<int> GetRiskWindowMinutesAsync();

    Task<Dictionary<string, string>> GetAllSettingsAsync();
    Task<bool> UpdateSettingAsync(string key, string value);
}
