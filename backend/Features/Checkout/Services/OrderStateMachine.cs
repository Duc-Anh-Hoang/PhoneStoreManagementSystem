using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Checkout.Services;

public class OrderStateMachine : IOrderStateMachine
{
    private readonly AppDbContext _context;
    private static readonly object _stateLock = new();

    public static class Statuses
    {
        public const string PendingPayment = "PENDING_PAYMENT";
        public const string Paid = "PAID";
        public const string Expired = "EXPIRED";
        public const string Cancelled = "CANCELLED";
        public const string PaidOutOfStock = "PAID_OUT_OF_STOCK";
        public const string Confirmed = "CONFIRMED";
        public const string Shipping = "SHIPPING";
        public const string Completed = "COMPLETED";
        public const string Refunded = "REFUNDED";
    }

    private static readonly Dictionary<string, HashSet<string>> AllowedTransitions = new()
    {
        [Statuses.PendingPayment] = new() { Statuses.Paid, Statuses.Expired, Statuses.Cancelled },
        [Statuses.Expired] = new() { Statuses.Paid, Statuses.PaidOutOfStock },
        [Statuses.Paid] = new() { Statuses.Confirmed, Statuses.Refunded },
        [Statuses.Confirmed] = new() { Statuses.Shipping },
        [Statuses.Shipping] = new() { Statuses.Completed },
        [Statuses.PaidOutOfStock] = new() { Statuses.Refunded }
    };

    public OrderStateMachine(AppDbContext context)
    {
        _context = context;
    }

    public bool CanTransition(string fromStatus, string toStatus)
    {
        if (AllowedTransitions.TryGetValue(fromStatus, out var targetSet))
        {
            return targetSet.Contains(toStatus);
        }
        return false;
    }

    public async Task<StateTransitionResult> TransitionAsync(
        long orderId,
        string expectedCurrentStatus,
        string targetStatus,
        string? reason = null,
        CancellationToken cancellationToken = default)
    {
        if (!CanTransition(expectedCurrentStatus, targetStatus))
        {
            return new StateTransitionResult(false, $"Chuyển trạng thái không hợp lệ từ '{expectedCurrentStatus}' sang '{targetStatus}'.");
        }

        bool updated = false;

        if (_context.Database.IsRelational())
        {
            var now = DateTime.UtcNow;
            var affected = await _context.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE orders SET status = {targetStatus}, updated_at = {now} WHERE id = {orderId} AND status = {expectedCurrentStatus}",
                cancellationToken);

            updated = affected == 1;
        }
        else
        {
            lock (_stateLock)
            {
                var order = _context.Orders.FirstOrDefault(o => o.Id == orderId && o.Status == expectedCurrentStatus);
                if (order != null)
                {
                    order.Status = targetStatus;
                    order.UpdatedAt = DateTime.UtcNow;
                    _context.SaveChanges();
                    updated = true;
                }
            }
        }

        if (!updated)
        {
            return new StateTransitionResult(false, $"Cập nhật trạng thái thất bại: đơn #{orderId} không ở trạng thái '{expectedCurrentStatus}' hoặc đã bị tiến trình khác cập nhật.");
        }

        // Ghi nhận lịch sử trạng thái
        var history = new OrderStatusHistory
        {
            OrderId = orderId,
            FromStatus = expectedCurrentStatus,
            ToStatus = targetStatus,
            Reason = reason ?? $"Chuyển trạng thái từ {expectedCurrentStatus} sang {targetStatus}",
            CreatedAt = DateTime.UtcNow
        };

        _context.OrderStatusHistories.Add(history);
        await _context.SaveChangesAsync(cancellationToken);

        return new StateTransitionResult(true);
    }
}
