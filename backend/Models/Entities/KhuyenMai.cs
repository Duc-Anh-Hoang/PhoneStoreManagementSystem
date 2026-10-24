using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("khuyenmai")]
public class KhuyenMai
{
    [Key]
    [Column("makm")]
    [MaxLength(10)]
    public string MaKM { get; set; } = string.Empty;

    [Column("tenkm")]
    [MaxLength(100)]
    public string TenKM { get; set; } = string.Empty;

    [Column("phantramgiam")]
    public decimal PhanTramGiam { get; set; }

    [Column("ngaybd")]
    public DateTime NgayBD { get; set; }

    [Column("ngaykt")]
    public DateTime NgayKT { get; set; }

    [Column("tamdung")]
    public bool TamDung { get; set; }
}
