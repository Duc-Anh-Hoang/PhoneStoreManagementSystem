using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("donhangonline")]
public class DonHangOnline
{
    [Key]
    [Column("madh")]
    [MaxLength(15)]
    public string MaDH { get; set; } = string.Empty;

    [Column("ngaydat")]
    public DateTime NgayDat { get; set; } = DateTime.UtcNow;

    [Column("makh")]
    [MaxLength(10)]
    public string MaKH { get; set; } = string.Empty;

    [ForeignKey(nameof(MaKH))]
    public KhachHang? KhachHang { get; set; }

    [Column("tennguoinhan")]
    [MaxLength(100)]
    public string TenNguoiNhan { get; set; } = string.Empty;

    [Column("sdtnguoinhan")]
    [MaxLength(15)]
    public string SdtNguoiNhan { get; set; } = string.Empty;

    [Column("diachigiao")]
    [MaxLength(255)]
    public string DiaChiGiao { get; set; } = string.Empty;

    [Column("phuongthucthanhtoan")]
    [MaxLength(20)]
    public string PhuongThucThanhToan { get; set; } = "cod";

    [Column("tongtien")]
    public decimal TongTien { get; set; }

    [Column("trangthai")]
    [MaxLength(20)]
    public string TrangThai { get; set; } = "cho_xac_nhan";

    [Column("manv")]
    [MaxLength(10)]
    public string? MaNV { get; set; }

    [Column("sohd")]
    [MaxLength(15)]
    public string? SoHD { get; set; }

    public ICollection<ChiTietDonHangOnline> ChiTietDonHangs { get; set; } = new List<ChiTietDonHangOnline>();
}
