namespace PhoneStoreAPI.Features.Checkout.DTOs;

public class AddCartItemRequest
{
    public long VariantId { get; set; }
    public int Qty { get; set; } = 1;
}

public class UpdateCartItemRequest
{
    public int Qty { get; set; }
}

public class MergeCartRequest
{
    public string GuestCartToken { get; set; } = string.Empty;
}

public class CartWarningDTO
{
    public long VariantId { get; set; }
    public string Code { get; set; } = string.Empty; // OUT_OF_STOCK, INSUFFICIENT_STOCK, PRICE_CHANGED, MAX_QTY_EXCEEDED
    public string Message { get; set; } = string.Empty;
}

public class CartItemDTO
{
    public long Id { get; set; }
    public long VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Storage { get; set; } = string.Empty;
    public long Price { get; set; } // VND integer from database
    public int Qty { get; set; }
    public long Subtotal => Price * Qty;
    public int AvailableStock { get; set; }
}

public class CartDTO
{
    public long Id { get; set; }
    public string? CartToken { get; set; }
    public string? UserId { get; set; }
    public List<CartItemDTO> Items { get; set; } = new();
    public long Total => Items.Sum(i => i.Subtotal);
    public List<CartWarningDTO> Warnings { get; set; } = new();
}
