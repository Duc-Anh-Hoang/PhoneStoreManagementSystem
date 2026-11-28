namespace PhoneStoreAPI.Features.Dashboard.DTOs;

public class DashboardSummaryDto
{
    public decimal DoanhThuHomNay { get; set; }
    public decimal DoanhThuThang { get; set; }
    public decimal TongDoanhThu { get; set; }
    public decimal DoanhThuGop { get; set; }
    public decimal TongTienHoanTra { get; set; }
    public int SoDonHoanTra { get; set; }
    public decimal TongChiPhiNhap { get; set; }
    public int SoPhieuNhap { get; set; }
    public decimal GiaVonBanHang { get; set; }
    public decimal DoanhThuTong { get; set; }
    public int SoHoaDonHomNay { get; set; }
    public int TongHoaDon { get; set; }
    public int SoHoaDonTaiQuay { get; set; }
    public int SoHoaDonOnline { get; set; }
    public decimal DoanhThuTaiQuay { get; set; }
    public decimal DoanhThuOnline { get; set; }
    public int DonOnlineChoXacNhan { get; set; }
    public int TongMatHang { get; set; }
    public int TongNhomHang { get; set; }
    public int TongTonKho { get; set; }
    public int SoMatHangSapHet { get; set; }
    public int SoBaoHanhDangXuLy { get; set; }
}

public class ChannelRevenueDto
{
    public string KenhBan { get; set; } = string.Empty;
    public decimal DoanhThu { get; set; }
    public int SoHoaDon { get; set; }
}

public class CategoryRevenueDto
{
    public string MaNhom { get; set; } = string.Empty;
    public string TenNhom { get; set; } = string.Empty;
    public decimal DoanhThu { get; set; }
    public int SoLuongBan { get; set; }
}

public class ProductPerformanceDto
{
    public string MaHang { get; set; } = string.Empty;
    public string TenHang { get; set; } = string.Empty;
    public string TenNhom { get; set; } = string.Empty;
    public decimal DonGia { get; set; }
    public int SoLuongBan { get; set; }
    public decimal DoanhThu { get; set; }
    public int TonKhoHienTai { get; set; }
}

public class TopCustomerDto
{
    public string MaKH { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string Sdt { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int SoDonHang { get; set; }
    public decimal TongChiTieu { get; set; }
    public string HangThanhVien { get; set; } = "Thành viên";
}
