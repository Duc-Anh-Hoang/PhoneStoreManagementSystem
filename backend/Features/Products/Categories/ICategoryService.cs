using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Products.Categories;

public interface ICategoryService
{
    Task<List<NhomHang>> GetAllNhomHangAsync();
    Task<List<LoaiHang>> GetAllLoaiHangAsync();
    Task<NhomHang> CreateNhomHangAsync(NhomHang nhomHang);
    Task<LoaiHang> CreateLoaiHangAsync(LoaiHang loaiHang);
}
