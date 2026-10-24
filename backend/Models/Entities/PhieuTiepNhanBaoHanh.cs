using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("phieutiepnhanbaohanh")]
public class PhieuTiepNhanBaoHanh
{
    [Key]
    [Column("matn")]
    [MaxLength(15)]
    public string MaTN { get; set; } = string.Empty;

    [Column("mabh")]
    [MaxLength(15)]
    public string MaBH { get; set; } = string.Empty;

    [Column("ngaynhan")]
    public DateTime NgayNhan { get; set; } = DateTime.UtcNow;

    [Column("tinhtrangloi")]
    [MaxLength(255)]
    public string TinhTrangLoi { get; set; } = string.Empty;

    [Column("manv_tiepnhan")]
    [MaxLength(10)]
    public string MaNVTiepNhan { get; set; } = string.Empty;

    [Column("manv_kythuat")]
    [MaxLength(10)]
    public string? MaNVKyThuat { get; set; }

    [Column("ngayhentra")]
    public DateTime? NgayHenTra { get; set; }

    [Column("ngaytra")]
    public DateTime? NgayTra { get; set; }

    [Column("ketqua")]
    [MaxLength(255)]
    public string? KetQua { get; set; }

    [Column("chiphi")]
    public decimal ChiPhi { get; set; }

    [Column("trangthai")]
    [MaxLength(20)]
    public string TrangThai { get; set; } = "tiep_nhan";
}
