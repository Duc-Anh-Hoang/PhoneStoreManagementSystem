namespace PhoneStoreAPI.Models.Entities.Views;

public class VwTonKho
{
    public string MaHang { get; set; } = string.Empty;
    public string TenHang { get; set; } = string.Empty;
    public string? MaNhom { get; set; }
    public string? MaLoai { get; set; }
    public decimal DonGia { get; set; }
    public long TongNhap { get; set; }
    public long TongBan { get; set; }
    public long TonKho { get; set; }
}
