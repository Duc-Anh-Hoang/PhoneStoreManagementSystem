namespace PhoneStoreAPI.Features.Promotions.DTOs;

public class UpdatePromotionRequest
{
    public string TenKM { get; set; } = string.Empty;
    public decimal PhanTramGiam { get; set; }
    public DateTime NgayBD { get; set; }
    public DateTime NgayKT { get; set; }
    public bool TamDung { get; set; }
}

