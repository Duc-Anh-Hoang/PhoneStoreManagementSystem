using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("refund_requests")]
public class RefundRequest
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("order_id")]
    public long OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    [Column("payment_id")]
    public long? PaymentId { get; set; }

    [ForeignKey(nameof(PaymentId))]
    public PaymentRecord? Payment { get; set; }

    [Column("amount")]
    public long Amount { get; set; }

    [Column("reason")]
    [MaxLength(255)]
    public string Reason { get; set; } = string.Empty;

    [Column("status")]
    [MaxLength(32)]
    public string Status { get; set; } = "PENDING"; // PENDING, PROCESSING, COMPLETED, FAILED

    [Column("provider_refund_id")]
    [MaxLength(100)]
    public string? ProviderRefundId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
