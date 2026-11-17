using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("vouchers")]
public class Voucher
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("code")]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Column("discount_amount")]
    public long DiscountAmount { get; set; }

    [Column("min_order_total")]
    public long MinOrderTotal { get; set; }

    [Column("max_uses")]
    public int MaxUses { get; set; }

    [Column("used_count")]
    public int UsedCount { get; set; }

    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
