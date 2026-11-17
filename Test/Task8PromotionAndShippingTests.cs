using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhoneStoreAPI.Common.Configs;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Features.Checkout.PaymentProviders;
using PhoneStoreAPI.Features.Checkout.Services;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class Task8PromotionAndShippingTests
{
    private (AppDbContext context, OrderService orderService, RefundAndCancellationService cancelService, DbContextOptions<AppDbContext> dbOptions)
        CreateServices(string dbName)
    {
        var root = new Microsoft.EntityFrameworkCore.Storage.InMemoryDatabaseRoot();
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName, databaseRoot: root)
            .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new AppDbContext(dbOptions);

        var p = new Product { Id = 1, Name = "iPhone 16 Pro Max" };
        var giftProduct = new Product { Id = 2, Name = "Tai nghe EarPods tặng kèm" };
        context.Products.AddRange(p, giftProduct);

        context.ProductVariants.AddRange(
            new ProductVariant { Id = 1, ProductId = 1, Sku = "IP16PM-MAIN", Color = "Desert", Storage = "256GB", Price = 30000000, Stock = 50 },
            new ProductVariant { Id = 2, ProductId = 2, Sku = "GIFT-EARPODS", Color = "White", Storage = "Standard", Price = 0, Stock = 10 } // Quà tặng kèm giá 0
        );

        // Voucher giảm 500k cho đơn từ 20tr, giới hạn tối đa 3 lượt dùng
        context.Vouchers.Add(new Voucher
        {
            Id = 1,
            Code = "GIAM500K",
            DiscountAmount = 500000,
            MinOrderTotal = 20000000,
            MaxUses = 3,
            UsedCount = 0,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        context.SaveChanges();

        var opt = Options.Create(new CheckoutOptions { ReservationTtlMinutes = 10, MaxQtyPerOrder = 2 });
        var orderService = new OrderService(context, opt);
        var sm = new OrderStateMachine(context);
        var provider = new MockQrPaymentProvider("test");
        var cancelService = new RefundAndCancellationService(context, sm, provider);

        return (context, orderService, cancelService, dbOptions);
    }

    [Fact]
    public async Task Concurrent_Voucher_Usage_Exceeding_Max_Uses_Allows_Only_Allowed_Count()
    {
        // 10 request đồng thời dùng voucher GIAM500K (chỉ có 3 lượt)
        var dbName = Guid.NewGuid().ToString();
        var (setupContext, _, _, dbOptions) = CreateServices(dbName);
        var opt = Options.Create(new CheckoutOptions { ReservationTtlMinutes = 10, MaxQtyPerOrder = 2 });

        var successOrders = new ConcurrentBag<OrderDTO>();
        var rejectedExceptions = new ConcurrentBag<Exception>();

        var tasks = Enumerable.Range(1, 10).Select(async i =>
        {
            using var threadContext = new AppDbContext(dbOptions);
            var threadOrderService = new OrderService(threadContext, opt);

            var req = new CreateOrderRequest
            {
                RecipientName = $"Voucher User {i}",
                RecipientPhone = $"09110000{i:D2}",
                ShippingAddress = $"Address {i}",
                PaymentMethod = "QR_PAYMENT",
                VoucherCode = "GIAM500K",
                Items = new List<CreateOrderItemRequest>
                {
                    new() { VariantId = 1, Qty = 1 }
                }
            };

            try
            {
                var order = await threadOrderService.CreateOrderAsync($"key-voucher-{i}", req);
                successOrders.Add(order);
            }
            catch (Exception ex)
            {
                rejectedExceptions.Add(ex);
            }
        });

        await Task.WhenAll(tasks);

        // ĐÚNG 3 đơn thành công
        Assert.Equal(3, successOrders.Count);

        // 7 đơn bị từ chối
        Assert.Equal(7, rejectedExceptions.Count);

        // Voucher used_count = 3
        using var verifyContext = new AppDbContext(dbOptions);
        var voucher = await verifyContext.Vouchers.AsNoTracking().FirstOrDefaultAsync(v => v.Code == "GIAM500K");
        Assert.NotNull(voucher);
        Assert.Equal(3, voucher.UsedCount);

        // Đơn thành công có tổng tiền được giảm 500.000đ (30.000.000 - 500.000 = 29.500.000)
        Assert.All(successOrders, o => Assert.Equal(29500000, o.Total));
    }

    [Fact]
    public async Task Free_Gift_Item_Has_Price_Zero_And_Deducts_Stock()
    {
        var (context, orderService, _, _) = CreateServices(Guid.NewGuid().ToString());

        var req = new CreateOrderRequest
        {
            RecipientName = "Khách Nhận Quà",
            RecipientPhone = "0988776655",
            ShippingAddress = "789 Điện Biên Phủ, Bình Thạnh",
            PaymentMethod = "QR_PAYMENT",
            Items = new List<CreateOrderItemRequest>
            {
                new() { VariantId = 1, Qty = 1 }, // Điện thoại chính
                new() { VariantId = 2, Qty = 1 }  // Quà tặng kèm giá 0đ
            }
        };

        var order = await orderService.CreateOrderAsync("key-gift-test-1", req);
        Assert.Equal(2, order.Items.Count);

        var giftItem = order.Items.FirstOrDefault(i => i.VariantId == 2);
        Assert.NotNull(giftItem);
        Assert.Equal(0, giftItem.Price);
        Assert.Equal(0, giftItem.Subtotal);

        // Kho của quà tặng bị trừ 1 (10 - 1 = 9)
        var giftVariant = await context.ProductVariants.FindAsync(2L);
        Assert.Equal(9, giftVariant!.Stock);
    }

    [Fact]
    public async Task Cancelling_Order_With_Voucher_Restores_Voucher_Usage_Count()
    {
        var (context, orderService, cancelService, _) = CreateServices(Guid.NewGuid().ToString());

        var req = new CreateOrderRequest
        {
            RecipientName = "User Voucher Test",
            RecipientPhone = "0933333333",
            ShippingAddress = "Hà Nội",
            PaymentMethod = "QR_PAYMENT",
            VoucherCode = "GIAM500K",
            Items = new List<CreateOrderItemRequest>
            {
                new() { VariantId = 1, Qty = 1 }
            }
        };

        var order = await orderService.CreateOrderAsync("key-voucher-cancel-test", req);
        var voucherAfterCreate = await context.Vouchers.FindAsync(1L);
        Assert.Equal(1, voucherAfterCreate!.UsedCount);

        // Hủy đơn
        var cancelResult = await cancelService.CancelOrderAsync(order.OrderCode, "Khách không lấy nữa");
        Assert.True(cancelResult.Success);

        // Lượt dùng voucher được hoàn lại (1 - 1 = 0)
        var voucherAfterCancel = await context.Vouchers.FindAsync(1L);
        Assert.Equal(0, voucherAfterCancel!.UsedCount);
    }
}
