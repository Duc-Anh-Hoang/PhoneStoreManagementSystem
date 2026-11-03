using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Products.Attributes;

public class AttributeService : IAttributeService
{
    private readonly AppDbContext _context;

    public AttributeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ThuocTinh>> GetAllAttributesAsync()
    {
        return await _context.ThuocTinhs.AsNoTracking().ToListAsync();
    }

    public async Task<ThuocTinh> CreateAttributeAsync(ThuocTinh thuocTinh)
    {
        _context.ThuocTinhs.Add(thuocTinh);
        await _context.SaveChangesAsync();
        return thuocTinh;
    }
}
