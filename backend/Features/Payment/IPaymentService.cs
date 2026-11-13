using PhoneStoreAPI.Features.Payment.DTOs;

namespace PhoneStoreAPI.Features.Payment;

public interface IPaymentService
{
    Task<VietQrResponse> GenerateVietQrForOrderAsync(string maDH);
    Task<bool> ConfirmOrderPaymentAsync(string maDH, ConfirmPaymentRequest request);
    Task<bool> ProcessWebhookAsync(PaymentWebhookPayload payload);
    Task<PaymentStatusResponse> GetPaymentStatusAsync(string maDH);
}
