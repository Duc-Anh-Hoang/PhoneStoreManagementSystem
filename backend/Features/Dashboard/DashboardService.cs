using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Dashboard.DTOs;

namespace PhoneStoreAPI.Features.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(3);
    private const string SummaryCacheKey = "dashboard_summary";

    public DashboardService(AppDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(DateTime? tuNgay = null, DateTime? denNgay = null)
    {
        var now = DateTime.UtcNow;
        var startOfToday = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var invoices = await _context.HoaDons
            .AsNoTracking()
            .Select(h => new
            {
                h.SoHD,
                h.NgayLap,
                ThanhTien = h.ThanhTien > 0 ? h.ThanhTien : (h.TongTien - h.TienGiamGia),
                KenhBan = h.KenhBan ?? "tai_quay"
            })
            .ToListAsync();

        var refunds = await _context.TraHangs
            .AsNoTracking()
            .Where(t => t.TrangThai == "hoan_tat" && (t.HinhThucHoanTien == "tien_mat" || t.HinhThucHoanTien == "chuyen_khoan"))
            .Select(t => new
            {
                t.MaTR,
                t.SoHD,
                Ngay = t.NgayHoanTat ?? t.NgayTR,
                SoTienHoan = t.SoTienHoan ?? 0m
            })
            .ToListAsync();

        var todayInvoices = invoices.Where(h => h.NgayLap >= startOfToday).ToList();
        var monthInvoices = invoices.Where(h => h.NgayLap >= startOfMonth).ToList();

        var todayRefunds = refunds.Where(r => r.Ngay >= startOfToday).ToList();
        var monthRefunds = refunds.Where(r => r.Ngay >= startOfMonth).ToList();

        decimal grossToday = todayInvoices.Sum(h => h.ThanhTien);
        decimal grossMonth = monthInvoices.Sum(h => h.ThanhTien);

        decimal refundToday = todayRefunds.Sum(r => r.SoTienHoan);
        decimal refundMonth = monthRefunds.Sum(r => r.SoTienHoan);

        decimal doanhThuHomNay = Math.Max(0, grossToday - refundToday);
        decimal doanhThuThang = Math.Max(0, grossMonth - refundMonth);
        int soHoaDonHomNay = todayInvoices.Count;

        // Lọc danh sách theo khoảng thời gian tuNgay - denNgay nếu có
        var filteredInvoices = invoices.AsEnumerable();
        var filteredRefunds = refunds.AsEnumerable();

        if (tuNgay.HasValue)
        {
            filteredInvoices = filteredInvoices.Where(h => h.NgayLap >= tuNgay.Value);
            filteredRefunds = filteredRefunds.Where(r => r.Ngay >= tuNgay.Value);
        }
        if (denNgay.HasValue)
        {
            filteredInvoices = filteredInvoices.Where(h => h.NgayLap <= denNgay.Value);
            filteredRefunds = filteredRefunds.Where(r => r.Ngay <= denNgay.Value);
        }

        var activeInvoices = filteredInvoices.ToList();
        var activeRefunds = filteredRefunds.ToList();

        decimal grossTotal = activeInvoices.Sum(h => h.ThanhTien);
        decimal refundTotal = activeRefunds.Sum(r => r.SoTienHoan);

        // Doanh thu thuần = Doanh thu bán - Tiền hoàn trả trong kỳ
        decimal tongDoanhThu = Math.Max(0, grossTotal - refundTotal);
        int tongHoaDon = activeInvoices.Count;

        var taiQuayInvoices = activeInvoices.Where(h => h.KenhBan == "tai_quay").ToList();
        var onlineInvoices = activeInvoices.Where(h => h.KenhBan == "online").ToList();
        var taiQuayHDSet = taiQuayInvoices.Select(h => h.SoHD).ToHashSet();

        decimal posRefunds = activeRefunds.Where(r => r.SoHD != null && taiQuayHDSet.Contains(r.SoHD)).Sum(r => r.SoTienHoan);
        decimal onlineRefunds = refundTotal - posRefunds;

        decimal doanhThuTaiQuay = Math.Max(0, taiQuayInvoices.Sum(h => h.ThanhTien) - posRefunds);
        decimal doanhThuOnline = Math.Max(0, onlineInvoices.Sum(h => h.ThanhTien) - onlineRefunds);

        int pendingOrders = await _context.DonHangOnlines
            .AsNoTracking()
            .CountAsync(d => d.TrangThai == "cho_xac_nhan");

        int warrantyProcessing = await _context.PhieuTiepNhanBaoHanhs
            .AsNoTracking()
            .CountAsync(t => t.TrangThai == "tiep_nhan" || t.TrangThai == "dang_sua" || t.TrangThai == "cho_linh_kien");

        var allProducts = await _context.MatHangs.AsNoTracking().ToListAsync();
        int tongMatHang = allProducts.Count;

        var imeiConHangGroup = await _context.IMEIs
            .AsNoTracking()
            .Where(i => i.TrangThai == "con_hang")
            .GroupBy(i => i.MaHang)
            .Select(g => new { MaHang = g.Key, Total = g.Count() })
            .ToDictionaryAsync(x => x.MaHang, x => x.Total);

        var nhapGroup = await _context.ChiTietPhieuNhaps
            .GroupBy(c => c.MaHang)
            .Select(g => new { MaHang = g.Key, Total = g.Sum(x => x.SlNhap) })
            .ToDictionaryAsync(x => x.MaHang, x => x.Total);

        var banGroup = await _context.ChiTietHoaDons
            .GroupBy(c => c.MaHang)
            .Select(g => new { MaHang = g.Key, Total = g.Sum(x => x.SlBan) })
            .ToDictionaryAsync(x => x.MaHang, x => x.Total);

        int tongTonKho = 0;
        int soMatHangSapHet = 0;

        foreach (var p in allProducts)
        {
            int stock;
            if (p.QuanLyImei)
            {
                stock = imeiConHangGroup.TryGetValue(p.MaHang, out var count) ? count : 0;
            }
            else
            {
                var nhap = nhapGroup.TryGetValue(p.MaHang, out var n) ? n : 0;
                var ban = banGroup.TryGetValue(p.MaHang, out var b) ? b : 0;
                stock = Math.Max(0, nhap - ban);
            }

            tongTonKho += stock;
            if (stock <= 5)
            {
                soMatHangSapHet++;
            }
        }

        var summary = new DashboardSummaryDto
        {
            DoanhThuHomNay = doanhThuHomNay,
            DoanhThuThang = doanhThuThang,
            TongDoanhThu = tongDoanhThu,
            DoanhThuGop = grossTotal,
            TongTienHoanTra = refundTotal,
            SoDonHoanTra = refunds.Count,
            SoHoaDonHomNay = soHoaDonHomNay,
            TongHoaDon = tongHoaDon,
            SoHoaDonTaiQuay = taiQuayInvoices.Count,
            SoHoaDonOnline = onlineInvoices.Count,
            DoanhThuTaiQuay = doanhThuTaiQuay,
            DoanhThuOnline = doanhThuOnline,
            DonOnlineChoXacNhan = pendingOrders,
            TongMatHang = tongMatHang,
            TongTonKho = tongTonKho,
            SoMatHangSapHet = soMatHangSapHet,
            SoBaoHanhDangXuLy = warrantyProcessing
        };

        return summary;
    }

    public async Task<List<ChannelRevenueDto>> GetRevenueByChannelAsync(DateTime? tuNgay, DateTime? denNgay)
    {
        var hdQuery = _context.HoaDons.AsNoTracking();
        var trQuery = _context.TraHangs.AsNoTracking()
            .Where(t => t.TrangThai == "hoan_tat" && (t.HinhThucHoanTien == "tien_mat" || t.HinhThucHoanTien == "chuyen_khoan"));

        if (tuNgay.HasValue)
        {
            hdQuery = hdQuery.Where(h => h.NgayLap >= tuNgay.Value);
            trQuery = trQuery.Where(t => (t.NgayHoanTat ?? t.NgayTR) >= tuNgay.Value);
        }
        if (denNgay.HasValue)
        {
            hdQuery = hdQuery.Where(h => h.NgayLap <= denNgay.Value);
            trQuery = trQuery.Where(t => (t.NgayHoanTat ?? t.NgayTR) <= denNgay.Value);
        }

        var invoices = await hdQuery
            .Select(h => new
            {
                h.SoHD,
                h.KenhBan,
                ThanhTien = h.ThanhTien > 0 ? h.ThanhTien : (h.TongTien - h.TienGiamGia)
            })
            .ToListAsync();

        var refunds = await trQuery
            .Select(t => new { t.SoHD, SoTienHoan = t.SoTienHoan ?? 0m })
            .ToListAsync();

        var result = new List<ChannelRevenueDto>();
        var channels = new[] { "tai_quay", "online" };
        foreach (var ch in channels)
        {
            var chInvoices = invoices.Where(h => (h.KenhBan ?? "tai_quay") == ch).ToList();
            var chSohds = chInvoices.Select(h => h.SoHD).ToHashSet();
            var chRefunds = refunds.Where(r => r.SoHD != null && chSohds.Contains(r.SoHD)).Sum(r => r.SoTienHoan);
            var gross = chInvoices.Sum(h => h.ThanhTien);
            result.Add(new ChannelRevenueDto
            {
                KenhBan = ch,
                DoanhThu = Math.Max(0, gross - chRefunds),
                SoHoaDon = chInvoices.Count
            });
        }

        return result;
    }

    public async Task<List<CategoryRevenueDto>> GetRevenueByCategoryAsync(DateTime? tuNgay, DateTime? denNgay)
    {
        var hdQuery = _context.HoaDons.AsNoTracking();
        if (tuNgay.HasValue)
        {
            hdQuery = hdQuery.Where(h => h.NgayLap >= tuNgay.Value);
        }
        if (denNgay.HasValue)
        {
            hdQuery = hdQuery.Where(h => h.NgayLap <= denNgay.Value);
        }

        var details = await (
            from hd in hdQuery
            join ct in _context.ChiTietHoaDons on hd.SoHD equals ct.SoHD
            join mh in _context.MatHangs on ct.MaHang equals mh.MaHang
            join nh in _context.NhomHangs on mh.MaNhom equals nh.MaNhom
            select new
            {
                nh.MaNhom,
                nh.TenNhom,
                ct.SlBan,
                ThanhTienItem = ct.SlBan * ct.DonGiaBan
            }
        ).ToListAsync();

        var result = details
            .GroupBy(x => new { x.MaNhom, x.TenNhom })
            .Select(g => new CategoryRevenueDto
            {
                MaNhom = g.Key.MaNhom,
                TenNhom = g.Key.TenNhom,
                DoanhThu = g.Sum(x => x.ThanhTienItem),
                SoLuongBan = g.Sum(x => x.SlBan)
            })
            .OrderByDescending(x => x.DoanhThu)
            .ToList();

        return result;
    }
}
