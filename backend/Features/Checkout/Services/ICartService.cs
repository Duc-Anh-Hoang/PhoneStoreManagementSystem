using PhoneStoreAPI.Features.Checkout.DTOs;

namespace PhoneStoreAPI.Features.Checkout.Services;

public interface ICartService
{
    Task<CartDTO> GetCartAsync(string? cartToken, string? userId);
    Task<CartDTO> AddItemAsync(string? cartToken, string? userId, AddCartItemRequest request);
    Task<CartDTO> UpdateItemAsync(string? cartToken, string? userId, long variantId, int qty);
    Task<CartDTO> RemoveItemAsync(string? cartToken, string? userId, long variantId);
    Task<CartDTO> MergeCartAsync(string guestCartToken, string userId);
}
