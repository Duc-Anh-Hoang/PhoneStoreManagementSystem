using PhoneStoreAPI.Features.OnlineOrders.DTOs;

namespace PhoneStoreAPI.Features.OnlineOrders;

public interface IOnlineOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderRequest request);
    Task<OrderDto?> GetOrderByMaDHAsync(string maDH, string? sdt);
    Task<List<OrderDto>> GetOrdersByPhoneAsync(string sdt);
    Task<(List<OrderDto> Items, int Total)> GetAdminOrdersAsync(string? trangThai, DateOnly? tuNgay, DateOnly? denNgay, int page, int pageSize);
    Task<OrderDto> UpdateOrderStatusAsync(string maDH, string newTrangThai, string? soHDKem = null);
    Task<OrderDto> CancelOrderByCustomerAsync(string maDH, CancelOrderRequest request);
}
