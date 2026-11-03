using PhoneStoreAPI.Features.Staff.DTOs;

namespace PhoneStoreAPI.Features.Staff;

public interface ITaiKhoanService
{
    Task<List<TaiKhoanDto>> GetAllAsync();
    Task<TaiKhoanDto> GetByUsernameAsync(string username);
    Task<TaiKhoanDto> CreateAsync(CreateTaiKhoanRequest request);
    Task<TaiKhoanDto> UpdateAsync(string username, UpdateTaiKhoanRequest request, string? currentUsername);
    Task<TaiKhoanDto> UpdateStatusAsync(string username, string trangThai, string? currentUsername);
    Task<List<PhanQuyenDto>> GetAllRolesAsync();
}
