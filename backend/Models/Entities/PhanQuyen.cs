using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("phanquyen")]
public class PhanQuyen
{
    [Key]
    [Column("maquyen")]
    [MaxLength(10)]
    public string MaQuyen { get; set; } = string.Empty;

    [Column("tenquyen")]
    [MaxLength(50)]
    public string TenQuyen { get; set; } = string.Empty;

    [Column("mota")]
    [MaxLength(255)]
    public string? MoTa { get; set; }
}
