using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("taikhoan")]
public class TaiKhoan
{
    [Key]
    [Column("username")]
    [MaxLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Column("password_hash")]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("manv")]
    [MaxLength(10)]
    public string MaNV { get; set; } = string.Empty;

    [ForeignKey(nameof(MaNV))]
    public NhanVien? NhanVien { get; set; }

    [Column("maquyen")]
    [MaxLength(10)]
    public string MaQuyen { get; set; } = string.Empty;

    [ForeignKey(nameof(MaQuyen))]
    public PhanQuyen? PhanQuyen { get; set; }

    [Column("trangthai")]
    [MaxLength(10)]
    public string TrangThai { get; set; } = "hoat_dong";

    [Column("ngaytao")]
    public DateTime NgayTao { get; set; } = DateTime.UtcNow;
}
