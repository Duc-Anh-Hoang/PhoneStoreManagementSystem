using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("phieunhap")]
public class PhieuNhap
{
    [Key]
    [Column("sopn")]
    [MaxLength(15)]
    public string SoPN { get; set; } = string.Empty;

    [Column("ngaynhap")]
    public DateTime NgayNhap { get; set; } = DateTime.UtcNow;

    [Column("mancc")]
    [MaxLength(10)]
    public string MaNCC { get; set; } = string.Empty;

    [ForeignKey(nameof(MaNCC))]
    public NhaCungCap? NhaCungCap { get; set; }

    [Column("manv")]
    [MaxLength(10)]
    public string MaNV { get; set; } = string.Empty;

    [ForeignKey(nameof(MaNV))]
    public NhanVien? NhanVien { get; set; }

    public ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();
}
