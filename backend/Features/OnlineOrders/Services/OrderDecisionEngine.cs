using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Customers.Helpers;

namespace PhoneStoreAPI.Features.OnlineOrders.Services;

public class OrderDecisionContext
{
    public string Phone { get; set; } = string.Empty;
    public string AddressHash { get; set; } = string.Empty;
    public string? ClientIp { get; set; }
    public decimal OrderAmount { get; set; }
    public string PaymentMethod { get; set; } = "cod"; // cod | chuyen_khoan | the
    public bool IsOnlinePaid { get; set; } = false;
}

public class OrderDecisionResult
{
    public string InitialStatus { get; set; } = AppConstants.OrderStatus.ChoXacNhan;
    public bool RiskFlag { get; set; } = false;
    public List<string> RiskReasons { get; set; } = new();
    public DateTime? AutoApproveAt { get; set; }
    public bool IsTrustedCustomer { get; set; } = false;
}

public interface IOrderDecisionEngine
{
    Task<bool> IsTrustedCustomerAsync(string phone, string addressHash, decimal orderAmount);
    Task<List<string>> EvaluateRiskAsync(string phone, string addressHash, string? clientIp, decimal orderAmount, string paymentMethod);
    Task<OrderDecisionResult> DecideInitialStatusAsync(OrderDecisionContext context);
}

public class OrderDecisionEngine : IOrderDecisionEngine
{
    private readonly AppDbContext _context;
    private readonly IOrderSettingsService _settings;
    private readonly ILogger<OrderDecisionEngine> _logger;

    public OrderDecisionEngine(
        AppDbContext context,
        IOrderSettingsService settings,
        ILogger<OrderDecisionEngine> logger)
    {
        _context = context;
        _settings = settings;
        _logger = logger;
    }

    /// <summary>
    /// Kiểm tra khách tin cậy theo mục 3.2 (phải thỏa mãn TẤT CẢ 5 điều kiện)
    /// </summary>
    public async Task<bool> IsTrustedCustomerAsync(string phone, string addressHash, decimal orderAmount)
    {
        var normPhone = PhoneHelper.NormalizeVietnamesePhone(phone) ?? phone.Trim();
        var customer = await _context.KhachHangs.AsNoTracking().FirstOrDefaultAsync(k => k.Sdt == normPhone);
        if (customer == null) return false;

        // 1. Không bị blacklist
        if (customer.IsBlacklisted) return false;

        // 2. Điểm tin cậy từ đơn giá trị cao >= trusted_min_completed (mặc định 2)
        var minCompleted = await _settings.GetTrustedMinCompletedAsync();
        if (customer.QualifiedOrders < minCompleted) return false;

        // 3. Không boom trong 6 tháng (failed_orders == 0) hoặc tỷ lệ giao >= 90%
        var lookbackMonths = await _settings.GetTrustedLookbackMonthsAsync();
        var lookbackDate = DateTime.UtcNow.AddMonths(-lookbackMonths);
        var recentFailed = await _context.DonHangOnlines
            .AsNoTracking()
            .CountAsync(d => d.SdtNguoiNhan == normPhone && (d.TrangThai == "boom" || d.TrangThai == "hoan_hang") && d.NgayDat >= lookbackDate);

        if (recentFailed > 0)
        {
            var totalRecent = await _context.DonHangOnlines
                .AsNoTracking()
                .CountAsync(d => d.SdtNguoiNhan == normPhone && d.NgayDat >= lookbackDate);
            if (totalRecent == 0 || ((double)(totalRecent - recentFailed) / totalRecent) < 0.90)
            {
                return false;
            }
        }

        // 4. Địa chỉ giao khớp với một địa chỉ đã giao thành công trước đó (so bằng address_hash)
        if (!string.IsNullOrWhiteSpace(addressHash))
        {
            var addressMatch = await _context.CustomerAddresses
                .AsNoTracking()
                .AnyAsync(a => a.Phone == normPhone && a.AddressHash == addressHash);
            if (!addressMatch)
            {
                // Khác địa chỉ đã giao -> Coi như khách mới
                return false;
            }
        }
        else
        {
            return false;
        }

        // 5. Hạn mức theo lịch sử: giá trị đơn hiện tại <= max_completed_value * multiplier và < cod_hold_threshold
        var multiplier = await _settings.GetTrustValueMultiplierAsync();
        var codHoldThreshold = await _settings.GetCodHoldThresholdAsync();
        var maxAllowed = customer.MaxCompletedValue * multiplier;

        if (orderAmount > maxAllowed || orderAmount >= codHoldThreshold)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Đánh giá rủi ro theo mục 3.4
    /// </summary>
    public async Task<List<string>> EvaluateRiskAsync(string phone, string addressHash, string? clientIp, decimal orderAmount, string paymentMethod)
    {
        var reasons = new List<string>();
        var normPhone = PhoneHelper.NormalizeVietnamesePhone(phone) ?? phone.Trim();

        // 1. Kiểm tra blacklist
        var customer = await _context.KhachHangs.AsNoTracking().FirstOrDefaultAsync(k => k.Sdt == normPhone);
        if (customer != null && customer.IsBlacklisted)
        {
            reasons.Add("Số điện thoại nằm trong danh sách đen (Blacklist)");
        }

        // 2. COD vượt cod_max_value (30tr)
        var codMaxValue = await _settings.GetCodMaxValueAsync();
        if (paymentMethod == "cod" && orderAmount > codMaxValue)
        {
            reasons.Add($"Đơn COD vượt hạn mức tối đa cho phép ({codMaxValue:N0}đ), yêu cầu cọc hoặc thanh toán trước");
        }

        // 3. Spam đặt hàng: Cùng SĐT hoặc cùng địa chỉ đặt >= risk_max_orders trong risk_window
        var windowMinutes = await _settings.GetRiskWindowMinutesAsync();
        var maxOrders = await _settings.GetRiskMaxOrdersPerWindowAsync();
        var windowStart = DateTime.UtcNow.AddMinutes(-windowMinutes);

        var phoneOrderCount = await _context.DonHangOnlines
            .AsNoTracking()
            .CountAsync(d => d.SdtNguoiNhan == normPhone && d.NgayDat >= windowStart);

        if (phoneOrderCount >= maxOrders)
        {
            reasons.Add($"SĐT đã đặt {phoneOrderCount} đơn trong vòng {windowMinutes} phút (vượt ngưỡng {maxOrders} đơn)");
        }

        if (!string.IsNullOrWhiteSpace(addressHash))
        {
            var addressOrderCount = await _context.DonHangOnlines
                .AsNoTracking()
                .CountAsync(d => d.AddressHash == addressHash && d.NgayDat >= windowStart);

            if (addressOrderCount >= maxOrders)
            {
                reasons.Add($"Địa chỉ giao đã có {addressOrderCount} đơn trong vòng {windowMinutes} phút");
            }
        }

        // 4. Kiểm tra IP nếu có
        if (!string.IsNullOrWhiteSpace(clientIp) && clientIp != "127.0.0.1" && clientIp != "::1")
        {
            var ipOrderCount = await _context.DonHangOnlines
                .AsNoTracking()
                .CountAsync(d => d.ClientIp == clientIp && d.NgayDat >= windowStart);

            if (ipOrderCount >= maxOrders * 2)
            {
                reasons.Add($"Địa chỉ IP ({clientIp}) gửi quá nhiều đơn liên tục ({ipOrderCount} đơn)");
            }
        }

        return reasons;
    }

    /// <summary>
    /// Quyết định trạng thái ban đầu của đơn hàng theo Bảng 3.3
    /// </summary>
    public async Task<OrderDecisionResult> DecideInitialStatusAsync(OrderDecisionContext context)
    {
        var result = new OrderDecisionResult();
        var normPhone = PhoneHelper.NormalizeVietnamesePhone(context.Phone) ?? context.Phone.Trim();

        // Đánh giá các rủi ro
        var riskReasons = await EvaluateRiskAsync(normPhone, context.AddressHash, context.ClientIp, context.OrderAmount, context.PaymentMethod);
        if (riskReasons.Count > 0)
        {
            result.RiskFlag = true;
            result.RiskReasons = riskReasons;
            result.InitialStatus = "can_kiem_tra";
            return result;
        }

        // Dòng 3: Thanh toán online thành công -> DA_XAC_NHAN ngay
        if (context.IsOnlinePaid)
        {
            result.InitialStatus = "da_xac_nhan";
            return result;
        }

        // Đơn online chưa thanh toán -> CHO_THANH_TOAN
        if (context.PaymentMethod == "chuyen_khoan" || context.PaymentMethod == "the")
        {
            result.InitialStatus = "cho_thanh_toan";
            return result;
        }

        // Bắt đầu xét đơn COD:
        var isAutoApproveEnabled = await _settings.IsAutoApproveEnabledAsync();
        var codMaxValue = await _settings.GetCodMaxValueAsync();
        var codHoldThreshold = await _settings.GetCodHoldThresholdAsync();
        var codSmallAutoBelow = await _settings.GetCodSmallAutoBelowAsync();
        var delayMinutes = await _settings.GetAutoApproveDelayMinutesAsync();

        // Dòng 4: COD > cod_max_value (30tr) -> CAN_KIEM_TRA
        if (context.OrderAmount > codMaxValue)
        {
            result.RiskFlag = true;
            result.RiskReasons.Add($"Đơn COD vượt {codMaxValue:N0}đ");
            result.InitialStatus = "can_kiem_tra";
            return result;
        }

        // Nếu tắt auto approve khẩn cấp -> Mọi đơn COD rơi về CHO_XAC_NHAN
        if (!isAutoApproveEnabled)
        {
            result.InitialStatus = "cho_xac_nhan";
            return result;
        }

        // Dòng 5: COD < cod_small_auto_below (nếu > 0) -> CHO_TU_DUYET
        if (codSmallAutoBelow > 0 && context.OrderAmount < codSmallAutoBelow)
        {
            result.InitialStatus = "cho_tu_duyet";
            result.AutoApproveAt = DateTime.UtcNow.AddMinutes(delayMinutes);
            return result;
        }

        // Dòng 6: COD >= cod_hold_threshold (5tr) -> CHO_XAC_NHAN (nhân viên gọi)
        if (context.OrderAmount >= codHoldThreshold)
        {
            result.InitialStatus = "cho_xac_nhan";
            return result;
        }

        // Dòng 7: COD < 5tr, kiểm tra khách tin cậy
        var isTrusted = await IsTrustedCustomerAsync(normPhone, context.AddressHash, context.OrderAmount);
        result.IsTrustedCustomer = isTrusted;

        if (isTrusted)
        {
            result.InitialStatus = "cho_tu_duyet";
            result.AutoApproveAt = DateTime.UtcNow.AddMinutes(delayMinutes);
            return result;
        }

        // Dòng 8: COD còn lại (khách mới...) -> CHO_XAC_NHAN (nhân viên gọi)
        result.InitialStatus = "cho_xac_nhan";
        return result;
    }
}
