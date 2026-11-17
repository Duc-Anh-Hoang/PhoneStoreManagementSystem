using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("payments")]
public class PaymentRecord
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("order_id")]
    public long OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    [Column("provider")]
    [MaxLength(50)]
    public string Provider { get; set; } = string.Empty;

    [Column("provider_txn_id")]
    [MaxLength(100)]
    public string ProviderTxnId { get; set; } = string.Empty;

    [Column("amount")]
    public long Amount { get; set; }

    [Column("status")]
    [MaxLength(32)]
    public string Status { get; set; } = string.Empty;

    [Column("raw_payload")]
    public string? RawPayload { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
