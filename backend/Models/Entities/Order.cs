using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStoreAPI.Models.Entities;

[Table("orders")]
public class Order
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("order_code")]
    [MaxLength(32)]
    public string OrderCode { get; set; } = string.Empty;

    [Column("status")]
    [MaxLength(32)]
    public string Status { get; set; } = "PENDING_PAYMENT";

    [Column("total")]
    public long Total { get; set; }

    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [Column("idempotency_key")]
    [MaxLength(128)]
    public string IdempotencyKey { get; set; } = string.Empty;

    [Column("recipient_name")]
    [MaxLength(100)]
    public string RecipientName { get; set; } = string.Empty;

    [Column("recipient_phone")]
    [MaxLength(20)]
    public string RecipientPhone { get; set; } = string.Empty;

    [Column("shipping_address")]
    [MaxLength(255)]
    public string ShippingAddress { get; set; } = string.Empty;

    [Column("payment_method")]
    [MaxLength(30)]
    public string PaymentMethod { get; set; } = "QR_PAYMENT";

    [Column("voucher_code")]
    [MaxLength(50)]
    public string? VoucherCode { get; set; }

    [Column("discount_amount")]
    public long DiscountAmount { get; set; } = 0;

    [Column("shipping_fee")]
    public long ShippingFee { get; set; } = 0;

    [Column("user_id")]
    [MaxLength(50)]
    public string? UserId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public ICollection<PaymentRecord> Payments { get; set; } = new List<PaymentRecord>();
    public ICollection<OrderStatusHistory> StatusHistories { get; set; } = new List<OrderStatusHistory>();
    public ICollection<RefundRequest> RefundRequests { get; set; } = new List<RefundRequest>();
}
