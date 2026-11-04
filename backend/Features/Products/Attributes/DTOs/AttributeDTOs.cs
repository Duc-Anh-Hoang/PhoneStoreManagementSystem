namespace PhoneStoreAPI.Features.Products.Attributes.DTOs;

public class ThuocTinhDto
{
    public string MaThuocTinh { get; set; } = string.Empty;
    public string TenThuocTinh { get; set; } = string.Empty;
    public int SoLuongMatHang { get; set; }
}

public class CreateThuocTinhRequest
{
    public string MaThuocTinh { get; set; } = string.Empty;
    public string TenThuocTinh { get; set; } = string.Empty;
}

public class UpdateThuocTinhRequest
{
    public string TenThuocTinh { get; set; } = string.Empty;
}
