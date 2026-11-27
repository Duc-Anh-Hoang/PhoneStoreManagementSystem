namespace PhoneStoreAPI.Features.SystemSettings.DTOs;

public class UpdateThamSoRequest
{
    public string? MaThamSo { get; set; }
    public decimal GiaTri { get; set; }
    public string? TenThamSo { get; set; }
    public string? DonVi { get; set; }
}


public class CreateThamSoRequest : UpdateThamSoRequest
{
}
