using PhoneStoreAPI.Features.Products.Categories.DTOs;

namespace PhoneStoreAPI.Features.Products.Categories;

public interface ICategoryService
{
    // Nhóm hàng
    Task<List<NhomHangDto>> GetAllNhomHangAsync();
    Task<NhomHangDto> GetNhomHangByMaAsync(string maNhom);
    Task<NhomHangDto> CreateNhomHangAsync(CreateNhomHangRequest request);
    Task<NhomHangDto> UpdateNhomHangAsync(string maNhom, UpdateNhomHangRequest request);
    Task DeleteNhomHangAsync(string maNhom);

    // Loại hàng
    Task<List<LoaiHangDto>> GetAllLoaiHangAsync();
    Task<LoaiHangDto> GetLoaiHangByMaAsync(string maLoai);
    Task<LoaiHangDto> CreateLoaiHangAsync(CreateLoaiHangRequest request);
    Task<LoaiHangDto> UpdateLoaiHangAsync(string maLoai, UpdateLoaiHangRequest request);
    Task DeleteLoaiHangAsync(string maLoai);
}
