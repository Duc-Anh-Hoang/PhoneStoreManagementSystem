using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.PaymentProviders;
using PhoneStoreAPI.Features.Checkout.Services;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class Task7RefundAndCancellationTests
{
    private class FlakyMockPaymentProvider : MockQrPaymentProvider
    {
        public int RefundCallCount { get; private set; }
        public bool ShouldFailFirstTime { get; set; } = false;

        public FlakyMockPaymentProvider() : base("test_secret_refund") { }

        public override Task<RefundResult> RefundAsync(string providerTxnId, long amount, string reason, string idempotencyRefundId, CancellationToken cancellationToken = default)
        {
            RefundCallCount++;

            if (ShouldFailFirstTime && RefundCallCount == 1)
            {
                return Task.FromResult(new RefundResult(false, null, "Cổng thanh toán phản hồi 500 Network Timeout tạm thời"));
            }

            return Task.FromResult(new RefundResult(true, "MOCK_REFUND_" + idempotencyRefundId, null));
        }
    }

    private (AppDbContext context, RefundAndCancellationService service, FlakyMockPaymentProvider provider) CreateService(string dbName)
    {
        var root = new Microsoft.EntityFrameworkCore.Storage.InMemoryDatabaseRoot();
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName, databaseRoot: root)
            .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new AppDbContext(dbOptions);

        var sm = new OrderStateMachine(context);
        var provider = new FlakyMockPaymentProvider();
        var service = new RefundAndCancellationService(context, sm, provider);

        return (context, service, provider);
    }

    [Fact]
    public async Task Cancelling_Twice_Does_Not_Add_Stock_Twice()
    {
        var (context, service, _) = CreateService(Guid.NewGuid().ToString());

        var product = new Product { Id = 1, Name = "iPhone 16 Pro Max" };
        var variant = new ProductVariant { Id = 301, ProductId = 1, Sku = "IP16-CANCEL-TEST", Color = "Desert", Storage = "256GB", Price = 30000000, Stock = 5 };
        context.Products.Add(product);
        context.ProductVariants.Add(variant);

        var order = new Order
        {
            Id = 301,
            OrderCode = "ORD-CANCEL-TWICE",
            Status = OrderStateMachine.Statuses.PendingPayment,
            Total = 30000000,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IdempotencyKey = "key-cancel-twice"
        };
        order.Items.Add(new OrderItem
        {
            OrderId = 301,
            VariantId = 301,
            ProductNameSnapshot = "iPhone 16 Pro Max",
            SkuSnapshot = "IP16-CANCEL-TEST",
            PriceSnapshot = 30000000,
            Qty = 2 // Đơn đặt 2 chiếc
        });
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // Lần 1: Hủy đơn thành công -> trả kho 2 chiếc (5 + 2 = 7)
        var res1 = await service.CancelOrderAsync(order.OrderCode, "Đổi ý không mua nữa");
        Assert.True(res1.Success);
        Assert.Equal(OrderStateMachine.Statuses.Cancelled, res1.FinalStatus);

        var refreshedVariant1 = await context.ProductVariants.FindAsync(301L);
        Assert.Equal(7, refreshedVariant1!.Stock);

        // Lần 2: Cố tình gọi hủy lại đơn đã CANCELLED
        var res2 = await service.CancelOrderAsync(order.OrderCode, "Hủy lần hai");
        Assert.False(res2.Success); // Bị từ chối

        // Tồn kho KHÔNG được cộng thêm lần nữa, vẫn là 7
        var refreshedVariant2 = await context.ProductVariants.FindAsync(301L);
        Assert.Equal(7, refreshedVariant2!.Stock);
    }

    [Fact]
    public async Task Refund_Fails_Midway_And_Retries_Without_Duplicate_Refund()
    {
        var (context, service, provider) = CreateService(Guid.NewGuid().ToString());

        var product = new Product { Id = 1, Name = "iPhone 16 Pro Max" };
        var variant = new ProductVariant { Id = 302, ProductId = 1, Sku = "IP16-REFUND-TEST", Color = "Natural", Storage = "512GB", Price = 40000000, Stock = 3 };
        context.Products.Add(product);
        context.ProductVariants.Add(variant);

        var order = new Order
        {
            Id = 302,
            OrderCode = "ORD-REFUND-RETRY",
            Status = OrderStateMachine.Statuses.Paid, // Đơn đã thanh toán
            Total = 40000000,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IdempotencyKey = "key-refund-retry"
        };
        order.Items.Add(new OrderItem
        {
            OrderId = 302,
            VariantId = 302,
            ProductNameSnapshot = "iPhone 16 Pro Max",
            SkuSnapshot = "IP16-REFUND-TEST",
            PriceSnapshot = 40000000,
            Qty = 1
        });
        context.Orders.Add(order);

        var payment = new PaymentRecord
        {
            OrderId = 302,
            Provider = provider.ProviderName,
            ProviderTxnId = "TXN-PAID-FOR-REFUND",
            Amount = 40000000,
            Status = "SUCCESS"
        };
        context.Payments.Add(payment);
        await context.SaveChangesAsync();

        // Thiết lập provider gặp lỗi mạng lần gọi đầu tiên
        provider.ShouldFailFirstTime = true;

        // Khách hủy đơn đã PAID
        var cancelResult = await service.CancelOrderAsync(order.OrderCode, "Sản phẩm không vừa ý");
        Assert.True(cancelResult.Success);
        Assert.Equal(OrderStateMachine.Statuses.Refunded, cancelResult.FinalStatus);

        // Kho được hoàn trả: 3 + 1 = 4
        var refreshedVariant = await context.ProductVariants.FindAsync(302L);
        Assert.Equal(4, refreshedVariant!.Stock);

        // Kiểm tra RefundRequest được ghi nhận trạng thái FAILED do lỗi mạng lần đầu
        var refund = await context.RefundRequests.FirstOrDefaultAsync(r => r.OrderId == order.Id);
        Assert.NotNull(refund);
        Assert.Equal("FAILED", refund.Status);
        var originalIdempotencyRefundId = refund.ProviderRefundId;
        Assert.NotNull(originalIdempotencyRefundId);

        // Retry hoàn tiền lần 2
        var retryResult = await service.ProcessRefundAsync(refund.Id);
        Assert.True(retryResult.Success);

        // RefundRequest chuyển sang COMPLETED với mã hoàn tiền từ cổng
        var completedRefund = await context.RefundRequests.FindAsync(refund.Id);
        Assert.Equal("COMPLETED", completedRefund!.Status);
        Assert.NotNull(completedRefund.ProviderRefundId);
        Assert.Contains(originalIdempotencyRefundId, completedRefund.ProviderRefundId);

        // Gọi tiếp lần 3 (retry khi đã COMPLETED): hoàn toàn an toàn (idempotent), không gọi lại cổng provider
        var retry3 = await service.ProcessRefundAsync(refund.Id);
        Assert.True(retry3.Success);
        Assert.Contains("trước đó", retry3.ErrorMessage);

        // Tổng số lần thực tế gọi cổng là 2 lần (1 lần fail, 1 lần retry thành công, lần 3 chặn sớm do đã COMPLETED)
        Assert.Equal(2, provider.RefundCallCount);
    }
}
