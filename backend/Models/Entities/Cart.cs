using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("carts")]
public class Cart
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("cart_token")]
    [MaxLength(64)]
    public string? CartToken { get; set; }

    [Column("user_id")]
    [MaxLength(50)]
    public string? UserId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
