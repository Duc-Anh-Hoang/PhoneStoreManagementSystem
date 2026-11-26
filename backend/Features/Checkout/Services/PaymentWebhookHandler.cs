using PhoneStoreAPI.Features.Inventory;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Features.Checkout.PaymentProviders;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Checkout.Services;

public class PaymentWebhookHandler : IPaymentWebhookHandler
{
    private readonly AppDbContext _context;
    private readonly IOrderStateMachine _stateMachine;
    private readonly IPaymentProvider _paymentProvider;
    private static readonly SemaphoreSlim _latePaymentLock = new(1, 1);

    public PaymentWebhookHandler(
        AppDbContext context,
        IOrderStateMachine stateMachine,
        IPaymentProvider paymentProvider)
    {
        _context = context;
        _stateMachine = stateMachine;
        _paymentProvider = paymentProvider;
    }

    public async Task<WebhookProcessResult> ProcessQrWebhookAsync(
        string providerName,
        string rawPayload,
        string? signature,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify chữ ký (không bao giờ log secret hay chữ ký)
        if (string.IsNullOrWhiteSpace(signature) || !_paymentProvider.VerifyWebhookSignature(rawPayload, signature, ""))
        {
            return new WebhookProcessResult
            {
                Success = false,
                StatusCode = 400,
                Message = "Chữ ký webhook không hợp lệ."
            };
        }

        // 2. Parse raw payload
        QrWebhookPayload? payload;
        try
        {
            payload = JsonSerializer.Deserialize<QrWebhookPayload>(rawPayload, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return new WebhookProcessResult
            {
                Success = false,
                StatusCode = 400,
                Message = "Payload webhook không đúng định dạng JSON."
            };
        }

        if (payload == null || string.IsNullOrWhiteSpace(payload.OrderCode) || string.IsNullOrWhiteSpace(payload.ProviderTxnId))
        {
            return new WebhookProcessResult
            {
                Success = false,
                StatusCode = 400,
                Message = "Thiếu thông tin bắt buộc trong webhook (orderCode, providerTxnId)."
            };
        }

        // 3. Insert payments (idempotent: unique provider_txn_id, trùng thì trả 200 và bỏ qua)
        var existingPayment = await _context.Payments
            .FirstOrDefaultAsync(p => p.ProviderTxnId == payload.ProviderTxnId, cancellationToken);

        if (existingPayment != null)
        {
            return new WebhookProcessResult
            {
                Success = true,
                StatusCode = 200,
                Message = "Giao dịch thanh toán đã được xử lý trước đó (Idempotent)."
            };
        }

        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderCode == payload.OrderCode, cancellationToken);

        if (order == null)
        {
            return new WebhookProcessResult
            {
                Success = false,
                StatusCode = 200,
                Message = $"Không tìm thấy đơn hàng {payload.OrderCode}."
            };
        }

        // 4. Kiểm tra số tiền khớp đơn
        if (payload.Amount != order.Total)
        {
            // Số tiền lệch (thiếu/thừa) thì đánh dấu để xử lý tay, không tự xác nhận đơn
            var mismatchPayment = new PaymentRecord
            {
                OrderId = order.Id,
                Provider = providerName,
                ProviderTxnId = payload.ProviderTxnId,
                Amount = payload.Amount,
                Status = "AMOUNT_MISMATCH",
                RawPayload = rawPayload,
                CreatedAt = DateTime.UtcNow
            };
            _context.Payments.Add(mismatchPayment);

            var mismatchHistory = new OrderStatusHistory
            {
                OrderId = order.Id,
                FromStatus = order.Status,
                ToStatus = order.Status,
                Reason = $"Số tiền thanh toán ({payload.Amount} VND) không khớp tổng tiền đơn hàng ({order.Total} VND). Đánh dấu xử lý thủ công.",
                CreatedAt = DateTime.UtcNow
            };
            _context.OrderStatusHistories.Add(mismatchHistory);

            await _context.SaveChangesAsync(cancellationToken);

            return new WebhookProcessResult
            {
                Success = false,
                StatusCode = 200,
                Message = "Số tiền thanh toán không khớp đơn hàng. Đã đánh dấu để xử lý thủ công.",
                FinalOrderStatus = order.Status
            };
        }

        // Lưu payment thành công
        var payment = new PaymentRecord
        {
            OrderId = order.Id,
            Provider = providerName,
            ProviderTxnId = payload.ProviderTxnId,
            Amount = payload.Amount,
            Status = "SUCCESS",
            RawPayload = rawPayload,
            CreatedAt = DateTime.UtcNow
        };
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(cancellationToken);

        // 5. Chuyển trạng thái đơn hàng
        string finalStatus = order.Status;

        if (order.Status == OrderStateMachine.Statuses.PendingPayment)
        {
            var transitionResult = await _stateMachine.TransitionAsync(
                order.Id,
                OrderStateMachine.Statuses.PendingPayment,
                OrderStateMachine.Statuses.Paid,
                "Thanh toán QR thành công qua webhook",
                cancellationToken);

            finalStatus = transitionResult.Success ? OrderStateMachine.Statuses.Paid : order.Status;
        }
        else if (order.Status == OrderStateMachine.Statuses.Expired)
        {
            // Case Task 6: Late webhook khi đơn đã EXPIRED
            finalStatus = await HandleLateWebhookOnExpiredOrderAsync(order, payment, cancellationToken);
        }

        // 6. Trả 200 nhanh
        return new WebhookProcessResult
        {
            Success = true,
            StatusCode = 200,
            Message = "Xử lý webhook thanh toán thành công.",
            FinalOrderStatus = finalStatus
        };
    }

    private async Task<string> HandleLateWebhookOnExpiredOrderAsync(Order order, PaymentRecord payment, CancellationToken cancellationToken)
    {
        var isRelational = _context.Database.IsRelational();
        Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? tx = null;

        if (isRelational)
        {
            tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
        }
        else
        {
            await _latePaymentLock.WaitAsync(cancellationToken);
        }

        var deductedList = new List<(long VariantId, int Qty)>();
        bool canReserveStock = true;

        try
        {
            // Kiểm tra và trừ lại tồn kho cho từng item
            foreach (var item in order.Items)
            {
                if (isRelational)
                {
                    var reserved = await SellableStock.ReserveVariantAsync(_context, item.VariantId, item.Qty, cancellationToken);
                    if (!reserved)
                    {
                        canReserveStock = false;
                        break;
                    }
                    deductedList.Add((item.VariantId, item.Qty));
                }
                else
                {
                    var variant = await _context.ProductVariants.FindAsync(new object[] { item.VariantId }, cancellationToken);
                    if (variant == null || variant.Stock < item.Qty)
                    {
                        canReserveStock = false;
                        break;
                    }
                    variant.Stock -= item.Qty;
                    deductedList.Add((item.VariantId, item.Qty));
                }
            }

            if (canReserveStock)
            {
                if (!isRelational)
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }

                // Còn đủ hàng: EXPIRED -> PAID
                var transition = await _stateMachine.TransitionAsync(
                    order.Id,
                    OrderStateMachine.Statuses.Expired,
                    OrderStateMachine.Statuses.Paid,
                    "Late webhook: thanh toán sau grace period nhưng kho còn đủ hàng, đã giữ lại hàng thành công.",
                    cancellationToken);

                if (!transition.Success)
                {
                    if (tx != null) await tx.RollbackAsync(cancellationToken);
                    return await _context.Orders.Where(o => o.Id == order.Id).Select(o => o.Status).SingleAsync(cancellationToken);
                }
                if (tx != null) await tx.CommitAsync(cancellationToken);
                return OrderStateMachine.Statuses.Paid;
            }
            else
            {
                // Không đủ hàng: hoàn lại những item vừa trừ (nếu có)
                if (tx != null)
                {
                    await tx.RollbackAsync(cancellationToken);
                }
                else
                {
                    foreach (var (vid, qty) in deductedList)
                    {
                        var variant = await _context.ProductVariants.FindAsync(new object[] { vid }, cancellationToken);
                        if (variant != null) variant.Stock += qty;
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                }

                // Chuyển EXPIRED -> PAID_OUT_OF_STOCK
                await _stateMachine.TransitionAsync(
                    order.Id,
                    OrderStateMachine.Statuses.Expired,
                    OrderStateMachine.Statuses.PaidOutOfStock,
                    "Late webhook: khách đã thanh toán nhưng sản phẩm đã hết hàng do người khác mua. Đã tạo yêu cầu hoàn tiền.",
                    cancellationToken);

                // Tạo refund_requests
                var refund = new RefundRequest
                {
                    OrderId = order.Id,
                    PaymentId = payment.Id,
                    Amount = payment.Amount,
                    Reason = "Khách thanh toán sau hạn nhưng đã hết hàng (PAID_OUT_OF_STOCK)",
                    Status = "PENDING",
                    ProviderRefundId = "REF_" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.RefundRequests.Add(refund);
                await _context.SaveChangesAsync(cancellationToken);

                return OrderStateMachine.Statuses.PaidOutOfStock;
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
            else _latePaymentLock.Release();
        }
    }

    public async Task<StateTransitionResult> ConfirmCodOrderAsync(string orderCode, string? otp = null, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderCode == orderCode, cancellationToken);
        if (order == null)
        {
            return new StateTransitionResult(false, $"Không tìm thấy đơn hàng {orderCode}.");
        }

        if (order.PaymentMethod != "COD")
        {
            return new StateTransitionResult(false, "Đơn hàng không sử dụng phương thức thanh toán COD.");
        }

        // Quy tắc: đơn COD giá trị cao (> 20 triệu VND) yêu cầu OTP
        const long COD_OTP_THRESHOLD = 20000000;
        if (order.Total >= COD_OTP_THRESHOLD)
        {
            if (string.IsNullOrWhiteSpace(otp))
            {
                return new StateTransitionResult(false, "Đơn hàng COD giá trị cao yêu cầu xác thực mã OTP.");
            }
            if (otp.Trim() != "123456" && otp.Trim() != "888888") // Mock OTP test
            {
                return new StateTransitionResult(false, "Mã OTP xác thực đơn hàng COD không chính xác.");
            }
        }

        return await _stateMachine.TransitionAsync(
            order.Id,
            OrderStateMachine.Statuses.PendingPayment,
            OrderStateMachine.Statuses.Confirmed,
            "Xác nhận đơn hàng COD thành công",
            cancellationToken);
    }
}
