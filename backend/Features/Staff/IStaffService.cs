using PhoneStoreAPI.Features.Staff.DTOs;

namespace PhoneStoreAPI.Features.Staff;

public interface IStaffService
{
    Task<(List<NhanVienDto> items, int totalCount)> GetAllAsync(string? keyword, string? chucVu, string? trangThai, int page, int pageSize);
    Task<NhanVienDto> GetByIdAsync(string maNV);
    Task<NhanVienDto> CreateAsync(CreateNhanVienRequest request);
    Task<NhanVienDto> UpdateAsync(string maNV, UpdateNhanVienRequest request);
    Task<(bool isDeleted, string message)> DeleteAsync(string maNV);
}
