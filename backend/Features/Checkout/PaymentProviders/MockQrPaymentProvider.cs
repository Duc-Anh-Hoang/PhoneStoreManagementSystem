using System.Security.Cryptography;
using System.Text;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Checkout.PaymentProviders;

public class MockQrPaymentProvider : IPaymentProvider
{
    public const string Name = "MOCK_QR";
    public string ProviderName => Name;

    private readonly string _secretKey;
    private readonly Dictionary<string, TransactionQueryResult> _mockTransactions = new();

    public MockQrPaymentProvider(string secretKey = "mock_webhook_secret_key_2026")
    {
        _secretKey = secretKey;
    }

    public void RegisterMockTransaction(string orderCode, bool isPaid, long amount, string providerTxnId, string status = "SUCCESS")
    {
        _mockTransactions[orderCode] = new TransactionQueryResult(isPaid, amount, providerTxnId, status);
    }

    public Task<PaymentCreationResult> CreatePaymentAsync(Order order, CancellationToken cancellationToken = default)
    {
        var txnId = "TXN_" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();
        var qrContent = $"vietqr://pay?acc=1903666888&amount={order.Total}&memo={order.OrderCode}";
        var paymentUrl = $"https://checkout.cellphones.vn/mock-qr?code={order.OrderCode}&amount={order.Total}";

        return Task.FromResult(new PaymentCreationResult(paymentUrl, qrContent, txnId));
    }

    public bool VerifyWebhookSignature(string rawPayload, string signature, string secret)
    {
        if (string.IsNullOrWhiteSpace(rawPayload) || string.IsNullOrWhiteSpace(signature))
        {
            return false;
        }

        var keyBytes = Encoding.UTF8.GetBytes(string.IsNullOrWhiteSpace(secret) ? _secretKey : secret);
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawPayload));
        var computedSignature = Convert.ToHexString(hash).ToLowerInvariant();

        return string.Equals(signature.Trim(), computedSignature, StringComparison.OrdinalIgnoreCase);
    }

    public string GenerateSignature(string rawPayload, string? secret = null)
    {
        var keyBytes = Encoding.UTF8.GetBytes(string.IsNullOrWhiteSpace(secret) ? _secretKey : secret);
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawPayload));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public Task<TransactionQueryResult> QueryTransactionAsync(string orderCode, string? providerTxnId = null, CancellationToken cancellationToken = default)
    {
        if (_mockTransactions.TryGetValue(orderCode, out var result))
        {
            return Task.FromResult(result);
        }

        return Task.FromResult(new TransactionQueryResult(false, 0, providerTxnId ?? string.Empty, "NOT_FOUND"));
    }

    public virtual Task<RefundResult> RefundAsync(string providerTxnId, long amount, string reason, string idempotencyRefundId, CancellationToken cancellationToken = default)
    {
        var refundId = "REF_" + idempotencyRefundId;
        return Task.FromResult(new RefundResult(true, refundId, null));
    }
}
