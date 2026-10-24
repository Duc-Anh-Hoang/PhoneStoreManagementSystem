using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("trahang")]
public class TraHang
{
    [Key]
    [Column("matr")]
    [MaxLength(15)]
    public string MaTR { get; set; } = string.Empty;

    [Column("sohd")]
    [MaxLength(15)]
    public string SoHD { get; set; } = string.Empty;

    [Column("maimei")]
    [MaxLength(20)]
    public string MaIMEI { get; set; } = string.Empty;

    [Column("ngaytr")]
    public DateTime NgayTR { get; set; } = DateTime.UtcNow;

    [Column("lydo")]
    [MaxLength(255)]
    public string LyDo { get; set; } = string.Empty;

    [Column("hinhthuchoantien")]
    [MaxLength(20)]
    public string HinhThucHoanTien { get; set; } = "tien_mat";

    [Column("manv")]
    [MaxLength(10)]
    public string MaNV { get; set; } = string.Empty;
}
