using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Features.Checkout.PaymentProviders;

namespace PhoneStoreAPI.Features.Checkout.Services;

public class ReconciliationService : IReconciliationService
{
    private readonly AppDbContext _context;
    private readonly IPaymentProvider _paymentProvider;
    private readonly ILogger<ReconciliationService> _logger;

    public ReconciliationService(
        AppDbContext context,
        IPaymentProvider paymentProvider,
        ILogger<ReconciliationService> logger)
    {
        _context = context;
        _paymentProvider = paymentProvider;
        _logger = logger;
    }

    public async Task<ReconciliationReportDTO> ReconcileDailyTransactionsAsync(DateTime? date = null, CancellationToken cancellationToken = default)
    {
        var targetDate = (date ?? DateTime.UtcNow).Date;
        var nextDate = targetDate.AddDays(1);

        var orders = await _context.Orders
            .Include(o => o.Payments)
            .Where(o => o.CreatedAt >= targetDate && o.CreatedAt < nextDate)
            .ToListAsync(cancellationToken);

        var payments = await _context.Payments
            .Where(p => p.CreatedAt >= targetDate && p.CreatedAt < nextDate)
            .ToListAsync(cancellationToken);

        var report = new ReconciliationReportDTO
        {
            ReconciledAt = DateTime.UtcNow,
            TotalOrdersChecked = orders.Count,
            TotalPaymentsChecked = payments.Count
        };

        // 1. Kiểm tra đơn đã PAID nhưng không có bản ghi payment thành công
        foreach (var order in orders.Where(o => o.Status == OrderStateMachine.Statuses.Paid))
        {
            var successPayment = order.Payments.FirstOrDefault(p => p.Status == "SUCCESS");
            if (successPayment == null)
            {
                var discrepancy = new ReconciliationDiscrepancyDTO
                {
                    Type = "ORDER_PAID_NO_PAYMENT",
                    OrderCode = order.OrderCode,
                    ExpectedAmount = order.Total,
                    ActualAmount = 0,
                    Description = $"Đơn #{order.OrderCode} ở trạng thái PAID nhưng không tìm thấy bản ghi Payment thành công!"
                };
                report.Discrepancies.Add(discrepancy);
                _logger.LogWarning("[RECONCILIATION_ALERT] {Description}", discrepancy.Description);
            }
        }

        // 2. Kiểm tra có tiền (Payment SUCCESS) nhưng đơn bị hủy hoặc EXPIRED mà chưa có refund
        foreach (var order in orders.Where(o => o.Status == OrderStateMachine.Statuses.Cancelled || o.Status == OrderStateMachine.Statuses.Expired))
        {
            var successPayment = order.Payments.FirstOrDefault(p => p.Status == "SUCCESS");
            if (successPayment != null)
            {
                var refund = await _context.RefundRequests.FirstOrDefaultAsync(r => r.OrderId == order.Id, cancellationToken);
                if (refund == null || refund.Status != "COMPLETED")
                {
                    var discrepancy = new ReconciliationDiscrepancyDTO
                    {
                        Type = "MONEY_RECEIVED_ORDER_CANCELLED",
                        OrderCode = order.OrderCode,
                        ProviderTxnId = successPayment.ProviderTxnId,
                        ExpectedAmount = 0,
                        ActualAmount = successPayment.Amount,
                        Description = $"Đơn #{order.OrderCode} có tiền thành công ({successPayment.Amount:N0} VND) nhưng đơn bị {order.Status} mà chưa hoàn tất hoàn tiền!"
                    };
                    report.Discrepancies.Add(discrepancy);
                    _logger.LogCritical("[RECONCILIATION_ALERT] {Description}", discrepancy.Description);
                }
            }
        }

        // 3. Kiểm tra số tiền lệch giữa Payment và Order
        foreach (var payment in payments.Where(p => p.Status == "AMOUNT_MISMATCH"))
        {
            var order = orders.FirstOrDefault(o => o.Id == payment.OrderId);
            var discrepancy = new ReconciliationDiscrepancyDTO
            {
                Type = "AMOUNT_MISMATCH",
                OrderCode = order?.OrderCode ?? $"OrderID_{payment.OrderId}",
                ProviderTxnId = payment.ProviderTxnId,
                ExpectedAmount = order?.Total ?? 0,
                ActualAmount = payment.Amount,
                Description = $"Giao dịch {payment.ProviderTxnId} có số tiền {payment.Amount:N0} VND khác với tổng đơn {order?.Total:N0} VND."
            };
            report.Discrepancies.Add(discrepancy);
        }

        return report;
    }
}
