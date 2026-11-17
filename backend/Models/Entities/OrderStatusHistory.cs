using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("order_status_history")]
public class OrderStatusHistory
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("order_id")]
    public long OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    [Column("from_status")]
    [MaxLength(32)]
    public string? FromStatus { get; set; }

    [Column("to_status")]
    [MaxLength(32)]
    public string ToStatus { get; set; } = string.Empty;

    [Column("reason")]
    [MaxLength(255)]
    public string? Reason { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
