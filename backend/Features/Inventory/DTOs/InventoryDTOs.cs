namespace PhoneStoreAPI.Features.Inventory.DTOs;

public class TonKhoDto
{
    public string MaHang { get; set; } = string.Empty;
    public string TenHang { get; set; } = string.Empty;
    public string? MaNhom { get; set; }
    public string? MaLoai { get; set; }
    public decimal DonGia { get; set; }
    public long TongNhap { get; set; }
    public long TongBan { get; set; }
    public long TonKho { get; set; }
    public bool CanhBao { get; set; }
}

public class TonKhoQueryParameters
{
    public string? Keyword { get; set; }
    public string? MaNhom { get; set; }
    public string? MaLoai { get; set; }
    public bool? ChiCanhBao { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class ImeiDto
{
    public string MaIMEI { get; set; } = string.Empty;
    public string SoIMEI { get; set; } = string.Empty;
    public string MaHang { get; set; } = string.Empty;
    public string? TenHang { get; set; }
    public string SoPN { get; set; } = string.Empty;
    public DateTime? NgayNhap { get; set; }
    public string TrangThai { get; set; } = "con_hang";
}

public class ImeiQueryParameters
{
    public string? MaHang { get; set; }
    public string? TrangThai { get; set; } // "con_hang", "da_ban", "tra_hang"
    public string? Keyword { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
