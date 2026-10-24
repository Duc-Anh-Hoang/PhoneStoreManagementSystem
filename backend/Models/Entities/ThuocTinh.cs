using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("thuoctinh")]
public class ThuocTinh
{
    [Key]
    [Column("mathuoctinh")]
    [MaxLength(10)]
    public string MaThuocTinh { get; set; } = string.Empty;

    [Column("tenthuoctinh")]
    [MaxLength(50)]
    public string TenThuocTinh { get; set; } = string.Empty;
}
