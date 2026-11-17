using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PhoneStoreAPI.Common.Configs;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.PaymentProviders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Checkout.Services;

public class OrderExpirationService : IOrderExpirationService
{
    private readonly AppDbContext _context;
    private readonly IOrderStateMachine _stateMachine;
    private readonly IPaymentProvider _paymentProvider;
    private readonly CheckoutOptions _options;
    private readonly ILogger<OrderExpirationService> _logger;

    public OrderExpirationService(
        AppDbContext context,
        IOrderStateMachine stateMachine,
        IPaymentProvider paymentProvider,
        IOptions<CheckoutOptions> options,
        ILogger<OrderExpirationService> logger)
    {
        _context = context;
        _stateMachine = stateMachine;
        _paymentProvider = paymentProvider;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<ExpirationJobResult> RunExpirationScanAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        // now > expires_at + GRACE_PERIOD => expires_at < now - GRACE_PERIOD
        var cutoff = now.Subtract(_options.GracePeriod);

        var expiredCandidates = await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.Status == OrderStateMachine.Statuses.PendingPayment && o.ExpiresAt < cutoff)
            .ToListAsync(cancellationToken);

        int processed = 0;
        int paidCount = 0;
        int expiredCount = 0;

        foreach (var order in expiredCandidates)
        {
            processed++;

            // 1. Kiểm tra với cổng thanh toán trước
            var queryResult = await _paymentProvider.QueryTransactionAsync(order.OrderCode, cancellationToken: cancellationToken);

            if (queryResult.IsPaid && queryResult.Amount == order.Total)
            {
                // Cổng báo đã trả -> xác nhận PAID
                var existingPayment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.ProviderTxnId == queryResult.ProviderTxnId, cancellationToken);

                if (existingPayment == null)
                {
                    _context.Payments.Add(new PaymentRecord
                    {
                        OrderId = order.Id,
                        Provider = _paymentProvider.ProviderName,
                        ProviderTxnId = string.IsNullOrWhiteSpace(queryResult.ProviderTxnId) ? $"QUERY_{order.OrderCode}" : queryResult.ProviderTxnId,
                        Amount = queryResult.Amount,
                        Status = "SUCCESS",
                        RawPayload = "Recovered by Expiration Job via QueryTransactionAsync",
                        CreatedAt = DateTime.UtcNow
                    });
                    await _context.SaveChangesAsync(cancellationToken);
                }

                var transResult = await _stateMachine.TransitionAsync(
                    order.Id,
                    OrderStateMachine.Statuses.PendingPayment,
                    OrderStateMachine.Statuses.Paid,
                    "Expiration Job: Cổng thanh toán xác nhận giao dịch thành công (queryTransaction)",
                    cancellationToken);

                if (transResult.Success)
                {
                    paidCount++;
                }
            }
            else
            {
                // Chưa trả -> cập nhật có điều kiện sang EXPIRED và hoàn trả kho trong cùng transaction
                var isRelational = _context.Database.IsRelational();
                Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? tx = null;

                if (isRelational)
                {
                    tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
                }

                try
                {
                    var transResult = await _stateMachine.TransitionAsync(
                        order.Id,
                        OrderStateMachine.Statuses.PendingPayment,
                        OrderStateMachine.Statuses.Expired,
                        "Expiration Job: Đơn hàng quá hạn thanh toán và thời gian ân hạn (grace period).",
                        cancellationToken);

                    if (transResult.Success)
                    {
                        // Hoàn lại kho trong cùng transaction
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
                                if (variant != null)
                                {
                                    variant.Stock += item.Qty;
                                }
                            }
                        }

                        await _context.SaveChangesAsync(cancellationToken);
                        if (tx != null) await tx.CommitAsync(cancellationToken);

                        expiredCount++;
                        _logger.LogInformation("Đã hoàn kho và chuyển EXPIRED thành công cho đơn #{OrderCode}", order.OrderCode);
                    }
                    else
                    {
                        // Bị tiến trình khác (như webhook) cập nhật trước đó
                        if (tx != null) await tx.RollbackAsync(cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    if (tx != null) await tx.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Lỗi khi xử lý hết hạn cho đơn #{OrderCode}", order.OrderCode);
                }
                finally
                {
                    if (tx != null) await tx.DisposeAsync();
                }
            }
        }

        return new ExpirationJobResult(processed, paidCount, expiredCount);
    }
}
