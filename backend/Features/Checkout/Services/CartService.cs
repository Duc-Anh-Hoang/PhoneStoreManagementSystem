using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhoneStoreAPI.Common.Configs;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Checkout.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _context;
    private readonly CheckoutOptions _options;

    public CartService(AppDbContext context, IOptions<CheckoutOptions> options)
    {
        _context = context;
        _options = options.Value;
    }

    private async Task<Cart> GetOrCreateCartEntityAsync(string? cartToken, string? userId)
    {
        Cart? cart = null;

        if (!string.IsNullOrWhiteSpace(userId))
        {
            cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Variant)
                .ThenInclude(v => v!.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartToken = string.IsNullOrWhiteSpace(cartToken) ? Guid.NewGuid().ToString("N") : cartToken,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            return cart;
        }

        if (!string.IsNullOrWhiteSpace(cartToken))
        {
            cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Variant)
                .ThenInclude(v => v!.Product)
                .FirstOrDefaultAsync(c => c.CartToken == cartToken);
        }

        if (cart == null)
        {
            cart = new Cart
            {
                CartToken = string.IsNullOrWhiteSpace(cartToken) ? Guid.NewGuid().ToString("N") : cartToken,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }

    public async Task<CartDTO> GetCartAsync(string? cartToken, string? userId)
    {
        var cart = await GetOrCreateCartEntityAsync(cartToken, userId);
        return await BuildCartDtoAsync(cart);
    }

    public async Task<CartDTO> AddItemAsync(string? cartToken, string? userId, AddCartItemRequest request)
    {
        var cart = await GetOrCreateCartEntityAsync(cartToken, userId);

        var variant = await _context.ProductVariants
            .Include(v => v.Product)
            .FirstOrDefaultAsync(v => v.Id == request.VariantId);

        if (variant == null)
        {
            throw new InvalidOperationException($"Không tìm thấy sản phẩm variant ID {request.VariantId}");
        }

        var item = cart.Items.FirstOrDefault(i => i.VariantId == request.VariantId);
        int targetQty = (item?.Qty ?? 0) + (request.Qty <= 0 ? 1 : request.Qty);

        if (targetQty > _options.MaxQtyPerOrder)
        {
            targetQty = _options.MaxQtyPerOrder;
        }

        if (item == null)
        {
            item = new CartItem
            {
                CartId = cart.Id,
                VariantId = request.VariantId,
                Qty = targetQty,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.CartItems.Add(item);
        }
        else
        {
            item.Qty = targetQty;
            item.UpdatedAt = DateTime.UtcNow;
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return await BuildCartDtoAsync(cart);
    }

    public async Task<CartDTO> UpdateItemAsync(string? cartToken, string? userId, long variantId, int qty)
    {
        var cart = await GetOrCreateCartEntityAsync(cartToken, userId);
        var item = cart.Items.FirstOrDefault(i => i.VariantId == variantId);

        if (item != null)
        {
            if (qty <= 0)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                item.Qty = Math.Min(qty, _options.MaxQtyPerOrder);
                item.UpdatedAt = DateTime.UtcNow;
            }
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        return await BuildCartDtoAsync(cart);
    }

    public async Task<CartDTO> RemoveItemAsync(string? cartToken, string? userId, long variantId)
    {
        var cart = await GetOrCreateCartEntityAsync(cartToken, userId);
        var item = cart.Items.FirstOrDefault(i => i.VariantId == variantId);

        if (item != null)
        {
            _context.CartItems.Remove(item);
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        return await BuildCartDtoAsync(cart);
    }

    public async Task<CartDTO> MergeCartAsync(string guestCartToken, string userId)
    {
        if (string.IsNullOrWhiteSpace(guestCartToken) || string.IsNullOrWhiteSpace(userId))
        {
            return await GetCartAsync(guestCartToken, userId);
        }

        var guestCart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.CartToken == guestCartToken && c.UserId == null);

        var userCart = await GetOrCreateCartEntityAsync(null, userId);

        if (guestCart != null && guestCart.Items.Count > 0)
        {
            foreach (var guestItem in guestCart.Items.ToList())
            {
                var existingUserItem = userCart.Items.FirstOrDefault(i => i.VariantId == guestItem.VariantId);
                if (existingUserItem != null)
                {
                    existingUserItem.Qty = Math.Min(_options.MaxQtyPerOrder, existingUserItem.Qty + guestItem.Qty);
                    existingUserItem.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    var newItem = new CartItem
                    {
                        CartId = userCart.Id,
                        VariantId = guestItem.VariantId,
                        Qty = Math.Min(_options.MaxQtyPerOrder, guestItem.Qty),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.CartItems.Add(newItem);
                    userCart.Items.Add(newItem);
                }
            }

            _context.CartItems.RemoveRange(guestCart.Items);
            _context.Carts.Remove(guestCart);
            userCart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        return await BuildCartDtoAsync(userCart);
    }

    private async Task<CartDTO> BuildCartDtoAsync(Cart cart)
    {
        // Reload items with latest variant and product data to guarantee server is source of truth
        var items = await _context.CartItems
            .Include(i => i.Variant)
            .ThenInclude(v => v!.Product)
            .Where(i => i.CartId == cart.Id)
            .ToListAsync();

        var dto = new CartDTO
        {
            Id = cart.Id,
            CartToken = cart.CartToken,
            UserId = cart.UserId
        };

        foreach (var item in items)
        {
            var variant = item.Variant;
            if (variant == null)
            {
                continue;
            }

            var itemDto = new CartItemDTO
            {
                Id = item.Id,
                VariantId = item.VariantId,
                ProductName = variant.Product?.Name ?? "Sản phẩm",
                Sku = variant.Sku,
                Color = variant.Color,
                Storage = variant.Storage,
                Price = variant.Price, // Server is source of truth
                Qty = item.Qty,
                AvailableStock = variant.Stock
            };

            dto.Items.Add(itemDto);

            // Warnings calculation
            if (variant.Stock == 0)
            {
                dto.Warnings.Add(new CartWarningDTO
                {
                    VariantId = item.VariantId,
                    Code = "OUT_OF_STOCK",
                    Message = $"Sản phẩm {itemDto.ProductName} ({variant.Color} - {variant.Storage}) hiện đã hết hàng."
                });
            }
            else if (variant.Stock < item.Qty)
            {
                dto.Warnings.Add(new CartWarningDTO
                {
                    VariantId = item.VariantId,
                    Code = "INSUFFICIENT_STOCK",
                    Message = $"Sản phẩm {itemDto.ProductName} chỉ còn {variant.Stock} chiếc trong kho (bạn đang chọn {item.Qty})."
                });
            }

            if (item.Qty > _options.MaxQtyPerOrder)
            {
                dto.Warnings.Add(new CartWarningDTO
                {
                    VariantId = item.VariantId,
                    Code = "MAX_QTY_EXCEEDED",
                    Message = $"Sản phẩm {itemDto.ProductName} chỉ được mua tối đa {_options.MaxQtyPerOrder} chiếc mỗi đơn hàng."
                });
            }
        }

        return dto;
    }
}
