using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("thamso")]
public class ThamSo
{
    [Key]
    [Column("mathamso")]
    [MaxLength(30)]
    public string MaThamSo { get; set; } = string.Empty;

    [Column("tenthamso")]
    [MaxLength(100)]
    public string TenThamSo { get; set; } = string.Empty;

    [Column("giatri")]
    public decimal GiaTri { get; set; }

    [Column("donvi")]
    [MaxLength(20)]
    public string? DonVi { get; set; }
}
