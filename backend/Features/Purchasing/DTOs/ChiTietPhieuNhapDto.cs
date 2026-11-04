namespace PhoneStoreAPI.Features.Purchasing.DTOs;

public class ChiTietPhieuNhapDto
{
    public string MaHang { get; set; } = string.Empty;
    public string TenHang { get; set; } = string.Empty;
    public int SlNhap { get; set; }
    public decimal DonGiaNhap { get; set; }
    public decimal ThanhTien => SlNhap * DonGiaNhap;
    public bool QuanLyImei { get; set; }
    public List<string> Imeis { get; set; } = new();
}

