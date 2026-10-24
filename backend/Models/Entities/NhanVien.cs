using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("nhanvien")]
public class NhanVien
{
    [Key]
    [Column("manv")]
    [MaxLength(10)]
    public string MaNV { get; set; } = string.Empty;

    [Column("hoten")]
    [MaxLength(100)]
    public string HoTen { get; set; } = string.Empty;

    [Column("chucvu")]
    [MaxLength(50)]
    public string ChucVu { get; set; } = string.Empty;

    [Column("sdt")]
    [MaxLength(15)]
    public string? Sdt { get; set; }

    [Column("email")]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Column("ngayvaolam")]
    public DateTime NgayVaoLam { get; set; } = DateTime.UtcNow;

    [Column("trangthai")]
    [MaxLength(10)]
    public string TrangThai { get; set; } = "dang_lam";
}
