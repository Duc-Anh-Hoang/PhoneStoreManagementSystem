using PhoneStoreAPI.Features.Inventory.DTOs;

namespace PhoneStoreAPI.Features.Inventory;

public interface IInventoryService
{
    // Tồn kho
    Task<(List<TonKhoDto> Items, int TotalCount)> GetTonKhoAsync(TonKhoQueryParameters query);
    Task<TonKhoDto> GetTonKhoByMaHangAsync(string maHang);

    // IMEI
    Task<(List<ImeiDto> Items, int TotalCount)> GetImeisAsync(ImeiQueryParameters query);
    Task<List<ImeiDto>> GetAvailableImeisAsync(string maHang);
}
