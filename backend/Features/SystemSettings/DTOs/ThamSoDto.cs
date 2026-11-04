namespace PhoneStoreAPI.Features.SystemSettings.DTOs;

public class ThamSoDto
{
    public string MaThamSo { get; set; } = string.Empty;
    public string TenThamSo { get; set; } = string.Empty;
    public decimal GiaTri { get; set; }
    public string? DonVi { get; set; }
}

