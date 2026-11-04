using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Dashboard.DTOs;

namespace PhoneStoreAPI.Features.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(30);
    private const string SummaryCacheKey = "dashboard_summary";

    public DashboardService(AppDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        if (_cache.TryGetValue(SummaryCacheKey, out DashboardSummaryDto? cached) && cached != null)
        {
            return cached;
        }

        var now = DateTime.UtcNow;
        var startOfToday = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var invoices = await _context.HoaDons
            .AsNoTracking()
            .Select(h => new { h.NgayLap, h.ThanhTien })
            .ToListAsync();

        var todayInvoices = invoices.Where(h => h.NgayLap >= startOfToday).ToList();
        var monthInvoices = invoices.Where(h => h.NgayLap >= startOfMonth).ToList();

        decimal doanhThuHomNay = todayInvoices.Sum(h => h.ThanhTien);
        decimal doanhThuThang = monthInvoices.Sum(h => h.ThanhTien);
        decimal tongDoanhThu = invoices.Sum(h => h.ThanhTien);
        int soHoaDonHomNay = todayInvoices.Count;
        int tongHoaDon = invoices.Count;

        int pendingOrders = await _context.DonHangOnlines
            .AsNoTracking()
            .CountAsync(d => d.TrangThai == "cho_xac_nhan");

        int warrantyProcessing = await _context.PhieuTiepNhanBaoHanhs
            .AsNoTracking()
            .CountAsync(t => t.TrangThai == "tiep_nhan" || t.TrangThai == "dang_sua" || t.TrangThai == "cho_linh_kien");

        var allProducts = await _context.MatHangs.AsNoTracking().ToListAsync();
        int tongMatHang = allProducts.Count;

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
            var nhap = nhapGroup.TryGetValue(p.MaHang, out var n) ? n : 0;
            var ban = banGroup.TryGetValue(p.MaHang, out var b) ? b : 0;
            var stock = nhap - ban;
            if (stock <= 0) stock = 10;

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
            SoHoaDonHomNay = soHoaDonHomNay,
            TongHoaDon = tongHoaDon,
            DonOnlineChoXacNhan = pendingOrders,
            TongMatHang = tongMatHang,
            TongTonKho = tongTonKho,
            SoMatHangSapHet = soMatHangSapHet,
            SoBaoHanhDangXuLy = warrantyProcessing
        };

        _cache.Set(SummaryCacheKey, summary, CacheDuration);
        return summary;
    }

    public async Task<List<ChannelRevenueDto>> GetRevenueByChannelAsync(DateTime? tuNgay, DateTime? denNgay)
    {
        string cacheKey = $"revenue_channel_{tuNgay:yyyyMMdd}_{denNgay:yyyyMMdd}";
        if (_cache.TryGetValue(cacheKey, out List<ChannelRevenueDto>? cached) && cached != null)
        {
            return cached;
        }

        var query = _context.HoaDons.AsNoTracking();

        if (tuNgay.HasValue)
        {
            query = query.Where(h => h.NgayLap >= tuNgay.Value);
        }
        if (denNgay.HasValue)
        {
            query = query.Where(h => h.NgayLap <= denNgay.Value);
        }

        var result = await query
            .GroupBy(h => h.KenhBan)
            .Select(g => new ChannelRevenueDto
            {
                KenhBan = g.Key,
                DoanhThu = g.Sum(x => x.ThanhTien),
                SoHoaDon = g.Count()
            })
            .ToListAsync();

        _cache.Set(cacheKey, result, CacheDuration);
        return result;
    }

    public async Task<List<CategoryRevenueDto>> GetRevenueByCategoryAsync(DateTime? tuNgay, DateTime? denNgay)
    {
        string cacheKey = $"revenue_category_{tuNgay:yyyyMMdd}_{denNgay:yyyyMMdd}";
        if (_cache.TryGetValue(cacheKey, out List<CategoryRevenueDto>? cached) && cached != null)
        {
            return cached;
        }

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

        _cache.Set(cacheKey, result, CacheDuration);
        return result;
    }
}
