using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Checkout.PaymentProviders;

public record PaymentCreationResult(string PaymentUrl, string QrContent, string ProviderTxnId);
public record TransactionQueryResult(bool IsPaid, long Amount, string ProviderTxnId, string Status);
public record RefundResult(bool Success, string? ProviderRefundId, string? ErrorMessage);

public interface IPaymentProvider
{
    string ProviderName { get; }
    Task<PaymentCreationResult> CreatePaymentAsync(Order order, CancellationToken cancellationToken = default);
    bool VerifyWebhookSignature(string rawPayload, string signature, string secret);
    Task<TransactionQueryResult> QueryTransactionAsync(string orderCode, string? providerTxnId = null, CancellationToken cancellationToken = default);
    Task<RefundResult> RefundAsync(string providerTxnId, long amount, string reason, string idempotencyRefundId, CancellationToken cancellationToken = default);
}
