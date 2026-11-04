using System.Net;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Products.DTOs;
using PhoneStoreAPI.Models.Entities;
using PhoneStoreAPI.Models.Entities.Views;

namespace PhoneStoreAPI.Features.Products;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<ProductDto> Items, int TotalCount)> GetProductsAsync(ProductQueryParameters query)
    {
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        var page = Math.Max(query.Page, 1);

        var baseQuery = _context.MatHangs
            .AsNoTracking()
            .Include(m => m.NhomHang)
            .Include(m => m.LoaiHang)
            .AsQueryable();

        // 1. Lọc theo từ khóa (Mã hàng hoặc Tên hàng)
        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var kw = query.Keyword.Trim().ToLower();
            baseQuery = baseQuery.Where(m => m.MaHang.ToLower().Contains(kw) || m.TenHang.ToLower().Contains(kw));
        }

        // 2. Lọc theo Nhóm hàng
        if (!string.IsNullOrWhiteSpace(query.MaNhom))
        {
            var nhom = query.MaNhom.Trim();
            baseQuery = baseQuery.Where(m => m.MaNhom == nhom);
        }

        // 3. Lọc theo Loại hàng
        if (!string.IsNullOrWhiteSpace(query.MaLoai))
        {
            var loai = query.MaLoai.Trim();
            baseQuery = baseQuery.Where(m => m.MaLoai == loai);
        }

        // 4. Lọc theo Giá
        if (query.MinGia.HasValue)
        {
            baseQuery = baseQuery.Where(m => m.DonGia >= query.MinGia.Value);
        }
        if (query.MaxGia.HasValue)
        {
            baseQuery = baseQuery.Where(m => m.DonGia <= query.MaxGia.Value);
        }

        // 5. Đếm tổng số bản ghi
        var totalCount = await baseQuery.CountAsync();

        // 6. Sắp xếp
        baseQuery = query.Sort?.ToLower() switch
        {
            "giatang" => baseQuery.OrderBy(m => m.DonGia),
            "giagiam" => baseQuery.OrderByDescending(m => m.DonGia),
            "ten" => baseQuery.OrderBy(m => m.TenHang),
            _ => baseQuery.OrderBy(m => m.MaHang)
        };

        // 7. Phân trang
        var pagedMatHang = await baseQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var maHangList = pagedMatHang.Select(m => m.MaHang).ToList();

        // 8. Lấy tồn kho tương ứng từ vw_TonKho (tránh N+1 truy vấn)
        var tonKhoDict = await _context.Set<VwTonKho>()
            .AsNoTracking()
            .Where(v => maHangList.Contains(v.MaHang))
            .ToDictionaryAsync(v => v.MaHang, v => v.TonKho);

        var items = pagedMatHang.Select(m => new ProductDto
        {
            MaHang = m.MaHang,
            TenHang = m.TenHang,
            DonGia = m.DonGia,
            Dvt = m.Dvt,
            MaNhom = m.MaNhom,
            TenNhom = m.NhomHang?.TenNhom,
            MaLoai = m.MaLoai,
            TenLoai = m.LoaiHang?.TenLoai,
            QuanLyImei = m.QuanLyImei,
            ThoiHanBaoHanh = m.ThoiHanBaoHanh,
            TonKho = tonKhoDict.TryGetValue(m.MaHang, out var tk) ? tk : 0
        }).ToList();

        return (items, totalCount);
    }

    public async Task<ProductDetailDto> GetProductByMaAsync(string maHang)
    {
        var m = await _context.MatHangs
            .AsNoTracking()
            .Include(x => x.NhomHang)
            .Include(x => x.LoaiHang)
            .FirstOrDefaultAsync(x => x.MaHang == maHang);

        if (m == null)
        {
            throw new NotFoundException($"Không tìm thấy mặt hàng với mã '{maHang}'");
        }

        var tonKho = await _context.Set<VwTonKho>()
            .AsNoTracking()
            .Where(v => v.MaHang == maHang)
            .Select(v => v.TonKho)
            .FirstOrDefaultAsync();

        var specs = await _context.ChiTietThuocTinhs
            .AsNoTracking()
            .Include(c => c.ThuocTinh)
            .Where(c => c.MaHang == maHang)
            .OrderBy(c => c.MaThuocTinh)
            .Select(c => new ProductSpecDto
            {
                MaThuocTinh = c.MaThuocTinh,
                TenThuocTinh = c.ThuocTinh != null ? c.ThuocTinh.TenThuocTinh : c.MaThuocTinh,
                GiaTri = c.GiaTri
            })
            .ToListAsync();

        return new ProductDetailDto
        {
            MaHang = m.MaHang,
            TenHang = m.TenHang,
            DonGia = m.DonGia,
            Dvt = m.Dvt,
            MaNhom = m.MaNhom,
            TenNhom = m.NhomHang?.TenNhom,
            MaLoai = m.MaLoai,
            TenLoai = m.LoaiHang?.TenLoai,
            QuanLyImei = m.QuanLyImei,
            ThoiHanBaoHanh = m.ThoiHanBaoHanh,
            TonKho = tonKho,
            ThongSo = specs
        };
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductRequest request)
    {
        var maHang = request.MaHang.Trim().ToUpper();

        // 1. Kiểm tra trùng mã mặt hàng (409)
        if (await _context.MatHangs.AnyAsync(m => m.MaHang == maHang))
        {
            throw new ConflictException($"Mã mặt hàng '{maHang}' đã tồn tại trên hệ thống");
        }

        // 2. Kiểm tra Nhóm hàng và Loại hàng phải tồn tại
        var nhom = await _context.NhomHangs.FindAsync(request.MaNhom);
        if (nhom == null)
        {
            throw new AppException($"Nhóm hàng '{request.MaNhom}' không tồn tại", HttpStatusCode.BadRequest);
        }

        var loai = await _context.LoaiHangs.FindAsync(request.MaLoai);
        if (loai == null)
        {
            throw new AppException($"Loại hàng '{request.MaLoai}' không tồn tại", HttpStatusCode.BadRequest);
        }

        var entity = new MatHang
        {
            MaHang = maHang,
            TenHang = request.TenHang.Trim(),
            DonGia = request.DonGia,
            Dvt = string.IsNullOrWhiteSpace(request.Dvt) ? "Cái" : request.Dvt.Trim(),
            MaNhom = request.MaNhom,
            MaLoai = request.MaLoai,
            QuanLyImei = request.QuanLyImei,
            ThoiHanBaoHanh = request.ThoiHanBaoHanh
        };

        _context.MatHangs.Add(entity);
        await _context.SaveChangesAsync();

        return new ProductDto
        {
            MaHang = entity.MaHang,
            TenHang = entity.TenHang,
            DonGia = entity.DonGia,
            Dvt = entity.Dvt,
            MaNhom = entity.MaNhom,
            TenNhom = nhom.TenNhom,
            MaLoai = entity.MaLoai,
            TenLoai = loai.TenLoai,
            QuanLyImei = entity.QuanLyImei,
            ThoiHanBaoHanh = entity.ThoiHanBaoHanh,
            TonKho = 0
        };
    }

    public async Task<ProductDto> UpdateProductAsync(string maHang, UpdateProductRequest request)
    {
        var entity = await _context.MatHangs
            .Include(m => m.NhomHang)
            .Include(m => m.LoaiHang)
            .FirstOrDefaultAsync(m => m.MaHang == maHang);

        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy mặt hàng với mã '{maHang}'");
        }

        // 1. Kiểm tra quy tắc: Không cho đổi quanLyIMEI khi mặt hàng đã có IMEI hoặc đã nhập kho
        if (entity.QuanLyImei != request.QuanLyImei)
        {
            var hasImei = await _context.IMEIs.AnyAsync(i => i.MaHang == maHang);
            var hasImport = await _context.ChiTietPhieuNhaps.AnyAsync(p => p.MaHang == maHang);

            if (hasImei || hasImport)
            {
                throw new ConflictException("Không thể thay đổi hình thức quản lý IMEI khi mặt hàng đã phát sinh IMEI hoặc phiếu nhập kho");
            }
        }

        // 2. Kiểm tra Nhóm hàng và Loại hàng
        var nhom = await _context.NhomHangs.FindAsync(request.MaNhom);
        if (nhom == null)
        {
            throw new AppException($"Nhóm hàng '{request.MaNhom}' không tồn tại", HttpStatusCode.BadRequest);
        }

        var loai = await _context.LoaiHangs.FindAsync(request.MaLoai);
        if (loai == null)
        {
            throw new AppException($"Loại hàng '{request.MaLoai}' không tồn tại", HttpStatusCode.BadRequest);
        }

        entity.TenHang = request.TenHang.Trim();
        entity.DonGia = request.DonGia;
        entity.Dvt = string.IsNullOrWhiteSpace(request.Dvt) ? "Cái" : request.Dvt.Trim();
        entity.MaNhom = request.MaNhom;
        entity.MaLoai = request.MaLoai;
        entity.QuanLyImei = request.QuanLyImei;
        entity.ThoiHanBaoHanh = request.ThoiHanBaoHanh;

        await _context.SaveChangesAsync();

        var tonKho = await _context.Set<VwTonKho>()
            .AsNoTracking()
            .Where(v => v.MaHang == maHang)
            .Select(v => v.TonKho)
            .FirstOrDefaultAsync();

        return new ProductDto
        {
            MaHang = entity.MaHang,
            TenHang = entity.TenHang,
            DonGia = entity.DonGia,
            Dvt = entity.Dvt,
            MaNhom = entity.MaNhom,
            TenNhom = nhom.TenNhom,
            MaLoai = entity.MaLoai,
            TenLoai = loai.TenLoai,
            QuanLyImei = entity.QuanLyImei,
            ThoiHanBaoHanh = entity.ThoiHanBaoHanh,
            TonKho = tonKho
        };
    }

    public async Task DeleteProductAsync(string maHang)
    {
        var entity = await _context.MatHangs.FindAsync(maHang);
        if (entity == null)
        {
            throw new NotFoundException($"Không tìm thấy mặt hàng với mã '{maHang}'");
        }

        // Kiểm tra ràng buộc: DELETE trả 409 nếu đã có trong phiếu nhập, hóa đơn hoặc đơn online
        var inPhieuNhap = await _context.ChiTietPhieuNhaps.AnyAsync(pn => pn.MaHang == maHang);
        if (inPhieuNhap)
        {
            throw new ConflictException("Không thể xóa mặt hàng đã có trong phiếu nhập kho");
        }

        var inHoaDon = await _context.ChiTietHoaDons.AnyAsync(hd => hd.MaHang == maHang);
        if (inHoaDon)
        {
            throw new ConflictException("Không thể xóa mặt hàng đã có trong hóa đơn bán hàng");
        }

        var inDonOnline = await _context.ChiTietDonHangOnlines.AnyAsync(dh => dh.MaHang == maHang);
        if (inDonOnline)
        {
            throw new ConflictException("Không thể xóa mặt hàng đã có trong đơn hàng trực tuyến");
        }

        var inImei = await _context.IMEIs.AnyAsync(i => i.MaHang == maHang);
        if (inImei)
        {
            throw new ConflictException("Không thể xóa mặt hàng đang có dữ liệu IMEI");
        }

        // Xóa thông số kỹ thuật (ChiTietThuocTinh)
        var specs = await _context.ChiTietThuocTinhs.Where(c => c.MaHang == maHang).ToListAsync();
        if (specs.Any())
        {
            _context.ChiTietThuocTinhs.RemoveRange(specs);
        }

        _context.MatHangs.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProductSpecDto>> GetProductSpecsAsync(string maHang)
    {
        if (!await _context.MatHangs.AnyAsync(m => m.MaHang == maHang))
        {
            throw new NotFoundException($"Không tìm thấy mặt hàng với mã '{maHang}'");
        }

        return await _context.ChiTietThuocTinhs
            .AsNoTracking()
            .Include(c => c.ThuocTinh)
            .Where(c => c.MaHang == maHang)
            .OrderBy(c => c.MaThuocTinh)
            .Select(c => new ProductSpecDto
            {
                MaThuocTinh = c.MaThuocTinh,
                TenThuocTinh = c.ThuocTinh != null ? c.ThuocTinh.TenThuocTinh : c.MaThuocTinh,
                GiaTri = c.GiaTri
            })
            .ToListAsync();
    }

    public async Task<List<ProductSpecDto>> UpdateProductSpecsAsync(string maHang, List<UpdateProductSpecRequest> specs)
    {
        if (!await _context.MatHangs.AnyAsync(m => m.MaHang == maHang))
        {
            throw new NotFoundException($"Không tìm thấy mặt hàng với mã '{maHang}'");
        }

        // Kiểm tra tất cả thuộc tính phải tồn tại trên hệ thống
        var distinctMaTT = specs.Select(s => s.MaThuocTinh.Trim()).Distinct().ToList();
        var existingTT = await _context.ThuocTinhs
            .Where(t => distinctMaTT.Contains(t.MaThuocTinh))
            .ToDictionaryAsync(t => t.MaThuocTinh, t => t.TenThuocTinh);

        foreach (var maTT in distinctMaTT)
        {
            if (!existingTT.ContainsKey(maTT))
            {
                throw new AppException($"Thuộc tính '{maTT}' không tồn tại trên hệ thống", HttpStatusCode.BadRequest);
            }
        }

        // Chạy trong một Transaction
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Xóa toàn bộ thuộc tính cũ của mặt hàng
            var oldSpecs = await _context.ChiTietThuocTinhs.Where(c => c.MaHang == maHang).ToListAsync();
            _context.ChiTietThuocTinhs.RemoveRange(oldSpecs);
            await _context.SaveChangesAsync();

            // Thêm các thuộc tính mới
            var newEntities = specs
                .Where(s => !string.IsNullOrWhiteSpace(s.GiaTri))
                .Select(s => new ChiTietThuocTinh
                {
                    MaHang = maHang,
                    MaThuocTinh = s.MaThuocTinh.Trim(),
                    GiaTri = s.GiaTri.Trim()
                }).ToList();

            if (newEntities.Any())
            {
                _context.ChiTietThuocTinhs.AddRange(newEntities);
                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            return newEntities.Select(e => new ProductSpecDto
            {
                MaThuocTinh = e.MaThuocTinh,
                TenThuocTinh = existingTT[e.MaThuocTinh],
                GiaTri = e.GiaTri
            }).ToList();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
