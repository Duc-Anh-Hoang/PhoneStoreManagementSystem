using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.PaymentProviders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Checkout.Services;

public class RefundAndCancellationService : IRefundAndCancellationService
{
    private readonly AppDbContext _context;
    private readonly IOrderStateMachine _stateMachine;
    private readonly IPaymentProvider _paymentProvider;

    public RefundAndCancellationService(
        AppDbContext context,
        IOrderStateMachine stateMachine,
        IPaymentProvider paymentProvider)
    {
        _context = context;
        _stateMachine = stateMachine;
        _paymentProvider = paymentProvider;
    }

    public async Task<CancelOrderResult> CancelOrderAsync(string orderCode, string reason, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderCode == orderCode, cancellationToken);

        if (order == null)
        {
            return new CancelOrderResult(false, $"Không tìm thấy đơn hàng #{orderCode}.", null);
        }

        var isRelational = _context.Database.IsRelational();
        Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? tx = null;

        if (isRelational)
        {
            tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
        }

        try
        {
            if (order.Status == OrderStateMachine.Statuses.PendingPayment)
            {
                // Hủy đơn PENDING_PAYMENT thì nhả kho (chỉ nhả khi đổi trạng thái thành công)
                var transition = await _stateMachine.TransitionAsync(
                    order.Id,
                    OrderStateMachine.Statuses.PendingPayment,
                    OrderStateMachine.Statuses.Cancelled,
                    $"Khách hủy đơn hàng: {reason}",
                    cancellationToken);

                if (!transition.Success)
                {
                    if (tx != null) await tx.RollbackAsync(cancellationToken);
                    return new CancelOrderResult(false, transition.ErrorMessage ?? "Không thể hủy đơn hàng.", order.Status);
                }

                // Nhả kho
                foreach (var item in order.Items)
                {
                    if (isRelational)
                    {
                        await _context.Database.ExecuteSqlInterpolatedAsync(
                            $"UPDATE variants SET stock = stock + {item.Qty}, updated_at = {DateTime.UtcNow} WHERE id = {item.VariantId}",
                            cancellationToken);
                    }
                    else
                    {
                        var variant = await _context.ProductVariants.FindAsync(new object[] { item.VariantId }, cancellationToken);
                        if (variant != null) variant.Stock += item.Qty;
                    }
                }

                // Hoàn lại lượt dùng voucher (nếu có)
                if (!string.IsNullOrWhiteSpace(order.VoucherCode))
                {
                    if (isRelational)
                    {
                        await _context.Database.ExecuteSqlInterpolatedAsync(
                            $"UPDATE vouchers SET used_count = GREATEST(0, used_count - 1), updated_at = {DateTime.UtcNow} WHERE code = {order.VoucherCode}",
                            cancellationToken);
                    }
                    else
                    {
                        var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Code == order.VoucherCode, cancellationToken);
                        if (voucher != null && voucher.UsedCount > 0) voucher.UsedCount--;
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                if (tx != null) await tx.CommitAsync(cancellationToken);

                return new CancelOrderResult(true, "Hủy đơn hàng và nhả kho thành công.", OrderStateMachine.Statuses.Cancelled);
            }
            else if (order.Status == OrderStateMachine.Statuses.Paid)
            {
                // Hủy đơn đã PAID thì tạo refund request và trả lại kho
                var transition = await _stateMachine.TransitionAsync(
                    order.Id,
                    OrderStateMachine.Statuses.Paid,
                    OrderStateMachine.Statuses.Refunded,
                    $"Khách hủy đơn hàng đã thanh toán: {reason}",
                    cancellationToken);

                if (!transition.Success)
                {
                    if (tx != null) await tx.RollbackAsync(cancellationToken);
                    return new CancelOrderResult(false, transition.ErrorMessage ?? "Không thể hủy đơn hàng.", order.Status);
                }

                // Trả lại kho
                foreach (var item in order.Items)
                {
                    if (isRelational)
                    {
                        await _context.Database.ExecuteSqlInterpolatedAsync(
                            $"UPDATE variants SET stock = stock + {item.Qty}, updated_at = {DateTime.UtcNow} WHERE id = {item.VariantId}",
                            cancellationToken);
                    }
                    else
                    {
                        var variant = await _context.ProductVariants.FindAsync(new object[] { item.VariantId }, cancellationToken);
                        if (variant != null) variant.Stock += item.Qty;
                    }
                }

                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.OrderId == order.Id && p.Status == "SUCCESS", cancellationToken);

                // Tạo refund request
                var refund = new RefundRequest
                {
                    OrderId = order.Id,
                    PaymentId = payment?.Id,
                    Amount = order.Total,
                    Reason = reason,
                    Status = "PENDING",
                    ProviderRefundId = "REF_" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.RefundRequests.Add(refund);

                await _context.SaveChangesAsync(cancellationToken);
                if (tx != null) await tx.CommitAsync(cancellationToken);

                // Kích hoạt hoàn tiền qua cổng
                await ProcessRefundAsync(refund.Id, cancellationToken);

                return new CancelOrderResult(true, "Hủy đơn hàng đã thanh toán, đã trả kho và tạo yêu cầu hoàn tiền thành công.", OrderStateMachine.Statuses.Refunded);
            }
            else
            {
                if (tx != null) await tx.RollbackAsync(cancellationToken);
                return new CancelOrderResult(false, $"Không thể hủy đơn hàng đang ở trạng thái '{order.Status}'.", order.Status);
            }
        }
        catch
        {
            if (tx != null) await tx.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (tx != null) await tx.DisposeAsync();
        }
    }

    public async Task<RefundResult> ProcessRefundAsync(long refundRequestId, CancellationToken cancellationToken = default)
    {
        var refund = await _context.RefundRequests
            .Include(r => r.Payment)
            .FirstOrDefaultAsync(r => r.Id == refundRequestId, cancellationToken);

        if (refund == null)
        {
            return new RefundResult(false, null, "Không tìm thấy yêu cầu hoàn tiền.");
        }

        // Idempotent: nếu đã hoàn tiền rồi thì không gọi cổng hoàn tiền lần nữa
        if (refund.Status == "COMPLETED")
        {
            return new RefundResult(true, refund.ProviderRefundId, "Yêu cầu hoàn tiền đã hoàn thành trước đó (Idempotent).");
        }

        var providerTxnId = refund.Payment?.ProviderTxnId ?? $"TXN_ORDER_{refund.OrderId}";
        var refundIdempotencyKey = refund.ProviderRefundId ?? $"REF_{refund.Id}";

        try
        {
            var result = await _paymentProvider.RefundAsync(
                providerTxnId,
                refund.Amount,
                refund.Reason,
                refundIdempotencyKey,
                cancellationToken);

            if (result.Success)
            {
                refund.Status = "COMPLETED";
                refund.ProviderRefundId = result.ProviderRefundId ?? refundIdempotencyKey;
                refund.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                refund.Status = "FAILED";
                refund.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            refund.Status = "FAILED";
            refund.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            return new RefundResult(false, refundIdempotencyKey, ex.Message);
        }
    }
}
