using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("customer_addresses")]
public class CustomerAddress
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("phone")]
    [MaxLength(15)]
    public string Phone { get; set; } = string.Empty;

    [Column("address_hash")]
    [MaxLength(64)]
    public string AddressHash { get; set; } = string.Empty;

    [Column("full_address")]
    [MaxLength(255)]
    public string FullAddress { get; set; } = string.Empty;

    [Column("last_delivered_at")]
    public DateTime LastDeliveredAt { get; set; } = DateTime.UtcNow;
}
