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

    [Column("discount_percent")]
    public int DiscountPercent { get; set; }

    public decimal DiscountFor(decimal subtotal) => DiscountPercent > 0
        ? Math.Round(subtotal * DiscountPercent / 100m, MidpointRounding.AwayFromZero)
        : DiscountAmount;

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
