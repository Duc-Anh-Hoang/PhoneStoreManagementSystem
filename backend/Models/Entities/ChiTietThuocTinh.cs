using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("chitietthuoctinh")]
public class ChiTietThuocTinh
{
    [Column("mahang")]
    [MaxLength(10)]
    public string MaHang { get; set; } = string.Empty;

    [ForeignKey(nameof(MaHang))]
    public MatHang? MatHang { get; set; }

    [Column("mathuoctinh")]
    [MaxLength(10)]
    public string MaThuocTinh { get; set; } = string.Empty;

    [ForeignKey(nameof(MaThuocTinh))]
    public ThuocTinh? ThuocTinh { get; set; }

    [Column("gia_tri")]
    [MaxLength(100)]
    public string GiaTri { get; set; } = string.Empty;
}
