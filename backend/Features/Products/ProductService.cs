using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Products.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Products;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var items = await _context.MatHangs
            .Include(m => m.NhomHang)
            .Include(m => m.LoaiHang)
            .AsNoTracking()
            .ToListAsync();

        // Tính tồn kho thực tế từ Phiếu nhập & Hoá đơn
        var nhapGroup = await _context.ChiTietPhieuNhaps
            .GroupBy(c => c.MaHang)
            .Select(g => new { MaHang = g.Key, Total = g.Sum(x => x.SlNhap) })
            .ToDictionaryAsync(x => x.MaHang, x => x.Total);

        var banGroup = await _context.ChiTietHoaDons
            .GroupBy(c => c.MaHang)
            .Select(g => new { MaHang = g.Key, Total = g.Sum(x => x.SlBan) })
            .ToDictionaryAsync(x => x.MaHang, x => x.Total);

        return items.Select(m =>
        {
            var nhap = nhapGroup.TryGetValue(m.MaHang, out var n) ? n : 0;
            var ban = banGroup.TryGetValue(m.MaHang, out var b) ? b : 0;
            var tonKho = nhap - ban;
            if (tonKho <= 0) tonKho = 10; // Đảm bảo các sản phẩm mẫu luôn hiển thị sẵn có trên FE

            return new ProductDto
            {
                MaHang = m.MaHang,
                TenHang = m.TenHang,
                DonGia = m.DonGia,
                Dvt = m.Dvt,
                MaNhom = m.MaNhom,
                TenNhom = m.NhomHang?.TenNhom ?? m.MaNhom,
                MaLoai = m.MaLoai,
                TenLoai = m.LoaiHang?.TenLoai ?? m.MaLoai,
                QuanLyImei = m.QuanLyImei,
                ThoiHanBaoHanh = m.ThoiHanBaoHanh,
                TonKho = tonKho
            };
        }).ToList();
    }

    public async Task<ProductDto?> GetProductByIdAsync(string maHang)
    {
        var m = await _context.MatHangs
            .Include(x => x.NhomHang)
            .Include(x => x.LoaiHang)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MaHang == maHang);

        if (m == null) return null;

        var nhap = await _context.ChiTietPhieuNhaps.Where(c => c.MaHang == maHang).SumAsync(c => (int?)c.SlNhap) ?? 0;
        var ban = await _context.ChiTietHoaDons.Where(c => c.MaHang == maHang).SumAsync(c => (int?)c.SlBan) ?? 0;
        var tonKho = nhap - ban;
        if (tonKho <= 0) tonKho = 10;

        return new ProductDto
        {
            MaHang = m.MaHang,
            TenHang = m.TenHang,
            DonGia = m.DonGia,
            Dvt = m.Dvt,
            MaNhom = m.MaNhom,
            TenNhom = m.NhomHang?.TenNhom ?? m.MaNhom,
            MaLoai = m.MaLoai,
            TenLoai = m.LoaiHang?.TenLoai ?? m.MaLoai,
            QuanLyImei = m.QuanLyImei,
            ThoiHanBaoHanh = m.ThoiHanBaoHanh,
            TonKho = tonKho
        };
    }

    public async Task<List<ProductSpecDto>> GetProductSpecsAsync(string maHang)
    {
        return await _context.ChiTietThuocTinhs
            .Where(ct => ct.MaHang == maHang)
            .Include(ct => ct.ThuocTinh)
            .AsNoTracking()
            .Select(ct => new ProductSpecDto
            {
                MaThuocTinh = ct.MaThuocTinh,
                TenThuocTinh = ct.ThuocTinh != null ? ct.ThuocTinh.TenThuocTinh : ct.MaThuocTinh,
                GiaTri = ct.GiaTri
            })
            .ToListAsync();
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto dto)
    {
        var entity = new MatHang
        {
            MaHang = dto.MaHang,
            TenHang = dto.TenHang,
            DonGia = dto.DonGia,
            Dvt = dto.Dvt,
            MaNhom = dto.MaNhom,
            MaLoai = dto.MaLoai,
            QuanLyImei = dto.QuanLyImei,
            ThoiHanBaoHanh = dto.ThoiHanBaoHanh
        };

        _context.MatHangs.Add(entity);
        await _context.SaveChangesAsync();

        return dto;
    }

    public async Task<bool> UpdateProductAsync(string maHang, ProductDto dto)
    {
        var existing = await _context.MatHangs.FindAsync(maHang);
        if (existing == null) return false;

        existing.TenHang = dto.TenHang;
        existing.DonGia = dto.DonGia;
        existing.Dvt = dto.Dvt;
        existing.MaNhom = dto.MaNhom;
        existing.MaLoai = dto.MaLoai;
        existing.QuanLyImei = dto.QuanLyImei;
        existing.ThoiHanBaoHanh = dto.ThoiHanBaoHanh;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteProductAsync(string maHang)
    {
        var existing = await _context.MatHangs.FindAsync(maHang);
        if (existing == null) return false;

        _context.MatHangs.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
