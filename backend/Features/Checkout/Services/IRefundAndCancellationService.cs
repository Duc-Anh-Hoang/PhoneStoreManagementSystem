using PhoneStoreAPI.Features.Checkout.PaymentProviders;

namespace PhoneStoreAPI.Features.Checkout.Services;

public record CancelOrderResult(bool Success, string Message, string? FinalStatus);

public interface IRefundAndCancellationService
{
    Task<CancelOrderResult> CancelOrderAsync(string orderCode, string reason, CancellationToken cancellationToken = default);
    Task<RefundResult> ProcessRefundAsync(long refundRequestId, CancellationToken cancellationToken = default);
}
