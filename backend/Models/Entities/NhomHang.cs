using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("nhomhang")]
public class NhomHang
{
    [Key]
    [Column("manhom")]
    [MaxLength(10)]
    public string MaNhom { get; set; } = string.Empty;

    [Column("tennhom")]
    [MaxLength(50)]
    public string TenNhom { get; set; } = string.Empty;
}
