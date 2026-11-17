using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PhoneStoreAPI.Common.Configs;
using PhoneStoreAPI.Features.Checkout.Services;

namespace PhoneStoreAPI.Features.Checkout.Background;

public class OrderExpirationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CheckoutOptions _options;
    private readonly ILogger<OrderExpirationBackgroundService> _logger;

    public OrderExpirationBackgroundService(
        IServiceProvider serviceProvider,
        IOptions<CheckoutOptions> options,
        ILogger<OrderExpirationBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OrderExpirationBackgroundService đã khởi động. Chu kỳ quét: {Interval} phút.", _options.ExpireJobIntervalMinutes);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var expirationService = scope.ServiceProvider.GetRequiredService<IOrderExpirationService>();
                var result = await expirationService.RunExpirationScanAsync(stoppingToken);

                if (result.ProcessedCount > 0)
                {
                    _logger.LogInformation("OrderExpirationJob: Đã quét {Processed} đơn quá hạn, Xác nhận PAID: {Paid}, Hết hạn và hoàn kho: {Expired}.",
                        result.ProcessedCount, result.ConfirmedPaidCount, result.ExpiredCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra trong vòng lặp quét đơn hết hạn.");
            }

            try
            {
                await Task.Delay(_options.ExpireJobInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        _logger.LogInformation("OrderExpirationBackgroundService đang dừng.");
    }
}
