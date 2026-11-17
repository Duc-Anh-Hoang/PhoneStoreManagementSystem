using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhoneStoreAPI.Common.Configs;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Features.Checkout.Services;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class Task3OrderTests
{
    private (AppDbContext context, OrderService orderService, DbContextOptions<AppDbContext> dbOptions) CreateService(string dbName)
    {
        var root = new Microsoft.EntityFrameworkCore.Storage.InMemoryDatabaseRoot();
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName, databaseRoot: root)
            .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new AppDbContext(dbOptions);

        var p = new Product { Id = 1, Name = "iPhone 16 Pro Max" };
        context.Products.Add(p);
        context.ProductVariants.AddRange(
            new ProductVariant { Id = 1, ProductId = 1, Sku = "IP16PM-LAST-1", Color = "Desert", Storage = "256GB", Price = 34990000, Stock = 1 },
            new ProductVariant { Id = 2, ProductId = 1, Sku = "IP16PM-PLENTY", Color = "Natural", Storage = "512GB", Price = 40990000, Stock = 10 },
            new ProductVariant { Id = 3, ProductId = 1, Sku = "IP16PM-EMPTY", Color = "Black", Storage = "1TB", Price = 46990000, Stock = 0 }
        );
        context.SaveChanges();

        var opt = Options.Create(new CheckoutOptions { ReservationTtlMinutes = 10, MaxQtyPerOrder = 2 });
        var orderService = new OrderService(context, opt);

        return (context, orderService, dbOptions);
    }

    [Fact]
    public async Task Same_IdempotencyKey_Sent_Twice_Returns_Same_Order()
    {
        var (context, service, _) = CreateService(Guid.NewGuid().ToString());

        var req = new CreateOrderRequest
        {
            RecipientName = "Nguyễn Văn A",
            RecipientPhone = "0901234567",
            ShippingAddress = "123 Lê Lợi, Q1, TP.HCM",
            PaymentMethod = "QR_PAYMENT",
            Items = new List<CreateOrderItemRequest>
            {
                new() { VariantId = 2, Qty = 1 }
            }
        };

        var key = "idempotency-key-unique-001";
        var order1 = await service.CreateOrderAsync(key, req);
        var order2 = await service.CreateOrderAsync(key, req);

        Assert.Equal(order1.Id, order2.Id);
        Assert.Equal(order1.OrderCode, order2.OrderCode);

        // Kiểm tra database chỉ có đúng 1 đơn hàng
        var orderCount = await context.Orders.CountAsync();
        Assert.Equal(1, orderCount);

        // Stock chỉ bị trừ 1 lần (10 - 1 = 9)
        var variant2 = await context.ProductVariants.FindAsync(2L);
        Assert.Equal(9, variant2!.Stock);
    }

    [Fact]
    public async Task One_Item_Out_Of_Stock_Rolls_Back_Entire_Order_And_Keeps_Other_Stocks_Unchanged()
    {
        var (context, service, _) = CreateService(Guid.NewGuid().ToString());

        // Đơn hàng gồm 2 item: variant 2 (còn 10 cái) và variant 3 (còn 0 cái - hết hàng)
        var req = new CreateOrderRequest
        {
            RecipientName = "Trần B",
            RecipientPhone = "0909999999",
            ShippingAddress = "456 Nguyễn Huệ, Q1",
            PaymentMethod = "QR_PAYMENT",
            Items = new List<CreateOrderItemRequest>
            {
                new() { VariantId = 2, Qty = 1 },
                new() { VariantId = 3, Qty = 1 } // Sẽ gây lỗi hết hàng
            }
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateOrderAsync("idempotency-key-rollback-test", req));

        Assert.Contains("hết hàng", ex.Message);

        // Kiểm tra toàn bộ đơn bị rollback: không có đơn nào trong DB
        var orderCount = await context.Orders.CountAsync();
        Assert.Equal(0, orderCount);

        // Stock của variant 2 vẫn nguyên vẹn là 10 (không bị trừ)
        var variant2 = await context.ProductVariants.FindAsync(2L);
        Assert.Equal(10, variant2!.Stock);
    }

    [Fact]
    public async Task Concurrent_100_Requests_For_Last_1_Item_Results_In_Exactly_1_Success_And_Stock_0()
    {
        var dbName = Guid.NewGuid().ToString();
        var (setupContext, _, dbOptions) = CreateService(dbName);
        var opt = Options.Create(new CheckoutOptions { ReservationTtlMinutes = 10, MaxQtyPerOrder = 2 });

        var successOrders = new ConcurrentBag<OrderDTO>();
        var outOfStockExceptions = new ConcurrentBag<Exception>();

        var tasks = Enumerable.Range(1, 100).Select(async i =>
        {
            using var threadContext = new AppDbContext(dbOptions);
            var threadService = new OrderService(threadContext, opt);

            var req = new CreateOrderRequest
            {
                RecipientName = $"Buyer {i}",
                RecipientPhone = $"09000000{i:D2}",
                ShippingAddress = $"Address {i}",
                PaymentMethod = "QR_PAYMENT",
                Items = new List<CreateOrderItemRequest>
                {
                    new() { VariantId = 1, Qty = 1 } // Chiếc duy nhất còn lại
                }
            };

            try
            {
                var order = await threadService.CreateOrderAsync($"key-buyer-{i}", req);
                successOrders.Add(order);
            }
            catch (InvalidOperationException ex)
            {
                outOfStockExceptions.Add(ex);
            }
        });

        await Task.WhenAll(tasks);

        // ĐÚNG 1 đơn thành công
        Assert.Single(successOrders);

        // 99 đơn bị từ chối do hết hàng
        Assert.Equal(99, outOfStockExceptions.Count);

        // Stock kết thúc ở 0, KHÔNG âm
        using var verifyContext = new AppDbContext(dbOptions);
        var finalVariant = await verifyContext.ProductVariants.AsNoTracking().FirstOrDefaultAsync(v => v.Id == 1L);
        Assert.NotNull(finalVariant);
        Assert.Equal(0, finalVariant.Stock);
    }
}
