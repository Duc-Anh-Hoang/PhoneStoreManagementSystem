using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhoneStoreAPI.Common.Configs;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Checkout.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    private readonly CheckoutOptions _options;
    private static readonly SemaphoreSlim _inMemoryInventoryLock = new(1, 1);

    public OrderService(AppDbContext context, IOptions<CheckoutOptions> options)
    {
        _context = context;
        _options = options.Value;
    }

    public async Task<OrderDTO> CreateOrderAsync(
        string idempotencyKey,
        CreateOrderRequest request,
        string? userId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            throw new ArgumentException("Idempotency-Key không được để trống.", nameof(idempotencyKey));
        }

        if (request.Items == null || request.Items.Count == 0)
        {
            throw new InvalidOperationException("Đơn hàng phải có ít nhất một sản phẩm.");
        }

        foreach (var item in request.Items)
        {
            if (item.Qty <= 0)
            {
                throw new InvalidOperationException($"Số lượng sản phẩm variant {item.VariantId} phải lớn hơn 0.");
            }
            if (item.Qty > _options.MaxQtyPerOrder)
            {
                throw new InvalidOperationException($"Số lượng đặt cho variant {item.VariantId} vượt quá giới hạn tối đa ({_options.MaxQtyPerOrder} chiếc).");
            }
        }

        // 1. Kiểm tra IdempotencyKey trước
        var existingOrder = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.IdempotencyKey == idempotencyKey, cancellationToken);

        if (existingOrder != null)
        {
            return MapToDto(existingOrder);
        }

        var isRelational = _context.Database.IsRelational();
        Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? tx = null;

        if (isRelational)
        {
            tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
        }
        else
        {
            await _inMemoryInventoryLock.WaitAsync(cancellationToken);
        }

        var deductedItems = new List<(long VariantId, int Qty)>();

        try
        {
            // Kiểm tra lại lần nữa trong transaction phòng khi 2 request cùng IdempotencyKey chạy song song
            var doubleCheckOrder = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.IdempotencyKey == idempotencyKey, cancellationToken);

            if (doubleCheckOrder != null)
            {
                if (tx != null) await tx.RollbackAsync(cancellationToken);
                return MapToDto(doubleCheckOrder);
            }

            // 2. Trừ tồn kho có điều kiện (lõi chống bán lố)
            var variantSnapshots = new List<(ProductVariant Variant, int Qty)>();

            foreach (var item in request.Items)
            {
                if (isRelational)
                {
                    var affected = await _context.Database.ExecuteSqlInterpolatedAsync(
                        $"UPDATE variants SET stock = stock - {item.Qty}, updated_at = {DateTime.UtcNow} WHERE id = {item.VariantId} AND stock >= {item.Qty}",
                        cancellationToken);

                    if (affected == 0)
                    {
                        throw new InvalidOperationException($"Sản phẩm variant #{item.VariantId} đã hết hàng hoặc không đủ số lượng tồn kho.");
                    }

                    var variant = await _context.ProductVariants
                        .Include(v => v.Product)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(v => v.Id == item.VariantId, cancellationToken);

                    if (variant == null)
                    {
                        throw new InvalidOperationException($"Không tìm thấy sản phẩm variant #{item.VariantId}.");
                    }

                    deductedItems.Add((item.VariantId, item.Qty));
                    variantSnapshots.Add((variant, item.Qty));
                }
                else
                {
                    // InMemory atomic update
                    var variant = await _context.ProductVariants
                        .Include(v => v.Product)
                        .FirstOrDefaultAsync(v => v.Id == item.VariantId, cancellationToken);

                    if (variant == null || variant.Stock < item.Qty)
                    {
                        throw new InvalidOperationException($"Sản phẩm variant #{item.VariantId} đã hết hàng hoặc không đủ số lượng tồn kho.");
                    }

                    variant.Stock -= item.Qty;
                    variant.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync(cancellationToken);

                    deductedItems.Add((item.VariantId, item.Qty));
                    variantSnapshots.Add((variant, item.Qty));
                }
            }

            // 3. Tính tổng tiền và voucher ở server
            long subtotal = variantSnapshots.Sum(vs => vs.Variant.Price * vs.Qty);
            long discountAmount = 0;
            string? appliedVoucher = null;

            if (!string.IsNullOrWhiteSpace(request.VoucherCode))
            {
                var cleanCode = request.VoucherCode.Trim().ToUpperInvariant();
                if (isRelational)
                {
                    var vAffected = await _context.Database.ExecuteSqlInterpolatedAsync(
                        $"UPDATE vouchers SET used_count = used_count + 1, updated_at = {DateTime.UtcNow} WHERE code = {cleanCode} AND used_count < max_uses AND expires_at > {DateTime.UtcNow}",
                        cancellationToken);

                    if (vAffected == 0)
                    {
                        throw new InvalidOperationException($"Voucher '{cleanCode}' không hợp lệ, đã hết lượt sử dụng hoặc đã quá hạn.");
                    }

                    var vDb = await _context.Vouchers.AsNoTracking().FirstOrDefaultAsync(v => v.Code == cleanCode, cancellationToken);
                    if (vDb != null)
                    {
                        if (subtotal < vDb.MinOrderTotal)
                        {
                            throw new InvalidOperationException($"Đơn hàng chưa đạt giá trị tối thiểu {vDb.MinOrderTotal:N0} VND để áp dụng voucher.");
                        }
                        discountAmount = Math.Min(vDb.DiscountAmount, subtotal);
                        appliedVoucher = cleanCode;
                    }
                }
                else
                {
                    var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Code == cleanCode, cancellationToken);
                    if (voucher == null || voucher.UsedCount >= voucher.MaxUses || voucher.ExpiresAt < DateTime.UtcNow)
                    {
                        throw new InvalidOperationException($"Voucher '{cleanCode}' không hợp lệ, đã hết lượt sử dụng hoặc đã quá hạn.");
                    }
                    if (subtotal < voucher.MinOrderTotal)
                    {
                        throw new InvalidOperationException($"Đơn hàng chưa đạt giá trị tối thiểu {voucher.MinOrderTotal:N0} VND để áp dụng voucher.");
                    }
                    voucher.UsedCount++;
                    voucher.UpdatedAt = DateTime.UtcNow;
                    discountAmount = Math.Min(voucher.DiscountAmount, subtotal);
                    appliedVoucher = cleanCode;
                }
            }

            // Phí vận chuyển: Miễn phí nếu >= 5.000.000 VND, ngược lại 30.000 VND
            long shippingFee = subtotal >= 5000000 ? 0 : 30000;
            long totalAmount = Math.Max(0, subtotal - discountAmount + shippingFee);

            // 4. Tạo Order và OrderItems
            var orderCode = "ORD" + DateTime.UtcNow.ToString("yyMMdd") + Random.Shared.Next(100000, 999999);
            var now = DateTime.UtcNow;

            var order = new Order
            {
                OrderCode = orderCode,
                Status = OrderStateMachine.Statuses.PendingPayment,
                Total = totalAmount,
                ExpiresAt = now.Add(_options.ReservationTtl),
                IdempotencyKey = idempotencyKey,
                RecipientName = request.RecipientName,
                RecipientPhone = request.RecipientPhone,
                ShippingAddress = request.ShippingAddress,
                PaymentMethod = request.PaymentMethod,
                VoucherCode = appliedVoucher,
                DiscountAmount = discountAmount,
                ShippingFee = shippingFee,
                UserId = userId,
                CreatedAt = now,
                UpdatedAt = now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var (variant, qty) in variantSnapshots)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    VariantId = variant.Id,
                    ProductNameSnapshot = variant.Product?.Name ?? "Điện thoại",
                    SkuSnapshot = variant.Sku,
                    PriceSnapshot = variant.Price,
                    Qty = qty,
                    CreatedAt = now
                };
                order.Items.Add(orderItem);
                _context.OrderItems.Add(orderItem);
            }

            // 5. Ghi nhận lịch sử trạng thái
            var history = new OrderStatusHistory
            {
                OrderId = order.Id,
                FromStatus = null,
                ToStatus = OrderStateMachine.Statuses.PendingPayment,
                Reason = "Tạo đơn hàng và giữ hàng thành công",
                CreatedAt = now
            };
            _context.OrderStatusHistories.Add(history);

            await _context.SaveChangesAsync(cancellationToken);

            if (tx != null)
            {
                await tx.CommitAsync(cancellationToken);
            }

            return MapToDto(order);
        }
        catch
        {
            if (tx != null)
            {
                await tx.RollbackAsync(cancellationToken);
            }
            else
            {
                // Hoàn lại kho trong InMemory nếu có lỗi giữa chừng
                foreach (var (variantId, qty) in deductedItems)
                {
                    var variant = await _context.ProductVariants.FindAsync(new object[] { variantId }, cancellationToken);
                    if (variant != null)
                    {
                        variant.Stock += qty;
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);
            }

            throw;
        }
        finally
        {
            if (tx != null)
            {
                await tx.DisposeAsync();
            }
            else
            {
                _inMemoryInventoryLock.Release();
            }
        }
    }

    public async Task<OrderDTO?> GetOrderByCodeAsync(string orderCode, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderCode == orderCode, cancellationToken);

        return order == null ? null : MapToDto(order);
    }

    public async Task<OrderDTO?> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        return order == null ? null : MapToDto(order);
    }

    private static OrderDTO MapToDto(Order order)
    {
        return new OrderDTO
        {
            Id = order.Id,
            OrderCode = order.OrderCode,
            Status = order.Status,
            Total = order.Total,
            ExpiresAt = order.ExpiresAt,
            IdempotencyKey = order.IdempotencyKey,
            RecipientName = order.RecipientName,
            RecipientPhone = order.RecipientPhone,
            ShippingAddress = order.ShippingAddress,
            PaymentMethod = order.PaymentMethod,
            VoucherCode = order.VoucherCode,
            DiscountAmount = order.DiscountAmount,
            ShippingFee = order.ShippingFee,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(i => new OrderItemDTO
            {
                Id = i.Id,
                VariantId = i.VariantId,
                ProductName = i.ProductNameSnapshot,
                Sku = i.SkuSnapshot,
                Price = i.PriceSnapshot,
                Qty = i.Qty
            }).ToList()
        };
    }
}
