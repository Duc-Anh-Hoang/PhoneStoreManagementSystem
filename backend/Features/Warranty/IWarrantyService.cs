namespace PhoneStoreAPI.Features.Warranty;

using PhoneStoreAPI.Features.Warranty.DTOs;

public interface IWarrantyService
{
    Task CreateForInvoiceAsync(string soHD, string maIMEI, string maHang);
    Task<WarrantyLookupDto> GetByImeiAsync(string imei);
    Task<WarrantyStatusDto> GetStatusByImeiAsync(string imei);

    Task<(List<TiepNhanBaoHanhDto> Items, int TotalCount)> GetTiepNhanListAsync(
        string? trangThai,
        DateTime? tuNgay,
        DateTime? denNgay,
        string? maNVKyThuat,
        string? keyword,
        int page,
        int pageSize);

    Task<TiepNhanBaoHanhDto> GetTiepNhanByIdAsync(string maTN);
    Task<TiepNhanBaoHanhDto> CreateTiepNhanAsync(CreateTiepNhanRequest request);
    Task<TiepNhanBaoHanhDto> UpdateTiepNhanAsync(string maTN, UpdateTiepNhanRequest request);
    Task<TiepNhanBaoHanhDto> UpdateTrangThaiAsync(string maTN, UpdateTrangThaiRequest request);
}
