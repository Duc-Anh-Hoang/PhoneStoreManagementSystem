using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("webhook_events")]
public class WebhookEvent
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("source")]
    [MaxLength(30)]
    public string Source { get; set; } = string.Empty;

    [Column("event_id")]
    [MaxLength(100)]
    public string EventId { get; set; } = string.Empty;

    [Column("payload")]
    public string? Payload { get; set; }

    [Column("received_at")]
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
}
