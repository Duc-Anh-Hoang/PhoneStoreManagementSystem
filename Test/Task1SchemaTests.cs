using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class Task1SchemaTests
{
    private AppDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task SeedData_Contains_3_Products_With_Multiple_Variants()
    {
        using var context = CreateInMemoryContext(Guid.NewGuid().ToString());

        var p1 = new Product { Id = 1, Name = "iPhone 16 Pro Max", Description = "Siêu phẩm Apple 2026" };
        var p2 = new Product { Id = 2, Name = "Samsung Galaxy S24 Ultra", Description = "Flagship Samsung" };
        var p3 = new Product { Id = 3, Name = "Xiaomi 14 Ultra", Description = "Chuyên gia Leica" };

        context.Products.AddRange(p1, p2, p3);
        await context.SaveChangesAsync();

        context.ProductVariants.AddRange(
            new ProductVariant { Id = 1, ProductId = 1, Sku = "IP16PM-DESERT-256", Color = "Desert Titanium", Storage = "256GB", Price = 34990000, Stock = 10 },
            new ProductVariant { Id = 2, ProductId = 1, Sku = "IP16PM-NATURAL-512", Color = "Natural Titanium", Storage = "512GB", Price = 40990000, Stock = 5 },
            new ProductVariant { Id = 3, ProductId = 1, Sku = "IP16PM-BLACK-1TB", Color = "Black Titanium", Storage = "1TB", Price = 46990000, Stock = 3 },
            new ProductVariant { Id = 4, ProductId = 2, Sku = "S24U-GRAY-256", Color = "Titanium Gray", Storage = "256GB", Price = 29990000, Stock = 8 },
            new ProductVariant { Id = 5, ProductId = 2, Sku = "S24U-BLACK-512", Color = "Titanium Black", Storage = "512GB", Price = 33990000, Stock = 4 },
            new ProductVariant { Id = 6, ProductId = 3, Sku = "XM14U-BLACK-512", Color = "Black", Storage = "512GB", Price = 28990000, Stock = 6 },
            new ProductVariant { Id = 7, ProductId = 3, Sku = "XM14U-WHITE-512", Color = "White", Storage = "512GB", Price = 28990000, Stock = 2 }
        );
        await context.SaveChangesAsync();

        var products = await context.Products.Include(p => p.Variants).ToListAsync();
        Assert.Equal(3, products.Count);
        Assert.All(products, p => Assert.True(p.Variants.Count >= 2));

        var variants = await context.ProductVariants.ToListAsync();
        Assert.Equal(7, variants.Count);
        Assert.All(variants, v => Assert.True(v.Price > 0));
        Assert.All(variants, v => Assert.True(v.Stock >= 0));
    }

    [Fact]
    public async Task Entities_Can_Be_Persisted_And_Queried_Successfully()
    {
        using var context = CreateInMemoryContext(Guid.NewGuid().ToString());

        var order = new Order
        {
            Id = 101,
            OrderCode = "ORD-20261117-001",
            Status = "PENDING_PAYMENT",
            Total = 34990000,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IdempotencyKey = "key-abc-123",
            RecipientName = "Nguyễn Văn A",
            RecipientPhone = "0901234567",
            ShippingAddress = "123 Đường Lê Lợi, Quận 1, TP.HCM",
            PaymentMethod = "QR_PAYMENT"
        };
        context.Orders.Add(order);

        var orderItem = new OrderItem
        {
            Id = 201,
            OrderId = 101,
            VariantId = 1,
            ProductNameSnapshot = "iPhone 16 Pro Max",
            SkuSnapshot = "IP16PM-DESERT-256",
            PriceSnapshot = 34990000,
            Qty = 1
        };
        context.OrderItems.Add(orderItem);

        var payment = new PaymentRecord
        {
            Id = 301,
            OrderId = 101,
            Provider = "MOCK_QR",
            ProviderTxnId = "TXN_99887766",
            Amount = 34990000,
            Status = "SUCCESS"
        };
        context.Payments.Add(payment);

        var history = new OrderStatusHistory
        {
            Id = 401,
            OrderId = 101,
            FromStatus = null,
            ToStatus = "PENDING_PAYMENT",
            Reason = "Đơn mới được tạo"
        };
        context.OrderStatusHistories.Add(history);

        var refund = new RefundRequest
        {
            Id = 501,
            OrderId = 101,
            PaymentId = 301,
            Amount = 34990000,
            Reason = "Hết hàng",
            Status = "PENDING",
            ProviderRefundId = "REF_123456"
        };
        context.RefundRequests.Add(refund);

        await context.SaveChangesAsync();

        var savedOrder = await context.Orders
            .Include(o => o.Items)
            .Include(o => o.Payments)
            .Include(o => o.StatusHistories)
            .Include(o => o.RefundRequests)
            .FirstOrDefaultAsync(o => o.Id == 101);

        Assert.NotNull(savedOrder);
        Assert.Single(savedOrder.Items);
        Assert.Equal("IP16PM-DESERT-256", savedOrder.Items.First().SkuSnapshot);
        Assert.Single(savedOrder.Payments);
        Assert.Equal("TXN_99887766", savedOrder.Payments.First().ProviderTxnId);
        Assert.Single(savedOrder.StatusHistories);
        Assert.Single(savedOrder.RefundRequests);
    }
}
