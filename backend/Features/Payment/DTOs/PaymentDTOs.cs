namespace PhoneStoreAPI.Features.Payment.DTOs;

public class VietQrResponse
{
    public string MaDH { get; set; } = string.Empty;
    public string BankId { get; set; } = "MB";
    public string BankBin { get; set; } = "970422";
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Memo { get; set; } = string.Empty;
    public string QrUrl { get; set; } = string.Empty;
    public string QrBase64 { get; set; } = string.Empty;
}

public class ConfirmPaymentRequest
{
    public string? TransactionId { get; set; }
    public decimal? Amount { get; set; }
    public string? Note { get; set; }
}

public class PaymentWebhookPayload
{
    public string? Content { get; set; }
    public decimal? TransferAmount { get; set; }
    public string? Gateway { get; set; }
    public string? ReferenceCode { get; set; }
}

public class PaymentStatusResponse
{
    public string MaDH { get; set; } = string.Empty;
    public string TrangThai { get; set; } = string.Empty;
    public bool DaThanhToan { get; set; }
    public bool IsPaid { get; set; }
    public decimal TongTien { get; set; }
    public string PhuongThucThanhToan { get; set; } = string.Empty;
    public DateTime? NgayDat { get; set; }
}
