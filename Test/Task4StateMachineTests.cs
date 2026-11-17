using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.Services;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class Task4StateMachineTests
{
    private (AppDbContext context, OrderStateMachine stateMachine, DbContextOptions<AppDbContext> dbOptions) CreateStateMachine(string dbName)
    {
        var root = new Microsoft.EntityFrameworkCore.Storage.InMemoryDatabaseRoot();
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName, databaseRoot: root)
            .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new AppDbContext(dbOptions);

        var sm = new OrderStateMachine(context);
        return (context, sm, dbOptions);
    }

    [Fact]
    public async Task Invalid_State_Transitions_Are_Rejected()
    {
        var (context, sm, _) = CreateStateMachine(Guid.NewGuid().ToString());

        var order = new Order
        {
            Id = 1,
            OrderCode = "ORD-001",
            Status = OrderStateMachine.Statuses.PendingPayment,
            Total = 1000000,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IdempotencyKey = "key-001"
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // PENDING_PAYMENT không thể chuyển thẳng sang COMPLETED hoặc SHIPPING
        var res1 = await sm.TransitionAsync(order.Id, OrderStateMachine.Statuses.PendingPayment, OrderStateMachine.Statuses.Completed);
        Assert.False(res1.Success);
        Assert.Contains("không hợp lệ", res1.ErrorMessage);

        var res2 = await sm.TransitionAsync(order.Id, OrderStateMachine.Statuses.PendingPayment, OrderStateMachine.Statuses.Shipping);
        Assert.False(res2.Success);

        // Đơn vẫn ở trạng thái PENDING_PAYMENT
        var refreshed = await context.Orders.FindAsync(order.Id);
        Assert.Equal(OrderStateMachine.Statuses.PendingPayment, refreshed!.Status);
    }

    [Fact]
    public async Task Valid_Transitions_Succeed_And_Record_History()
    {
        var (context, sm, _) = CreateStateMachine(Guid.NewGuid().ToString());

        var order = new Order
        {
            Id = 2,
            OrderCode = "ORD-002",
            Status = OrderStateMachine.Statuses.PendingPayment,
            Total = 2000000,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IdempotencyKey = "key-002"
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // PENDING_PAYMENT -> PAID
        var step1 = await sm.TransitionAsync(order.Id, OrderStateMachine.Statuses.PendingPayment, OrderStateMachine.Statuses.Paid, "Thanh toán thành công");
        Assert.True(step1.Success);

        // PAID -> CONFIRMED
        var step2 = await sm.TransitionAsync(order.Id, OrderStateMachine.Statuses.Paid, OrderStateMachine.Statuses.Confirmed, "Đã xác nhận");
        Assert.True(step2.Success);

        // CONFIRMED -> SHIPPING
        var step3 = await sm.TransitionAsync(order.Id, OrderStateMachine.Statuses.Confirmed, OrderStateMachine.Statuses.Shipping, "Đang giao hàng");
        Assert.True(step3.Success);

        // SHIPPING -> COMPLETED
        var step4 = await sm.TransitionAsync(order.Id, OrderStateMachine.Statuses.Shipping, OrderStateMachine.Statuses.Completed, "Giao thành công");
        Assert.True(step4.Success);

        var finalOrder = await context.Orders.FindAsync(order.Id);
        Assert.Equal(OrderStateMachine.Statuses.Completed, finalOrder!.Status);

        var histories = await context.OrderStatusHistories.Where(h => h.OrderId == order.Id).ToListAsync();
        Assert.Equal(4, histories.Count);
    }

    [Fact]
    public async Task Concurrent_Transitions_On_Same_Order_Only_One_Wins()
    {
        var dbName = Guid.NewGuid().ToString();
        var (setupContext, _, dbOptions) = CreateStateMachine(dbName);

        var order = new Order
        {
            Id = 3,
            OrderCode = "ORD-003",
            Status = OrderStateMachine.Statuses.PendingPayment,
            Total = 3000000,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IdempotencyKey = "key-003"
        };
        setupContext.Orders.Add(order);
        await setupContext.SaveChangesAsync();

        // 2 tiến trình đồng thời cố gắng chuyển trạng thái:
        // Tiến trình 1 muốn PENDING_PAYMENT -> PAID
        // Tiến trình 2 muốn PENDING_PAYMENT -> EXPIRED
        var results = new ConcurrentBag<(string Target, bool Success)>();

        var task1 = Task.Run(async () =>
        {
            using var ctx = new AppDbContext(dbOptions);
            var sm = new OrderStateMachine(ctx);
            var res = await sm.TransitionAsync(3, OrderStateMachine.Statuses.PendingPayment, OrderStateMachine.Statuses.Paid);
            results.Add((OrderStateMachine.Statuses.Paid, res.Success));
        });

        var task2 = Task.Run(async () =>
        {
            using var ctx = new AppDbContext(dbOptions);
            var sm = new OrderStateMachine(ctx);
            var res = await sm.TransitionAsync(3, OrderStateMachine.Statuses.PendingPayment, OrderStateMachine.Statuses.Expired);
            results.Add((OrderStateMachine.Statuses.Expired, res.Success));
        });

        await Task.WhenAll(task1, task2);

        // Đúng 1 tiến trình thắng, 1 tiến trình thua
        var winnerCount = results.Count(r => r.Success);
        var loserCount = results.Count(r => !r.Success);

        Assert.Equal(1, winnerCount);
        Assert.Equal(1, loserCount);

        using var verifyContext = new AppDbContext(dbOptions);
        var finalOrder = await verifyContext.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == 3L);
        Assert.NotNull(finalOrder);
        Assert.True(finalOrder.Status == OrderStateMachine.Statuses.Paid || finalOrder.Status == OrderStateMachine.Statuses.Expired);
    }
}
