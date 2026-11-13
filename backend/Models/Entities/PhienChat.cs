using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("phienchat")]
public class PhienChat
{
    [Key]
    [Column("maphienchat")]
    [MaxLength(36)]
    public string MaPhienChat { get; set; } = string.Empty;

    [Column("makh")]
    [MaxLength(10)]
    public string? MaKH { get; set; }

    [Column("khachhang_ten")]
    [MaxLength(100)]
    public string KhachHangTen { get; set; } = "Khách vãng lai";

    [Column("khachhang_sdt")]
    [MaxLength(15)]
    public string? KhachHangSdt { get; set; }

    [Column("manv")]
    [MaxLength(10)]
    public string? MaNV { get; set; } // Nhân viên CSKH tiếp nhận (FK nhanvien)

    [Column("trang_thai")]
    [MaxLength(20)]
    public string TrangThai { get; set; } = "WAITING"; // WAITING, ACTIVE, CLOSED

    [Column("ngay_tao")]
    public DateTime NgayTao { get; set; } = DateTime.UtcNow;

    [Column("ngay_ket_thuc")]
    public DateTime? NgayKetThuc { get; set; }

    // Navigation properties
    public virtual KhachHang? KhachHang { get; set; }
    public virtual NhanVien? NhanVien { get; set; }
    public virtual ICollection<TinNhan> TinNhans { get; set; } = new List<TinNhan>();
}
