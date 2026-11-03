using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Products.Categories;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<NhomHang>> GetAllNhomHangAsync()
    {
        return await _context.NhomHangs.AsNoTracking().ToListAsync();
    }

    public async Task<List<LoaiHang>> GetAllLoaiHangAsync()
    {
        return await _context.LoaiHangs.AsNoTracking().ToListAsync();
    }

    public async Task<NhomHang> CreateNhomHangAsync(NhomHang nhomHang)
    {
        _context.NhomHangs.Add(nhomHang);
        await _context.SaveChangesAsync();
        return nhomHang;
    }

    public async Task<LoaiHang> CreateLoaiHangAsync(LoaiHang loaiHang)
    {
        _context.LoaiHangs.Add(loaiHang);
        await _context.SaveChangesAsync();
        return loaiHang;
    }
}
