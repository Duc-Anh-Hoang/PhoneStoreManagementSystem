namespace PhoneStoreAPI.Features.Products.Categories.DTOs;

public class NhomHangDto
{
    public string MaNhom { get; set; } = string.Empty;
    public string TenNhom { get; set; } = string.Empty;
    public int SoLuongMatHang { get; set; }
}

public class CreateNhomHangRequest
{
    public string MaNhom { get; set; } = string.Empty;
    public string TenNhom { get; set; } = string.Empty;
}

public class UpdateNhomHangRequest
{
    public string TenNhom { get; set; } = string.Empty;
}

public class LoaiHangDto
{
    public string MaLoai { get; set; } = string.Empty;
    public string TenLoai { get; set; } = string.Empty;
    public int SoLuongMatHang { get; set; }
}

public class CreateLoaiHangRequest
{
    public string MaLoai { get; set; } = string.Empty;
    public string TenLoai { get; set; } = string.Empty;
}

public class UpdateLoaiHangRequest
{
    public string TenLoai { get; set; } = string.Empty;
}
