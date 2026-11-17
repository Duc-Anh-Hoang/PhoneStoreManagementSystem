using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("order_items")]
public class OrderItem
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("order_id")]
    public long OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order? Order { get; set; }

    [Column("variant_id")]
    public long VariantId { get; set; }

    [ForeignKey(nameof(VariantId))]
    public ProductVariant? Variant { get; set; }

    [Column("product_name_snapshot")]
    [MaxLength(255)]
    public string ProductNameSnapshot { get; set; } = string.Empty;

    [Column("sku_snapshot")]
    [MaxLength(100)]
    public string SkuSnapshot { get; set; } = string.Empty;

    [Column("price_snapshot")]
    public long PriceSnapshot { get; set; }

    [Column("qty")]
    public int Qty { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
