using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("cart_items")]
public class CartItem
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("cart_id")]
    public long CartId { get; set; }

    [ForeignKey(nameof(CartId))]
    public Cart? Cart { get; set; }

    [Column("variant_id")]
    public long VariantId { get; set; }

    [ForeignKey(nameof(VariantId))]
    public ProductVariant? Variant { get; set; }

    [Column("qty")]
    public int Qty { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
