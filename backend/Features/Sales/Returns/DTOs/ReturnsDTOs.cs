namespace PhoneStoreAPI.Features.Sales.Returns.DTOs;

public class CreateTraHangRequest
{
    public string SoHD { get; set; } = string.Empty;
    public string? MaIMEI { get; set; }
    public string? SoIMEI { get; set; }
    public string LyDo { get; set; } = string.Empty;
    public string HinhThucHoanTien { get; set; } = "tien_mat"; // tien_mat, chuyen_khoan, doi_hang
}

public class TraHangDto
{
    public string MaTR { get; set; } = string.Empty;
    public string SoHD { get; set; } = string.Empty;
    public string MaIMEI { get; set; } = string.Empty;
    public string? SoIMEI { get; set; }
    public string? TenHang { get; set; }
    public DateTime NgayTR { get; set; }
    public string LyDo { get; set; } = string.Empty;
    public string HinhThucHoanTien { get; set; } = string.Empty;
    public string MaNV { get; set; } = string.Empty;
    public string? TenNV { get; set; }
}
