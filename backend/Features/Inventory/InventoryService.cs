using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Inventory.DTOs;
using PhoneStoreAPI.Models.Entities.Views;

namespace PhoneStoreAPI.Features.Inventory;

public class InventoryService : IInventoryService
{
    private readonly AppDbContext _context;

    public InventoryService(AppDbContext context)
    {
        _context = context;
    }

    #region TỒN KHO

    public async Task<(List<TonKhoDto> Items, int TotalCount)> GetTonKhoAsync(TonKhoQueryParameters query)
    {
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        var page = Math.Max(query.Page, 1);

        var baseQuery = _context.Set<VwTonKho>()
            .AsNoTracking()
            .AsQueryable();

        // 1. Lọc từ khóa
        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var kw = query.Keyword.Trim().ToLower();
            baseQuery = baseQuery.Where(v => v.MaHang.ToLower().Contains(kw) || v.TenHang.ToLower().Contains(kw));
        }

        // 2. Lọc nhóm hàng
        if (!string.IsNullOrWhiteSpace(query.MaNhom))
        {
            var nhom = query.MaNhom.Trim();
            baseQuery = baseQuery.Where(v => v.MaNhom == nhom);
        }

        // 3. Lọc loại hàng
        if (!string.IsNullOrWhiteSpace(query.MaLoai))
        {
            var loai = query.MaLoai.Trim();
            baseQuery = baseQuery.Where(v => v.MaLoai == loai);
        }

        // 4. Lọc cảnh báo sắp hết hàng (tồn <= 5)
        if (query.ChiCanhBao == true)
        {
            baseQuery = baseQuery.Where(v => v.TonKho <= AppConstants.InventoryConstants.CanhBaoTonToiThieu);
        }

        var totalCount = await baseQuery.CountAsync();

        var list = await baseQuery
            .OrderBy(v => v.MaHang)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = list.Select(v => new TonKhoDto
        {
            MaHang = v.MaHang,
            TenHang = v.TenHang,
            MaNhom = v.MaNhom,
            MaLoai = v.MaLoai,
            DonGia = v.DonGia,
            TongNhap = v.TongNhap,
            TongBan = v.TongBan,
            TonKho = v.TonKho,
            CanhBao = v.TonKho <= AppConstants.InventoryConstants.CanhBaoTonToiThieu
        }).ToList();

        return (items, totalCount);
    }

    public async Task<TonKhoDto> GetTonKhoByMaHangAsync(string maHang)
    {
        var v = await _context.Set<VwTonKho>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MaHang == maHang);

        if (v == null)
        {
            // Kiểm tra mặt hàng có tồn tại không
            var m = await _context.MatHangs.FindAsync(maHang);
            if (m == null)
            {
                throw new NotFoundException($"Không tìm thấy mặt hàng với mã '{maHang}'");
            }

            return new TonKhoDto
            {
                MaHang = m.MaHang,
                TenHang = m.TenHang,
                MaNhom = m.MaNhom,
                MaLoai = m.MaLoai,
                DonGia = m.DonGia,
                TongNhap = 0,
                TongBan = 0,
                TonKho = 0,
                CanhBao = true
            };
        }

        return new TonKhoDto
        {
            MaHang = v.MaHang,
            TenHang = v.TenHang,
            MaNhom = v.MaNhom,
            MaLoai = v.MaLoai,
            DonGia = v.DonGia,
            TongNhap = v.TongNhap,
            TongBan = v.TongBan,
            TonKho = v.TonKho,
            CanhBao = v.TonKho <= AppConstants.InventoryConstants.CanhBaoTonToiThieu
        };
    }

    #endregion

    #region IMEI

    public async Task<(List<ImeiDto> Items, int TotalCount)> GetImeisAsync(ImeiQueryParameters query)
    {
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        var page = Math.Max(query.Page, 1);

        var baseQuery = _context.IMEIs
            .AsNoTracking()
            .Include(i => i.MatHang)
            .AsQueryable();

        // 1. Lọc theo mã hàng
        if (!string.IsNullOrWhiteSpace(query.MaHang))
        {
            var mh = query.MaHang.Trim();
            baseQuery = baseQuery.Where(i => i.MaHang == mh);
        }

        // 2. Lọc theo trạng thái (con_hang, da_ban, tra_hang)
        if (!string.IsNullOrWhiteSpace(query.TrangThai))
        {
            var tt = query.TrangThai.Trim().ToLower();
            baseQuery = baseQuery.Where(i => i.TrangThai.ToLower() == tt);
        }

        // 3. Lọc từ khóa
        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var kw = query.Keyword.Trim().ToLower();
            baseQuery = baseQuery.Where(i => 
                i.SoIMEI.ToLower().Contains(kw) || 
                i.MaIMEI.ToLower().Contains(kw) || 
                (i.MatHang != null && i.MatHang.TenHang.ToLower().Contains(kw)));
        }

        var totalCount = await baseQuery.CountAsync();

        // Lấy danh sách phiếu nhập tương ứng để lấy NgayNhap
        var pagedImeis = await baseQuery
            .OrderBy(i => i.MaHang)
            .ThenBy(i => i.SoIMEI)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var soPNList = pagedImeis.Select(i => i.SoPN).Distinct().ToList();
        var pnDates = await _context.PhieuNhaps
            .AsNoTracking()
            .Where(pn => soPNList.Contains(pn.SoPN))
            .ToDictionaryAsync(pn => pn.SoPN, pn => (DateTime?)pn.NgayNhap);

        var items = pagedImeis.Select(i => new ImeiDto
        {
            MaIMEI = i.MaIMEI,
            SoIMEI = i.SoIMEI,
            MaHang = i.MaHang,
            TenHang = i.MatHang?.TenHang,
            SoPN = i.SoPN,
            NgayNhap = pnDates.TryGetValue(i.SoPN, out var dt) ? dt : null,
            TrangThai = i.TrangThai
        }).ToList();

        return (items, totalCount);
    }

    public async Task<List<ImeiDto>> GetAvailableImeisAsync(string maHang)
    {
        // P2-08: Chỉ IMEI con_hang, sắp theo ngày nhập tăng dần (xuất trước nhập trước - FIFO)
        var query = from i in _context.IMEIs.AsNoTracking()
                    join pn in _context.PhieuNhaps.AsNoTracking() on i.SoPN equals pn.SoPN into pnGroup
                    from pn in pnGroup.DefaultIfEmpty()
                    join m in _context.MatHangs.AsNoTracking() on i.MaHang equals m.MaHang into mGroup
                    from m in mGroup.DefaultIfEmpty()
                    where i.MaHang == maHang && i.TrangThai == AppConstants.ImeiStatus.ConHang
                    orderby pn.NgayNhap ascending, i.SoIMEI ascending
                    select new ImeiDto
                    {
                        MaIMEI = i.MaIMEI,
                        SoIMEI = i.SoIMEI,
                        MaHang = i.MaHang,
                        TenHang = m.TenHang,
                        SoPN = i.SoPN,
                        NgayNhap = pn != null ? pn.NgayNhap : null,
                        TrangThai = i.TrangThai
                    };

        return await query.ToListAsync();
    }

    #endregion
}
