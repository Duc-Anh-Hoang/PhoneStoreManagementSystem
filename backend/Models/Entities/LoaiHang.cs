using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("loaihang")]
public class LoaiHang
{
    [Key]
    [Column("maloai")]
    [MaxLength(10)]
    public string MaLoai { get; set; } = string.Empty;

    [Column("tenloai")]
    [MaxLength(50)]
    public string TenLoai { get; set; } = string.Empty;
}
