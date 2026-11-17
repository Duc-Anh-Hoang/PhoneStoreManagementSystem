using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Features.Checkout.Services;

namespace PhoneStoreAPI.Features.Checkout.Services;

public interface IPaymentWebhookHandler
{
    Task<WebhookProcessResult> ProcessQrWebhookAsync(string providerName, string rawPayload, string? signature, CancellationToken cancellationToken = default);
    Task<StateTransitionResult> ConfirmCodOrderAsync(string orderCode, string? otp = null, CancellationToken cancellationToken = default);
}
