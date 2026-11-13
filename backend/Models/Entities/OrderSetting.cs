using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("order_settings")]
public class OrderSetting
{
    [Key]
    [Column("key")]
    [MaxLength(50)]
    public string Key { get; set; } = string.Empty;

    [Column("value")]
    public string Value { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }
}
