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

    // Giai đoạn 2: Tra cứu công khai (thông tin đã được che)
    Task<OrderLookupDto?> GetOrderForLookupAsync(string maDH, string sdt);

    // Giai đoạn 4: Tra cứu danh sách đơn theo SĐT với mask (dùng sau khi xác thực OTP token)
    Task<List<OrderLookupDto>> GetOrdersByPhoneMaskedAsync(string phone);

    // Auto-approval & Bulk approval cho đợt cao điểm
    Task<BulkApproveResultDto> BulkApproveOrdersAsync(List<string>? orderCodes = null);
}
