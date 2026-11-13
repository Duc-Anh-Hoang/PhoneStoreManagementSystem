using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.OnlineOrders.Services;

namespace PhoneStoreAPI.Features.OnlineOrders.Background;

public class AutoApproveBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AutoApproveBackgroundService> _logger;

    public AutoApproveBackgroundService(IServiceProvider serviceProvider, ILogger<AutoApproveBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AutoApproveBackgroundService đã khởi chạy, kiểm tra định kỳ mỗi 30 giây.");

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await ProcessDueOrdersAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra trong vòng lặp AutoApproveBackgroundService");
            }
        }
    }

    private async Task ProcessDueOrdersAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var settings = scope.ServiceProvider.GetRequiredService<IOrderSettingsService>();
        var decisionEngine = scope.ServiceProvider.GetRequiredService<IOrderDecisionEngine>();
        var orderService = scope.ServiceProvider.GetRequiredService<IOnlineOrderService>();
        var shippingService = scope.ServiceProvider.GetService<IShippingService>();

        if (!await settings.IsAutoApproveEnabledAsync())
        {
            return;
        }

        var now = DateTime.UtcNow;
        var dueOrders = await context.DonHangOnlines
            .Where(d => d.TrangThai == "cho_tu_duyet" && d.AutoApproveAt <= now)
            .OrderBy(d => d.AutoApproveAt)
            .Take(50)
            .ToListAsync(stoppingToken);

        if (dueOrders.Count == 0) return;

        _logger.LogInformation("Tìm thấy {Count} đơn hàng CHO_TU_DUYET đến hạn tự duyệt.", dueOrders.Count);

        foreach (var ord in dueOrders)
        {
            try
            {
                // Kiểm tra lại bắt buộc theo mục 3.5:
                // 1. Đơn vẫn ở CHO_TU_DUYET
                var freshOrder = await context.DonHangOnlines
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.MaDH == ord.MaDH, stoppingToken);

                if (freshOrder == null || freshOrder.TrangThai != "cho_tu_duyet")
                {
                    continue;
                }

                // 2. Kiểm tra lại rủi ro mới
                var risks = await decisionEngine.EvaluateRiskAsync(ord.SdtNguoiNhan, ord.AddressHash ?? "", ord.ClientIp, ord.TongTien, ord.PhuongThucThanhToan);
                if (risks.Count > 0)
                {
                    _logger.LogWarning("Đơn #{MaDH} không đạt kiểm tra lại rủi ro, chuyển CAN_KIEM_TRA: {Risks}", ord.MaDH, string.Join("; ", risks));
                    await orderService.UpdateOrderStatusAsync(ord.MaDH, "can_kiem_tra");
                    continue;
                }

                // 3. Kiểm tra lại khách tin cậy (trừ khi có bật cod_small_auto_below)
                var smallBelow = await settings.GetCodSmallAutoBelowAsync();
                bool isSmall = smallBelow > 0 && ord.TongTien < smallBelow;

                if (!isSmall)
                {
                    var isTrusted = await decisionEngine.IsTrustedCustomerAsync(ord.SdtNguoiNhan, ord.AddressHash ?? "", ord.TongTien);
                    if (!isTrusted)
                    {
                        _logger.LogInformation("Đơn #{MaDH} không còn đủ điều kiện khách tin cậy, chuyển CHO_XAC_NHAN", ord.MaDH);
                        await orderService.UpdateOrderStatusAsync(ord.MaDH, "cho_xac_nhan");
                        continue;
                    }
                }

                // Đạt hết -> Chuyển DA_XAC_NHAN & đẩy tạo vận đơn GHN
                _logger.LogInformation("Đơn #{MaDH} đã vượt qua toàn bộ kiểm tra lại, TỰ ĐỘNG DUYỆT sang DA_XAC_NHAN.", ord.MaDH);
                await orderService.UpdateOrderStatusAsync(ord.MaDH, "da_xac_nhan");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tự duyệt đơn #{MaDH}", ord.MaDH);
            }
        }
    }
}
