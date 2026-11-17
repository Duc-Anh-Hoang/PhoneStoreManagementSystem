using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("variants")]
public class ProductVariant
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("product_id")]
    public long ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    [Column("sku")]
    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    [Column("color")]
    [MaxLength(50)]
    public string Color { get; set; } = string.Empty;

    [Column("storage")]
    [MaxLength(50)]
    public string Storage { get; set; } = string.Empty;

    [Column("price")]
    public long Price { get; set; }

    [Column("stock")]
    public int Stock { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
