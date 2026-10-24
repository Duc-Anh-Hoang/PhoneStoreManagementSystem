using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("mathang")]
public class MatHang
{
    [Key]
    [Column("mahang")]
    [MaxLength(10)]
    public string MaHang { get; set; } = string.Empty;

    [Column("tenhang")]
    [MaxLength(100)]
    public string TenHang { get; set; } = string.Empty;

    [Column("dongia")]
    public decimal DonGia { get; set; }

    [Column("dvt")]
    [MaxLength(20)]
    public string Dvt { get; set; } = "Cái";

    [Column("manhom")]
    [MaxLength(10)]
    public string MaNhom { get; set; } = string.Empty;

    [ForeignKey(nameof(MaNhom))]
    public NhomHang? NhomHang { get; set; }

    [Column("maloai")]
    [MaxLength(10)]
    public string MaLoai { get; set; } = string.Empty;

    [ForeignKey(nameof(MaLoai))]
    public LoaiHang? LoaiHang { get; set; }

    [Column("quanlyimei")]
    public bool QuanLyImei { get; set; }

    [Column("thoihanbaohanh")]
    public int ThoiHanBaoHanh { get; set; }
}
