namespace PhoneStoreAPI.Features.Sales.DTOs;

public class CreateHoaDonRequest
{
    public string? MaKH { get; set; }
    public string? MaKM { get; set; }
    public string PhuongThucThanhToan { get; set; } = "tien_mat"; // tien_mat, chuyen_khoan, the
    public string? KenhBan { get; set; } = "tai_quay";
    public List<ChiTietHoaDonRequest> ChiTiet { get; set; } = new();
}

public class ChiTietHoaDonRequest
{
    public string MaHang { get; set; } = string.Empty;
    public int SoLuong { get; set; } = 1;
    public string? MaIMEI { get; set; }
    public string? SoIMEI { get; set; }
}

public class HoaDonDto
{
    public string SoHD { get; set; } = string.Empty;
    public DateTime NgayLap { get; set; }
    public string? MaKH { get; set; }
    public string? TenKhachHang { get; set; }
    public string? TenKH { get; set; }
    public string MaNV { get; set; } = string.Empty;
    public string? TenNV { get; set; }
    public string? MaKM { get; set; }
    public string? TenKM { get; set; }
    public decimal? PhanTramGiam { get; set; }
    public string KenhBan { get; set; } = "tai_quay";
    public decimal TongTien { get; set; }
    public decimal TienGiamGia { get; set; }
    public decimal ThanhTien { get; set; }
    public decimal TienVatDaGom { get; set; }
    public string PhuongThucThanhToan { get; set; } = string.Empty;
    public List<ChiTietHoaDonDto> ChiTiet { get; set; } = new();
}

public class ChiTietHoaDonDto
{
    public int MaCTHD { get; set; }
    public string MaHang { get; set; } = string.Empty;
    public string TenHang { get; set; } = string.Empty;
    public string? MaIMEI { get; set; }
    public string? SoIMEI { get; set; }
    public int SlBan { get; set; }
    public decimal DonGiaBan { get; set; }
    public decimal ThanhTienDong { get; set; }
}
