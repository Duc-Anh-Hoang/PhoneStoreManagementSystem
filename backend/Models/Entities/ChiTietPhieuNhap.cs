using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("chitietphieunhap")]
public class ChiTietPhieuNhap
{
    [Column("sopn")]
    [MaxLength(15)]
    public string SoPN { get; set; } = string.Empty;

    [ForeignKey(nameof(SoPN))]
    public PhieuNhap? PhieuNhap { get; set; }

    [Column("mahang")]
    [MaxLength(10)]
    public string MaHang { get; set; } = string.Empty;

    [ForeignKey(nameof(MaHang))]
    public MatHang? MatHang { get; set; }

    [Column("slnhap")]
    public int SlNhap { get; set; }

    [Column("dongia_nhap")]
    public decimal DonGiaNhap { get; set; }
}
