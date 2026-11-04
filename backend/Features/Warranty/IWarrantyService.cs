namespace PhoneStoreAPI.Features.Warranty;

using PhoneStoreAPI.Features.Warranty.DTOs;

public interface IWarrantyService
{
    Task CreateForInvoiceAsync(string soHD, string maIMEI, string maHang);
    Task<WarrantyLookupDto> GetByImeiAsync(string imei);
    Task<WarrantyStatusDto> GetStatusByImeiAsync(string imei);
}
