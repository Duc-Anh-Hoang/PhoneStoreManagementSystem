namespace PhoneStoreAPI.Features.Purchasing.DTOs;

public class PhieuNhapDto
{
    public string SoPN { get; set; } = string.Empty;

    /// <summary>
    /// Alias tương thích frontend admin.js đọc soPNhap
    /// </summary>
    public string SoPNhap => SoPN;

    public DateTime NgayNhap { get; set; }
    public string MaNCC { get; set; } = string.Empty;
    public string TenNCC { get; set; } = string.Empty;
    public string MaNV { get; set; } = string.Empty;
    public string TenNhanVien { get; set; } = string.Empty;
    public decimal TongTien { get; set; }
    public int TongSoLuong { get; set; }
    public List<ChiTietPhieuNhapDto> ChiTiet { get; set; } = new();
}

