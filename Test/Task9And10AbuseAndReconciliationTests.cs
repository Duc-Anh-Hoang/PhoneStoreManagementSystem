using System.Collections.Concurrent;
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

public class Task9And10AbuseAndReconciliationTests
{
    private (AppDbContext context, ReconciliationService reconService, DbContextOptions<AppDbContext> dbOptions) CreateServices(string dbName)
    {
        var root = new Microsoft.EntityFrameworkCore.Storage.InMemoryDatabaseRoot();
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName, databaseRoot: root)
            .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new AppDbContext(dbOptions);

        var provider = new MockQrPaymentProvider("test");
        var reconService = new ReconciliationService(context, provider, NullLogger<ReconciliationService>.Instance);

        return (context, reconService, dbOptions);
    }

    [Fact]
    public async Task Reconciliation_Detects_Discrepancies_Correctly()
    {
        var (context, reconService, _) = CreateServices(Guid.NewGuid().ToString());
        var today = DateTime.UtcNow;

        // Đơn 1: PAID nhưng không có Payment nào
        var order1 = new Order
        {
            Id = 501,
            OrderCode = "ORD-RECON-1",
            Status = OrderStateMachine.Statuses.Paid,
            Total = 20000000,
            CreatedAt = today,
            IdempotencyKey = "key-recon-1"
        };

        // Đơn 2: CANCELLED nhưng có Payment SUCCESS (tiền vào mà đơn bị hủy, chưa hoàn)
        var order2 = new Order
        {
            Id = 502,
            OrderCode = "ORD-RECON-2",
            Status = OrderStateMachine.Statuses.Cancelled,
            Total = 30000000,
            CreatedAt = today,
            IdempotencyKey = "key-recon-2"
        };
        var payment2 = new PaymentRecord
        {
            OrderId = 502,
            Provider = "MOCK_QR",
            ProviderTxnId = "TXN-PAID-BUT-CANCELLED",
            Amount = 30000000,
            Status = "SUCCESS",
            CreatedAt = today
        };

        // Đơn 3: AMOUNT_MISMATCH
        var order3 = new Order
        {
            Id = 503,
            OrderCode = "ORD-RECON-3",
            Status = OrderStateMachine.Statuses.PendingPayment,
            Total = 10000000,
            CreatedAt = today,
            IdempotencyKey = "key-recon-3"
        };
        var payment3 = new PaymentRecord
        {
            OrderId = 503,
            Provider = "MOCK_QR",
            ProviderTxnId = "TXN-MISMATCH-103",
            Amount = 5000000, // Khách chuyển 5tr trong khi đơn 10tr
            Status = "AMOUNT_MISMATCH",
            CreatedAt = today
        };

        context.Orders.AddRange(order1, order2, order3);
        context.Payments.AddRange(payment2, payment3);
        await context.SaveChangesAsync();

        var report = await reconService.ReconcileDailyTransactionsAsync(today);

        Assert.Equal(3, report.TotalOrdersChecked);
        Assert.Equal(3, report.DiscrepancyCount);

        Assert.Contains(report.Discrepancies, d => d.Type == "ORDER_PAID_NO_PAYMENT" && d.OrderCode == "ORD-RECON-1");
        Assert.Contains(report.Discrepancies, d => d.Type == "MONEY_RECEIVED_ORDER_CANCELLED" && d.OrderCode == "ORD-RECON-2");
        Assert.Contains(report.Discrepancies, d => d.Type == "AMOUNT_MISMATCH" && d.OrderCode == "ORD-RECON-3");
    }

    [Fact]
    public async Task HighLoad_FlashSale_Scenario_1000_Buyers_For_10_Items_Exactly_10_Orders_Stock_Zero()
    {
        // Kịch bản Flash sale Task 10: 1000 người mua 10 chiếc -> đúng 10 đơn thành công, stock = 0
        var dbName = Guid.NewGuid().ToString();
        var root = new Microsoft.EntityFrameworkCore.Storage.InMemoryDatabaseRoot();
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName, databaseRoot: root)
            .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var setupContext = new AppDbContext(dbOptions);

        var product = new Product { Id = 1, Name = "iPhone 16 Flash Sale" };
        var variant = new ProductVariant { Id = 888, ProductId = 1, Sku = "IP16-FLASH-SALE", Color = "Black", Storage = "128GB", Price = 20000000, Stock = 10 };
        setupContext.Products.Add(product);
        setupContext.ProductVariants.Add(variant);
        await setupContext.SaveChangesAsync();

        var opt = Options.Create(new CheckoutOptions { ReservationTtlMinutes = 10, MaxQtyPerOrder = 1 });

        var successOrders = new ConcurrentBag<OrderDTO>();
        var outOfStockCount = 0;

        var tasks = Enumerable.Range(1, 1000).Select(async i =>
        {
            using var threadContext = new AppDbContext(dbOptions);
            var orderService = new OrderService(threadContext, opt);

            var req = new CreateOrderRequest
            {
                RecipientName = $"Flash Buyer {i}",
                RecipientPhone = $"090000{i:D4}",
                ShippingAddress = $"Street {i}",
                PaymentMethod = "QR_PAYMENT",
                Items = new List<CreateOrderItemRequest>
                {
                    new() { VariantId = 888, Qty = 1 }
                }
            };

            try
            {
                var order = await orderService.CreateOrderAsync($"key-flash-{i}", req);
                successOrders.Add(order);
            }
            catch (InvalidOperationException)
            {
                Interlocked.Increment(ref outOfStockCount);
            }
        });

        await Task.WhenAll(tasks);

        // ĐÚNG 10 đơn thành công
        Assert.Equal(10, successOrders.Count);

        // 990 đơn bị từ chối do hết hàng
        Assert.Equal(990, outOfStockCount);

        // Stock kết thúc ở 0, KHÔNG âm
        using var verifyContext = new AppDbContext(dbOptions);
        var finalVariant = await verifyContext.ProductVariants.AsNoTracking().FirstOrDefaultAsync(v => v.Id == 888L);
        Assert.NotNull(finalVariant);
        Assert.Equal(0, finalVariant.Stock);
    }
}
