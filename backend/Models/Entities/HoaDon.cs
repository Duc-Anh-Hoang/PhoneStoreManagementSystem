using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("hoadon")]
public class HoaDon
{
    [Key]
    [Column("sohd")]
    [MaxLength(15)]
    public string SoHD { get; set; } = string.Empty;

    [Column("ngaylap")]
    public DateTime NgayLap { get; set; } = DateTime.UtcNow;

    [Column("makh")]
    [MaxLength(10)]
    public string? MaKH { get; set; }

    [ForeignKey(nameof(MaKH))]
    public KhachHang? KhachHang { get; set; }

    [Column("manv")]
    [MaxLength(10)]
    public string MaNV { get; set; } = string.Empty;

    [ForeignKey(nameof(MaNV))]
    public NhanVien? NhanVien { get; set; }

    [Column("makm")]
    [MaxLength(10)]
    public string? MaKM { get; set; }

    [ForeignKey(nameof(MaKM))]
    public KhuyenMai? KhuyenMai { get; set; }

    [Column("kenhban")]
    [MaxLength(10)]
    public string KenhBan { get; set; } = "tai_quay";

    [Column("tongtien")]
    public decimal TongTien { get; set; }

    [Column("tiengiamgia")]
    public decimal TienGiamGia { get; set; }

    [Column("thanhtien")]
    public decimal ThanhTien { get; set; }

    [Column("phuongthucthanhtoan")]
    [MaxLength(20)]
    public string PhuongThucThanhToan { get; set; } = "tien_mat";

    public ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
}
