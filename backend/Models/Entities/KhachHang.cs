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

    [Column("completed_orders")]
    public int CompletedOrders { get; set; } = 0;

    [Column("qualified_orders")]
    public int QualifiedOrders { get; set; } = 0;

    [Column("max_completed_value")]
    public decimal MaxCompletedValue { get; set; } = 0;

    [Column("failed_orders")]
    public int FailedOrders { get; set; } = 0;

    [Column("is_blacklisted")]
    public bool IsBlacklisted { get; set; } = false;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
