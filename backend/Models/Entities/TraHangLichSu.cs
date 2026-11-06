using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("trahanglichsu")]
public class TraHangLichSu
{
    [Key]
    [Column("malichsu")]
    public long MaLichSu { get; set; }

    [Column("matr")]
    [MaxLength(15)]
    public string MaTR { get; set; } = string.Empty;

    [ForeignKey(nameof(MaTR))]
    public TraHang? TraHang { get; set; }

    [Column("trangthai_cu")]
    [MaxLength(20)]
    public string? TrangThaiCu { get; set; }

    [Column("trangthai_moi")]
    [MaxLength(20)]
    public string TrangThaiMoi { get; set; } = string.Empty;

    [Column("manv")]
    [MaxLength(10)]
    public string MaNV { get; set; } = string.Empty;

    [ForeignKey(nameof(MaNV))]
    public NhanVien? NhanVien { get; set; }

    [Column("thoidiem")]
    public DateTime ThoiDiem { get; set; } = DateTime.UtcNow;

    [Column("ghichu")]
    [MaxLength(255)]
    public string? GhiChu { get; set; }
}
