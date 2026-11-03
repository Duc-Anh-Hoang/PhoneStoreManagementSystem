using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Promotions;

public class PromotionService : IPromotionService
{
    private readonly AppDbContext _context;

    public PromotionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<KhuyenMai>> GetAllPromotionsAsync()
    {
        return await _context.KhuyenMais.AsNoTracking().ToListAsync();
    }

    public async Task<List<KhuyenMai>> GetActivePromotionsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.KhuyenMais
            .Where(k => !k.TamDung && k.NgayBD <= now && k.NgayKT >= now)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<KhuyenMai?> GetPromotionByIdAsync(string id)
    {
        return await _context.KhuyenMais.FindAsync(id);
    }

    public async Task<KhuyenMai> CreatePromotionAsync(KhuyenMai km)
    {
        _context.KhuyenMais.Add(km);
        await _context.SaveChangesAsync();
        return km;
    }

    public async Task<bool> UpdateStatusAsync(string id, bool tamDung)
    {
        var existing = await _context.KhuyenMais.FindAsync(id);
        if (existing == null) return false;

        existing.TamDung = tamDung;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePromotionAsync(string id)
    {
        var existing = await _context.KhuyenMais.FindAsync(id);
        if (existing == null) return false;

        _context.KhuyenMais.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
