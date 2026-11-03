namespace PhoneStoreAPI.Features.Products.DTOs;

public class ProductDto
{
    public string MaHang { get; set; } = string.Empty;
    public string TenHang { get; set; } = string.Empty;
    public decimal DonGia { get; set; }
    public string Dvt { get; set; } = "Cái";
    public string MaNhom { get; set; } = string.Empty;
    public string? TenNhom { get; set; }
    public string MaLoai { get; set; } = string.Empty;
    public string? TenLoai { get; set; }
    public bool QuanLyImei { get; set; }
    public int ThoiHanBaoHanh { get; set; }
    public int TonKho { get; set; }
}

public class ProductSpecDto
{
    public string MaThuocTinh { get; set; } = string.Empty;
    public string TenThuocTinh { get; set; } = string.Empty;
    public string GiaTri { get; set; } = string.Empty;
}
