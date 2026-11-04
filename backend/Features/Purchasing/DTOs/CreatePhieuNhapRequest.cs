namespace PhoneStoreAPI.Features.Purchasing.DTOs;

public class CreatePhieuNhapRequest
{
    public string MaNCC { get; set; } = string.Empty;
    public List<ChiTietNhapItemRequest> ChiTiet { get; set; } = new();
}

