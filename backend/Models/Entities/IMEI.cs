using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("imei")]
public class IMEI
{
    [Key]
    [Column("maimei")]
    [MaxLength(20)]
    public string MaIMEI { get; set; } = string.Empty;

    [Column("mahang")]
    [MaxLength(10)]
    public string MaHang { get; set; } = string.Empty;

    [ForeignKey(nameof(MaHang))]
    public MatHang? MatHang { get; set; }

    [Column("sopn")]
    [MaxLength(15)]
    public string SoPN { get; set; } = string.Empty;

    [Column("soimei")]
    [MaxLength(20)]
    public string SoIMEI { get; set; } = string.Empty;

    [Column("trangthai")]
    [MaxLength(20)]
    public string TrangThai { get; set; } = "con_hang";
}
