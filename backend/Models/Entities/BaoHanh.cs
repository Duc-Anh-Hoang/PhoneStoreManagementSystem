using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("baohanh")]
public class BaoHanh
{
    [Key]
    [Column("mabh")]
    [MaxLength(15)]
    public string MaBH { get; set; } = string.Empty;

    [Column("sohd")]
    [MaxLength(15)]
    public string SoHD { get; set; } = string.Empty;

    [Column("maimei")]
    [MaxLength(20)]
    public string MaIMEI { get; set; } = string.Empty;

    [Column("ngaybd")]
    public DateTime NgayBD { get; set; }

    [Column("thoihanthang")]
    public int ThoiHanThang { get; set; }

    [Column("ngaykt")]
    public DateTime? NgayKT { get; set; }

    [Column("dieukien")]
    [MaxLength(255)]
    public string? DieuKien { get; set; }
}
