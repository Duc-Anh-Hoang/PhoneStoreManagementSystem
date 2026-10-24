using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("khachhang")]
public class KhachHang
{
    [Key]
    [Column("makh")]
    [MaxLength(10)]
    public string MaKH { get; set; } = string.Empty;

    [Column("hoten")]
    [MaxLength(100)]
    public string HoTen { get; set; } = string.Empty;

    [Column("diachi")]
    [MaxLength(255)]
    public string? DiaChi { get; set; }

    [Column("sdt")]
    [MaxLength(15)]
    public string? Sdt { get; set; }

    [Column("email")]
    [MaxLength(100)]
    public string? Email { get; set; }
}
