using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using PhoneStoreAPI.Common.Configs;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Features.Checkout.PaymentProviders;
using PhoneStoreAPI.Features.Checkout.Services;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class Task6ExpirationTests
{
    private (AppDbContext context, OrderExpirationService expirationService, PaymentWebhookHandler webhookHandler, MockQrPaymentProvider provider, DbContextOptions<AppDbContext> dbOptions)
        CreateServices(string dbName, CheckoutOptions? options = null)
    {
        var root = new Microsoft.EntityFrameworkCore.Storage.InMemoryDatabaseRoot();
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName, databaseRoot: root)
            .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new AppDbContext(dbOptions);

        var opt = Options.Create(options ?? new CheckoutOptions
        {
            ReservationTtlMinutes = 10,
            GracePeriodMinutes = 5,
            ExpireJobIntervalMinutes = 1
        });

        var sm = new OrderStateMachine(context);
        var provider = new MockQrPaymentProvider("test_secret_123");
        var webhookHandler = new PaymentWebhookHandler(context, sm, provider);
        var expirationService = new OrderExpirationService(context, sm, provider, opt, NullLogger<OrderExpirationService>.Instance);

        return (context, expirationService, webhookHandler, provider, dbOptions);
    }

    [Fact]
    public async Task Test1_Webhook_Arrives_At_9m59s_Within_Grace_Period_Order_Becomes_PAID_Not_Cancelled()
    {
        // Kịch bản 1: Khách quét QR lúc 9:59 (đã qua 9 phút 59 giây, đơn có TTL 10 phút, vẫn trong thời hạn hoặc trong grace 5 phút)
        var (context, expService, webhookHandler, provider, _) = CreateServices(Guid.NewGuid().ToString());

        var order = new Order
        {
            Id = 101,
            OrderCode = "ORD-TEST-959",
            Status = OrderStateMachine.Statuses.PendingPayment,
            Total = 25000000,
            // Giả lập đơn được tạo 9 phút 59 giây trước (hết hạn sau 1 giây nữa)
            CreatedAt = DateTime.UtcNow.AddMinutes(-9).AddSeconds(-59),
            ExpiresAt = DateTime.UtcNow.AddSeconds(1),
            IdempotencyKey = "key-959"
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // Job hết hạn chạy lúc này
        var jobResult = await expService.RunExpirationScanAsync();
        // Không xử lý đơn này vì chưa vượt quá expires_at + GRACE_PERIOD (5 phút)
        Assert.Equal(0, jobResult.ProcessedCount);

        // Webhook thanh toán đến
        var payloadObj = new QrWebhookPayload
        {
            OrderCode = order.OrderCode,
            ProviderTxnId = "TXN-959-OK",
            Amount = 25000000,
            Status = "SUCCESS"
        };
        var rawPayload = JsonSerializer.Serialize(payloadObj);
        var signature = provider.GenerateSignature(rawPayload, "test_secret_123");

        var webhookRes = await webhookHandler.ProcessQrWebhookAsync(provider.ProviderName, rawPayload, signature);
        Assert.True(webhookRes.Success);
        Assert.Equal(OrderStateMachine.Statuses.Paid, webhookRes.FinalOrderStatus);

        var refreshed = await context.Orders.FindAsync(101L);
        Assert.Equal(OrderStateMachine.Statuses.Paid, refreshed!.Status);
    }

    [Fact]
    public async Task Test2_Webhook_Arrives_After_Grace_Stock_Available_Transitions_EXPIRED_To_PAID()
    {
        // Kịch bản 2: Webhook đến sau grace period, đơn đã bị EXPIRED bởi job, nhưng kho vẫn còn đủ hàng
        var (context, expService, webhookHandler, provider, _) = CreateServices(Guid.NewGuid().ToString());

        var product = new Product { Id = 1, Name = "iPhone 16 Pro Max" };
        var variant = new ProductVariant { Id = 201, ProductId = 1, Sku = "IP16-TEST2", Color = "Black", Storage = "256GB", Price = 30000000, Stock = 5 };
        context.Products.Add(product);
        context.ProductVariants.Add(variant);

        var order = new Order
        {
            Id = 102,
            OrderCode = "ORD-LATE-STOCK-AVAILABLE",
            Status = OrderStateMachine.Statuses.Expired, // Đã hết hạn
            Total = 30000000,
            CreatedAt = DateTime.UtcNow.AddMinutes(-20),
            ExpiresAt = DateTime.UtcNow.AddMinutes(-10),
            IdempotencyKey = "key-late-available"
        };
        order.Items.Add(new OrderItem
        {
            OrderId = 102,
            VariantId = 201,
            ProductNameSnapshot = "iPhone 16 Pro Max",
            SkuSnapshot = "IP16-TEST2",
            PriceSnapshot = 30000000,
            Qty = 1
        });
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // Webhook thanh toán đến muộn
        var payloadObj = new QrWebhookPayload
        {
            OrderCode = order.OrderCode,
            ProviderTxnId = "TXN-LATE-SUCCESS-001",
            Amount = 30000000,
            Status = "SUCCESS"
        };
        var rawPayload = JsonSerializer.Serialize(payloadObj);
        var signature = provider.GenerateSignature(rawPayload, "test_secret_123");

        var webhookRes = await webhookHandler.ProcessQrWebhookAsync(provider.ProviderName, rawPayload, signature);
        Assert.True(webhookRes.Success);
        Assert.Equal(OrderStateMachine.Statuses.Paid, webhookRes.FinalOrderStatus);

        var refreshedOrder = await context.Orders.FindAsync(102L);
        Assert.Equal(OrderStateMachine.Statuses.Paid, refreshedOrder!.Status);

        // Tồn kho đã được trừ lại (5 - 1 = 4)
        var refreshedVariant = await context.ProductVariants.FindAsync(201L);
        Assert.Equal(4, refreshedVariant!.Stock);
    }

    [Fact]
    public async Task Test3_Webhook_Arrives_After_Grace_Out_Of_Stock_Transitions_PAID_OUT_OF_STOCK_And_Creates_Refund()
    {
        // Kịch bản 3: Webhook đến sau grace, người khác đã mua hết tồn kho (stock = 0)
        var (context, expService, webhookHandler, provider, _) = CreateServices(Guid.NewGuid().ToString());

        var product = new Product { Id = 1, Name = "iPhone 16 Pro Max" };
        var variant = new ProductVariant { Id = 202, ProductId = 1, Sku = "IP16-SOLD-OUT", Color = "Titan", Storage = "256GB", Price = 30000000, Stock = 0 }; // Hết sạch kho!
        context.Products.Add(product);
        context.ProductVariants.Add(variant);

        var order = new Order
        {
            Id = 103,
            OrderCode = "ORD-LATE-OUT-OF-STOCK",
            Status = OrderStateMachine.Statuses.Expired, // Đã hết hạn
            Total = 30000000,
            CreatedAt = DateTime.UtcNow.AddMinutes(-20),
            ExpiresAt = DateTime.UtcNow.AddMinutes(-10),
            IdempotencyKey = "key-late-oos"
        };
        order.Items.Add(new OrderItem
        {
            OrderId = 103,
            VariantId = 202,
            ProductNameSnapshot = "iPhone 16 Pro Max",
            SkuSnapshot = "IP16-SOLD-OUT",
            PriceSnapshot = 30000000,
            Qty = 1
        });
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // Webhook đến khi tiền đã trừ ở ngân hàng nhưng shop đã hết sạch hàng
        var payloadObj = new QrWebhookPayload
        {
            OrderCode = order.OrderCode,
            ProviderTxnId = "TXN-LATE-OOS-999",
            Amount = 30000000,
            Status = "SUCCESS"
        };
        var rawPayload = JsonSerializer.Serialize(payloadObj);
        var signature = provider.GenerateSignature(rawPayload, "test_secret_123");

        var webhookRes = await webhookHandler.ProcessQrWebhookAsync(provider.ProviderName, rawPayload, signature);
        Assert.True(webhookRes.Success);
        Assert.Equal(OrderStateMachine.Statuses.PaidOutOfStock, webhookRes.FinalOrderStatus);

        var refreshedOrder = await context.Orders.FindAsync(103L);
        Assert.Equal(OrderStateMachine.Statuses.PaidOutOfStock, refreshedOrder!.Status);

        // Có tạo yêu cầu hoàn tiền (RefundRequest)
        var refund = await context.RefundRequests.FirstOrDefaultAsync(r => r.OrderId == 103L);
        Assert.NotNull(refund);
        Assert.Equal(30000000, refund.Amount);
        Assert.Equal("PENDING", refund.Status);
        Assert.Contains("PAID_OUT_OF_STOCK", refund.Reason);
    }

    [Fact]
    public async Task Test4_Webhook_Never_Arrives_But_QueryTransaction_Reports_Paid_Job_Confirms_Order()
    {
        // Kịch bản 4: Webhook bị rớt mạng không bao giờ đến, nhưng queryTransaction cổng báo đã thanh toán
        var (context, expService, webhookHandler, provider, _) = CreateServices(Guid.NewGuid().ToString());

        var order = new Order
        {
            Id = 104,
            OrderCode = "ORD-QUERY-RECOVER",
            Status = OrderStateMachine.Statuses.PendingPayment,
            Total = 15000000,
            // Đã quá hạn 10 phút + grace period 5 phút
            CreatedAt = DateTime.UtcNow.AddMinutes(-20),
            ExpiresAt = DateTime.UtcNow.AddMinutes(-6),
            IdempotencyKey = "key-query-recover"
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // Giả lập cổng thanh toán ghi nhận giao dịch thành công khi query
        provider.RegisterMockTransaction(order.OrderCode, isPaid: true, amount: 15000000, providerTxnId: "TXN-RECOVERED-FROM-GATEWAY");

        // Job hết hạn chạy
        var result = await expService.RunExpirationScanAsync();

        Assert.Equal(1, result.ProcessedCount);
        Assert.Equal(1, result.ConfirmedPaidCount);
        Assert.Equal(0, result.ExpiredCount);

        var refreshed = await context.Orders.FindAsync(104L);
        Assert.Equal(OrderStateMachine.Statuses.Paid, refreshed!.Status);

        var payment = await context.Payments.FirstOrDefaultAsync(p => p.OrderId == 104L);
        Assert.NotNull(payment);
        Assert.Equal("TXN-RECOVERED-FROM-GATEWAY", payment.ProviderTxnId);
        Assert.Equal("SUCCESS", payment.Status);
    }

    [Fact]
    public async Task Test5_ExpirationJob_And_Webhook_Run_In_Parallel_Consistent_State_And_Stock()
    {
        // Kịch bản 5: Job hết hạn và webhook cùng tranh chấp 1 đơn hàng song song
        var dbName = Guid.NewGuid().ToString();
        var (setupContext, _, _, provider, dbOptions) = CreateServices(dbName);

        var product = new Product { Id = 1, Name = "iPhone 16 Pro Max" };
        var variant = new ProductVariant { Id = 205, ProductId = 1, Sku = "IP16-PARALLEL", Color = "Natural", Storage = "256GB", Price = 30000000, Stock = 2 };
        setupContext.Products.Add(product);
        setupContext.ProductVariants.Add(variant);

        var order = new Order
        {
            Id = 105,
            OrderCode = "ORD-PARALLEL-RACE",
            Status = OrderStateMachine.Statuses.PendingPayment,
            Total = 30000000,
            CreatedAt = DateTime.UtcNow.AddMinutes(-20),
            ExpiresAt = DateTime.UtcNow.AddMinutes(-6), // Quá hạn
            IdempotencyKey = "key-parallel-race"
        };
        order.Items.Add(new OrderItem
        {
            OrderId = 105,
            VariantId = 205,
            ProductNameSnapshot = "iPhone 16 Pro Max",
            SkuSnapshot = "IP16-PARALLEL",
            PriceSnapshot = 30000000,
            Qty = 1
        });
        setupContext.Orders.Add(order);
        await setupContext.SaveChangesAsync();

        var payloadObj = new QrWebhookPayload
        {
            OrderCode = order.OrderCode,
            ProviderTxnId = "TXN-PARALLEL-105",
            Amount = 30000000,
            Status = "SUCCESS"
        };
        var rawPayload = JsonSerializer.Serialize(payloadObj);
        var signature = provider.GenerateSignature(rawPayload, "test_secret_123");

        var opt = Options.Create(new CheckoutOptions { ReservationTtlMinutes = 10, GracePeriodMinutes = 5 });

        // Chạy song song: Job và Webhook cùng lúc
        var taskJob = Task.Run(async () =>
        {
            using var ctx = new AppDbContext(dbOptions);
            var sm = new OrderStateMachine(ctx);
            var expSvc = new OrderExpirationService(ctx, sm, provider, opt, NullLogger<OrderExpirationService>.Instance);
            await expSvc.RunExpirationScanAsync();
        });

        var taskWebhook = Task.Run(async () =>
        {
            using var ctx = new AppDbContext(dbOptions);
            var sm = new OrderStateMachine(ctx);
            var handler = new PaymentWebhookHandler(ctx, sm, provider);
            await handler.ProcessQrWebhookAsync(provider.ProviderName, rawPayload, signature);
        });

        await Task.WhenAll(taskJob, taskWebhook);

        // Trạng thái cuối cùng phải là nhất quán: hoặc PAID (nếu webhook thắng hoặc late webhook giữ lại được hàng)
        using var verifyContext = new AppDbContext(dbOptions);
        var finalOrder = await verifyContext.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == 105L);
        Assert.NotNull(finalOrder);
        Assert.True(finalOrder.Status == OrderStateMachine.Statuses.Paid || finalOrder.Status == OrderStateMachine.Statuses.Expired || finalOrder.Status == OrderStateMachine.Statuses.PaidOutOfStock);

        // Tồn kho không bao giờ âm
        var finalVariant = await verifyContext.ProductVariants.AsNoTracking().FirstOrDefaultAsync(v => v.Id == 205L);
        Assert.NotNull(finalVariant);
        Assert.True(finalVariant.Stock >= 0);
        // Ban đầu là 2, đơn 105 giữ 1:
        // Nếu cuối cùng là PAID -> stock = 1 hoặc 2 (tùy thứ tự trả rồi trừ lại)
        Assert.InRange(finalVariant.Stock, 1, 3);
    }
}
