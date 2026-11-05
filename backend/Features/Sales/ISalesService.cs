using PhoneStoreAPI.Features.Sales.DTOs;

namespace PhoneStoreAPI.Features.Sales;

public interface ISalesService
{
    Task<HoaDonDto> CreateHoaDonAsync(CreateHoaDonRequest request);
    Task<(List<HoaDonDto> Items, int Total)> GetHoaDonsAsync(DateOnly? tuNgay, DateOnly? denNgay, string? kenhBan, string? maKH, string? keyword, int page, int pageSize);
    Task<HoaDonDto> GetHoaDonChiTietAsync(string soHD);
}
