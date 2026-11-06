using PhoneStoreAPI.Features.Sales.Returns.DTOs;

namespace PhoneStoreAPI.Features.Sales.Returns;

public interface IReturnsService
{
    Task<TraHangDto> CreateTraHangAsync(CreateTraHangRequest request);
    Task<TraHangDto> KiemTraAsync(string maTR, KiemTraTraHangRequest request);
    Task<TraHangDto> DuyetAsync(string maTR, DuyetTraHangRequest request);
    Task<TraHangDto> HoanTatAsync(string maTR, HoanTatTraHangRequest request);
    Task<TraHangDto> HuyAsync(string maTR, HuyTraHangRequest request);
    Task<TraHangDto> DoiHinhThucAsync(string maTR, DoiHinhThucRequest request);
    Task<TraHangDto> GetByIdAsync(string maTR);
    Task<TraCuuImeiTraHangDto> TraCuuImeiAsync(string soIMEI);
    Task<(List<TraHangDto> Items, int Total)> GetTraHangsAsync(
        string? trangThai,
        string? loaiYeuCau,
        string? hinhThuc,
        DateOnly? tuNgay,
        DateOnly? denNgay,
        string? keyword,
        int page,
        int pageSize);
}
