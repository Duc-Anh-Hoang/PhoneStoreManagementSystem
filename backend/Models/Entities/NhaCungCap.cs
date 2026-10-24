using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("nhacungcap")]
public class NhaCungCap
{
    [Key]
    [Column("mancc")]
    [MaxLength(10)]
    public string MaNCC { get; set; } = string.Empty;

    [Column("tenncc")]
    [MaxLength(100)]
    public string TenNCC { get; set; } = string.Empty;

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
