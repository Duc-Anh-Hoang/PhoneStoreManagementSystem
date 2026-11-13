using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("order_status_logs")]
public class OrderStatusLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_id")]
    [MaxLength(15)]
    public string OrderId { get; set; } = string.Empty;

    [Column("from_status")]
    [MaxLength(30)]
    public string? FromStatus { get; set; }

    [Column("to_status")]
    [MaxLength(30)]
    public string ToStatus { get; set; } = string.Empty;

    [Column("actor")]
    [MaxLength(30)]
    public string Actor { get; set; } = "system"; // system | staff | ghn | customer

    [Column("reason")]
    public string? Reason { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
