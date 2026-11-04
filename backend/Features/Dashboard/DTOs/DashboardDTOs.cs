namespace PhoneStoreAPI.Features.Dashboard.DTOs;

public class DashboardSummaryDto
{
    public decimal DoanhThuHomNay { get; set; }
    public decimal DoanhThuThang { get; set; }
    public decimal TongDoanhThu { get; set; }
    public int SoHoaDonHomNay { get; set; }
    public int TongHoaDon { get; set; }
    public int DonOnlineChoXacNhan { get; set; }
    public int TongMatHang { get; set; }
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
