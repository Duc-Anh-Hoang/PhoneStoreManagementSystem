using PhoneStoreAPI.Features.Purchasing.DTOs;

namespace PhoneStoreAPI.Features.Purchasing;

public interface IPurchasingService
{
    Task<PhieuNhapDto> CreateAsync(CreatePhieuNhapRequest request);
    Task<(List<PhieuNhapDto> Items, int TotalCount)> GetAllAsync(DateTime? tuNgay = null, DateTime? denNgay = null, string? maNCC = null, int? page = null, int? pageSize = null);
    Task<PhieuNhapDto> GetBySoAsync(string soPN);
}
