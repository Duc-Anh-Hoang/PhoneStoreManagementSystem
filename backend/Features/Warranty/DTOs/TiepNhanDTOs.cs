namespace PhoneStoreAPI.Features.Warranty.DTOs;

public class CreateTiepNhanRequest
{
    public string SoIMEI { get; set; } = string.Empty;
    public string TinhTrangLoi { get; set; } = string.Empty;
    public string? MaNVKyThuat { get; set; }
    public DateTime? NgayHenTra { get; set; }
    public decimal ChiPhi { get; set; }
}

public class UpdateTiepNhanRequest
{
    public string? MaNVKyThuat { get; set; }
    public string? KetQua { get; set; }
    public decimal? ChiPhi { get; set; }
    public DateTime? NgayHenTra { get; set; }
}

public class UpdateTrangThaiRequest
{
    public string TrangThai { get; set; } = string.Empty;
}

public class TiepNhanBaoHanhDto
{
    public string MaTN { get; set; } = string.Empty;
    public string MaBH { get; set; } = string.Empty;
    public string SoIMEI { get; set; } = string.Empty;
    public DateTime NgayNhan { get; set; }
    public string TinhTrangLoi { get; set; } = string.Empty;
    public string MaNVTiepNhan { get; set; } = string.Empty;
    public string? TenNVTiepNhan { get; set; }
    public string? MaNVKyThuat { get; set; }
    public string? TenNVKyThuat { get; set; }
    public DateTime? NgayHenTra { get; set; }
    public DateTime? NgayTra { get; set; }
    public string? KetQua { get; set; }
    public decimal ChiPhi { get; set; }
    public string TrangThai { get; set; } = string.Empty;
}
