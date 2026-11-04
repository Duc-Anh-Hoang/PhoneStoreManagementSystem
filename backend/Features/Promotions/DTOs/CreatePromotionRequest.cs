namespace PhoneStoreAPI.Features.Promotions.DTOs;

public class CreatePromotionRequest
{
    public string MaKM { get; set; } = string.Empty;
    public string TenKM { get; set; } = string.Empty;
    public decimal PhanTramGiam { get; set; }
    public DateTime NgayBD { get; set; }
    public DateTime NgayKT { get; set; }
    public bool TamDung { get; set; } = false;
}

