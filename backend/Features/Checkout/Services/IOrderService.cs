using PhoneStoreAPI.Features.Checkout.DTOs;

namespace PhoneStoreAPI.Features.Checkout.Services;

public interface IOrderService
{
    Task<OrderDTO> CreateOrderAsync(string idempotencyKey, CreateOrderRequest request, string? userId = null, CancellationToken cancellationToken = default);
    Task<OrderDTO?> GetOrderByCodeAsync(string orderCode, CancellationToken cancellationToken = default);
    Task<OrderDTO?> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken = default);
}
