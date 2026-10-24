using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("chitietdonhangonline")]
public class ChiTietDonHangOnline
{
    [Column("madh")]
    [MaxLength(15)]
    public string MaDH { get; set; } = string.Empty;

    [ForeignKey(nameof(MaDH))]
    public DonHangOnline? DonHangOnline { get; set; }

    [Column("mahang")]
    [MaxLength(10)]
    public string MaHang { get; set; } = string.Empty;

    [ForeignKey(nameof(MaHang))]
    public MatHang? MatHang { get; set; }

    [Column("soluong")]
    public int SoLuong { get; set; }

    [Column("dongia")]
    public decimal DonGia { get; set; }
}
