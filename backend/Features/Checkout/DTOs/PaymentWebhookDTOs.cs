namespace PhoneStoreAPI.Features.Checkout.DTOs;

public class QrWebhookPayload
{
    public string OrderCode { get; set; } = string.Empty;
    public string ProviderTxnId { get; set; } = string.Empty;
    public long Amount { get; set; }
    public string Status { get; set; } = "SUCCESS";
}

public class WebhookProcessResult
{
    public bool Success { get; set; }
    public int StatusCode { get; set; } = 200;
    public string Message { get; set; } = string.Empty;
    public string? FinalOrderStatus { get; set; }
}
