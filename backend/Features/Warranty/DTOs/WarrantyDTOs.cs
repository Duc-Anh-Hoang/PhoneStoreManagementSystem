namespace PhoneStoreAPI.Features.Warranty.DTOs;

public class WarrantyLookupDto
{
    public string TenHang { get; set; } = string.Empty;
    public string SoIMEI { get; set; } = string.Empty;
    public DateTime NgayBD { get; set; }
    public DateTime? NgayKT { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public int SoNgayConLai { get; set; }
}

public class WarrantyStatusDto
{
    public string SoIMEI { get; set; } = string.Empty;
    public string TrangThai { get; set; } = string.Empty;
    public int SoNgayConLai { get; set; }
    public bool ConHan { get; set; }
}
