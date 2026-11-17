namespace PhoneStoreAPI.Features.Checkout.DTOs;

public class CreateOrderItemRequest
{
    public long VariantId { get; set; }
    public int Qty { get; set; }
}

public class CreateOrderRequest
{
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientPhone { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = "QR_PAYMENT"; // QR_PAYMENT, COD
    public List<CreateOrderItemRequest> Items { get; set; } = new();
}

public class OrderItemDTO
{
    public long Id { get; set; }
    public long VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public long Price { get; set; }
    public int Qty { get; set; }
    public long Subtotal => Price * Qty;
}

public class OrderDTO
{
    public long Id { get; set; }
    public string OrderCode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public long Total { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientPhone { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDTO> Items { get; set; } = new();
}
