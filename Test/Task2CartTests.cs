using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhoneStoreAPI.Common.Configs;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Features.Checkout.Services;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class Task2CartTests
{
    private (AppDbContext context, CartService cartService) CreateService(string dbName, CheckoutOptions? options = null)
    {
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new AppDbContext(dbOptions);

        var product = new Product { Id = 1, Name = "iPhone 16 Pro Max" };
        context.Products.Add(product);
        context.ProductVariants.AddRange(
            new ProductVariant { Id = 101, ProductId = 1, Sku = "IP16PM-256", Color = "Desert", Storage = "256GB", Price = 34990000, Stock = 5 },
            new ProductVariant { Id = 102, ProductId = 1, Sku = "IP16PM-512", Color = "Natural", Storage = "512GB", Price = 40990000, Stock = 0 } // Hết hàng
        );
        context.SaveChanges();

        var opt = Options.Create(options ?? new CheckoutOptions { MaxQtyPerOrder = 2 });
        var cartService = new CartService(context, opt);

        return (context, cartService);
    }

    [Fact]
    public async Task AddItem_Ignores_Fake_Client_Price_And_Calculates_From_Database()
    {
        var (context, service) = CreateService(Guid.NewGuid().ToString());

        // Client chỉ gửi variant_id và qty
        var cart = await service.AddItemAsync("guest-token-1", null, new AddCartItemRequest
        {
            VariantId = 101,
            Qty = 1
        });

        Assert.Single(cart.Items);
        var item = cart.Items.First();
        Assert.Equal(34990000, item.Price);
        Assert.Equal(34990000, cart.Total);
    }

    [Fact]
    public async Task MergeCart_Does_Not_Duplicate_Items_And_Respects_MaxQty()
    {
        var (context, service) = CreateService(Guid.NewGuid().ToString(), new CheckoutOptions { MaxQtyPerOrder = 2 });

        // Guest thêm variant 101 với qty = 1
        await service.AddItemAsync("guest-token-abc", null, new AddCartItemRequest
        {
            VariantId = 101,
            Qty = 1
        });

        // User đã có variant 101 trong giỏ với qty = 1
        await service.AddItemAsync(null, "user-123", new AddCartItemRequest
        {
            VariantId = 101,
            Qty = 1
        });

        // Merge guest cart vào user cart
        var mergedCart = await service.MergeCartAsync("guest-token-abc", "user-123");

        Assert.Single(mergedCart.Items); // Không bị nhân đôi thành 2 dòng
        var item = mergedCart.Items.First();
        Assert.Equal(101, item.VariantId);
        Assert.Equal(2, item.Qty); // 1 + 1 = 2 (tối đa MaxQtyPerOrder)

        // Kiểm tra guest cart cũ đã bị xoá
        var guestCart = await context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.CartToken == "guest-token-abc");
        Assert.Null(guestCart);
    }

    [Fact]
    public async Task Out_Of_Stock_Item_Has_Warning()
    {
        var (context, service) = CreateService(Guid.NewGuid().ToString());

        // Thêm variant 102 (stock = 0)
        var cart = await service.AddItemAsync("guest-token-2", null, new AddCartItemRequest
        {
            VariantId = 102,
            Qty = 1
        });

        Assert.NotEmpty(cart.Warnings);
        var warning = cart.Warnings.FirstOrDefault(w => w.VariantId == 102 && w.Code == "OUT_OF_STOCK");
        Assert.NotNull(warning);
        Assert.Contains("hết hàng", warning.Message);
    }

    [Fact]
    public async Task Exceeding_Max_Qty_Is_Capped_At_MaxQtyPerOrder()
    {
        var (context, service) = CreateService(Guid.NewGuid().ToString(), new CheckoutOptions { MaxQtyPerOrder = 2 });

        // Thêm qty = 5 trong khi MaxQtyPerOrder = 2
        var cart = await service.AddItemAsync("guest-token-3", null, new AddCartItemRequest
        {
            VariantId = 101,
            Qty = 5
        });

        Assert.Single(cart.Items);
        Assert.Equal(2, cart.Items.First().Qty);
        Assert.Equal(2 * 34990000, cart.Total);
    }
}
