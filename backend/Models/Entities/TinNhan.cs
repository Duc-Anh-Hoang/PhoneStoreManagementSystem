using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("tinnhan")]
public class TinNhan
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("matinnhan")]
    public long MaTinNhan { get; set; }

    [Column("maphienchat")]
    [MaxLength(36)]
    public string MaPhienChat { get; set; } = string.Empty;

    [Column("nguoi_gui")]
    [MaxLength(20)]
    public string NguoiGui { get; set; } = string.Empty; // "CUSTOMER", "AGENT", "BOT"

    [Column("nguoi_gui_id")]
    [MaxLength(36)]
    public string? NguoiGuiId { get; set; } // Mã nhân viên hoặc ID người gửi

    [Column("noi_dung", TypeName = "text")]
    public string NoiDung { get; set; } = string.Empty;

    [Column("da_doc")]
    public bool DaDoc { get; set; } = false;

    [Column("thoi_gian")]
    public DateTime ThoiGian { get; set; } = DateTime.UtcNow;

    // Navigation property
    public virtual PhienChat? PhienChat { get; set; }
}
