using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("trahang")]
public class TraHang
{
    [Key]
    [Column("matr")]
    [MaxLength(15)]
    public string MaTR { get; set; } = string.Empty;

    [Column("sohd")]
    [MaxLength(15)]
    public string SoHD { get; set; } = string.Empty;

    [ForeignKey(nameof(SoHD))]
    public HoaDon? HoaDon { get; set; }

    [Column("maimei")]
    [MaxLength(20)]
    public string MaIMEI { get; set; } = string.Empty;

    [ForeignKey(nameof(MaIMEI))]
    public IMEI? IMEI { get; set; }

    [Column("loaiyeucau")]
    [MaxLength(20)]
    public string LoaiYeuCau { get; set; } = "loi_ky_thuat"; // loi_ky_thuat, doi_y

    [Column("hinhthuchoantien")]
    [MaxLength(20)]
    public string HinhThucHoanTien { get; set; } = "tien_mat"; // tien_mat, chuyen_khoan, doi_hang

    [Column("lydo")]
    [MaxLength(255)]
    public string LyDo { get; set; } = string.Empty;

    [Column("trangthai")]
    [MaxLength(20)]
    public string TrangThai { get; set; } = "tiep_nhan"; // tiep_nhan, da_kiem_tra, da_duyet, tu_choi, hoan_tat, da_huy

    [Column("ngaytr")]
    public DateTime NgayTR { get; set; } = DateTime.UtcNow;

    [Column("manv")]
    [MaxLength(10)]
    public string MaNV { get; set; } = string.Empty;

    [ForeignKey(nameof(MaNV))]
    public NhanVien? NhanVienTiepNhan { get; set; }

    [Column("checklist_ngoaiquan")]
    public string? ChecklistNgoaiQuan { get; set; }

    [Column("ketqua_kiemtra")]
    [MaxLength(30)]
    public string? KetQuaKiemTra { get; set; } // loi_nsx, khong_loi, hu_hong_do_nguoi_dung

    [Column("ghichu_kiemtra")]
    public string? GhiChuKiemTra { get; set; }

    [Column("manv_kiemtra")]
    [MaxLength(10)]
    public string? MaNVKiemTra { get; set; }

    [ForeignKey(nameof(MaNVKiemTra))]
    public NhanVien? NhanVienKiemTra { get; set; }

    [Column("ngay_kiemtra")]
    public DateTime? NgayKiemTra { get; set; }

    [Column("manv_duyet")]
    [MaxLength(10)]
    public string? MaNVDuyet { get; set; }

    [ForeignKey(nameof(MaNVDuyet))]
    public NhanVien? NhanVienDuyet { get; set; }

    [Column("ngay_duyet")]
    public DateTime? NgayDuyet { get; set; }

    [Column("lydo_tuchoi")]
    [MaxLength(255)]
    public string? LyDoTuChoi { get; set; }

    [Column("sotienhoan")]
    public decimal? SoTienHoan { get; set; }

    [Column("magiaodich")]
    [MaxLength(50)]
    public string? MaGiaoDich { get; set; }

    [Column("maimei_moi")]
    [MaxLength(20)]
    public string? MaIMEIMoi { get; set; }

    [ForeignKey(nameof(MaIMEIMoi))]
    public IMEI? IMEIMoi { get; set; }

    [Column("manv_hoantat")]
    [MaxLength(10)]
    public string? MaNVHoanTat { get; set; }

    [ForeignKey(nameof(MaNVHoanTat))]
    public NhanVien? NhanVienHoanTat { get; set; }

    [Column("ngay_hoantat")]
    public DateTime? NgayHoanTat { get; set; }

    public ICollection<TraHangLichSu> LichSu { get; set; } = new List<TraHangLichSu>();
}
