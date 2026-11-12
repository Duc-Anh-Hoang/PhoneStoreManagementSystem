using PhoneStoreAPI.Features.OnlineOrders.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.OnlineOrders;

public interface IShippingService
{
    Task<OrderShippingDto> CreateShipmentAsync(DonHangOnline order, IReadOnlyList<ChiTietDonHangOnline> items);
    Task<OrderShippingDto?> GetTrackingAsync(string maDH);
    Task<OrderShippingDto?> AdvanceMockShipmentAsync(string maDH);
    Task<OrderShippingDto> SetSpxTrackingAsync(string maDH, string trackingCode);
    Task<bool> HandleGhnWebhookAsync(GhnWebhookPayload payload, string? webhookSecretHeader);
}
