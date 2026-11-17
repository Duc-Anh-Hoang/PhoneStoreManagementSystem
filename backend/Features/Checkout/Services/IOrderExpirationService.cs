namespace PhoneStoreAPI.Features.Checkout.Services;

public record ExpirationJobResult(int ProcessedCount, int ConfirmedPaidCount, int ExpiredCount);

public interface IOrderExpirationService
{
    Task<ExpirationJobResult> RunExpirationScanAsync(CancellationToken cancellationToken = default);
}
