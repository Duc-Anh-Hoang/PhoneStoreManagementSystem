using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("chitiethoadon")]
public class ChiTietHoaDon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("macthd")]
    public int MaCTHD { get; set; }

    [Column("sohd")]
    [MaxLength(15)]
    public string SoHD { get; set; } = string.Empty;

    [ForeignKey(nameof(SoHD))]
    public HoaDon? HoaDon { get; set; }

    [Column("mahang")]
    [MaxLength(10)]
    public string MaHang { get; set; } = string.Empty;

    [ForeignKey(nameof(MaHang))]
    public MatHang? MatHang { get; set; }

    [Column("maimei")]
    [MaxLength(20)]
    public string? MaIMEI { get; set; }

    [ForeignKey(nameof(MaIMEI))]
    public IMEI? IMEI { get; set; }

    [Column("slban")]
    public int SlBan { get; set; }

    [Column("dongia_ban")]
    public decimal DonGiaBan { get; set; }
}
