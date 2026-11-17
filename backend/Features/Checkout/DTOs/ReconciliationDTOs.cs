namespace PhoneStoreAPI.Features.Checkout.DTOs;

public class ReconciliationDiscrepancyDTO
{
    public string Type { get; set; } = string.Empty; // MONEY_RECEIVED_ORDER_CANCELLED, ORDER_PAID_NO_PAYMENT, AMOUNT_MISMATCH
    public string OrderCode { get; set; } = string.Empty;
    public string ProviderTxnId { get; set; } = string.Empty;
    public long ExpectedAmount { get; set; }
    public long ActualAmount { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class ReconciliationReportDTO
{
    public DateTime ReconciledAt { get; set; } = DateTime.UtcNow;
    public int TotalOrdersChecked { get; set; }
    public int TotalPaymentsChecked { get; set; }
    public int DiscrepancyCount => Discrepancies.Count;
    public List<ReconciliationDiscrepancyDTO> Discrepancies { get; set; } = new();
}
