using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Checkout.DTOs;
using PhoneStoreAPI.Features.Checkout.PaymentProviders;
using PhoneStoreAPI.Features.Checkout.Services;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class Task5PaymentWebhookTests
{
    private (AppDbContext context, PaymentWebhookHandler handler, MockQrPaymentProvider provider) CreateHandler(string dbName)
    {
        var root = new Microsoft.EntityFrameworkCore.Storage.InMemoryDatabaseRoot();
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName, databaseRoot: root)
            .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new AppDbContext(dbOptions);

        var sm = new OrderStateMachine(context);
        var provider = new MockQrPaymentProvider("test_secret_key_123");
        var handler = new PaymentWebhookHandler(context, sm, provider);

        return (context, handler, provider);
    }

    [Fact]
    public async Task Invalid_Signature_Is_Rejected_With_400()
    {
        var (context, handler, provider) = CreateHandler(Guid.NewGuid().ToString());

        var payload = JsonSerializer.Serialize(new QrWebhookPayload
        {
            OrderCode = "ORD-TEST-001",
            ProviderTxnId = "TXN-001",
            Amount = 1000000
        });

        var result = await handler.ProcessQrWebhookAsync(provider.ProviderName, payload, "fake_signature_abc");

        Assert.False(result.Success);
        Assert.Equal(400, result.StatusCode);
        Assert.Contains("không hợp lệ", result.Message);
    }

    [Fact]
    public async Task Webhook_Sent_3_Times_Records_Only_1_Payment_And_Transitions_To_Paid()
    {
        var (context, handler, provider) = CreateHandler(Guid.NewGuid().ToString());

        var order = new Order
        {
            Id = 10,
            OrderCode = "ORD-TASK5-001",
            Status = OrderStateMachine.Statuses.PendingPayment,
            Total = 34990000,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IdempotencyKey = "key-task5-001"
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        var payloadObj = new QrWebhookPayload
        {
            OrderCode = order.OrderCode,
            ProviderTxnId = "MOCK_TXN_UNIQUE_12345",
            Amount = 34990000,
            Status = "SUCCESS"
        };
        var rawPayload = JsonSerializer.Serialize(payloadObj);
        var validSignature = provider.GenerateSignature(rawPayload, "test_secret_key_123");

        // Gửi lần 1
        var res1 = await handler.ProcessQrWebhookAsync(provider.ProviderName, rawPayload, validSignature);
        Assert.True(res1.Success);
        Assert.Equal(200, res1.StatusCode);

        // Gửi lần 2
        var res2 = await handler.ProcessQrWebhookAsync(provider.ProviderName, rawPayload, validSignature);
        Assert.True(res2.Success);
        Assert.Equal(200, res2.StatusCode);
        Assert.Contains("trước đó", res2.Message);

        // Gửi lần 3
        var res3 = await handler.ProcessQrWebhookAsync(provider.ProviderName, rawPayload, validSignature);
        Assert.True(res3.Success);
        Assert.Equal(200, res3.StatusCode);

        // Kiểm tra database: CHỈ CÓ ĐÚNG 1 bản ghi payment
        var payments = await context.Payments.Where(p => p.OrderId == order.Id).ToListAsync();
        Assert.Single(payments);
        Assert.Equal("MOCK_TXN_UNIQUE_12345", payments.First().ProviderTxnId);
        Assert.Equal("SUCCESS", payments.First().Status);

        // Đơn hàng chuyển sang PAID
        var updatedOrder = await context.Orders.FindAsync(order.Id);
        Assert.Equal(OrderStateMachine.Statuses.Paid, updatedOrder!.Status);
    }

    [Fact]
    public async Task Amount_Mismatch_Is_Not_Confirmed_And_Marked_For_Manual_Handling()
    {
        var (context, handler, provider) = CreateHandler(Guid.NewGuid().ToString());

        var order = new Order
        {
            Id = 20,
            OrderCode = "ORD-TASK5-MISMATCH",
            Status = OrderStateMachine.Statuses.PendingPayment,
            Total = 20000000,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IdempotencyKey = "key-task5-mismatch"
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // Khách chuyển thiếu tiền: đơn 20tr nhưng chỉ chuyển 15tr
        var payloadObj = new QrWebhookPayload
        {
            OrderCode = order.OrderCode,
            ProviderTxnId = "TXN_MISMATCH_999",
            Amount = 15000000,
            Status = "SUCCESS"
        };
        var rawPayload = JsonSerializer.Serialize(payloadObj);
        var validSignature = provider.GenerateSignature(rawPayload, "test_secret_key_123");

        var result = await handler.ProcessQrWebhookAsync(provider.ProviderName, rawPayload, validSignature);

        Assert.False(result.Success);
        Assert.Equal(200, result.StatusCode);
        Assert.Contains("không khớp", result.Message);

        // Đơn hàng KHÔNG được chuyển sang PAID, vẫn giữ PENDING_PAYMENT
        var updatedOrder = await context.Orders.FindAsync(order.Id);
        Assert.Equal(OrderStateMachine.Statuses.PendingPayment, updatedOrder!.Status);

        // Bản ghi payment được đánh dấu AMOUNT_MISMATCH
        var payment = await context.Payments.FirstOrDefaultAsync(p => p.OrderId == order.Id);
        Assert.NotNull(payment);
        Assert.Equal("AMOUNT_MISMATCH", payment.Status);

        // Có ghi nhận lịch sử xử lý tay
        var history = await context.OrderStatusHistories.FirstOrDefaultAsync(h => h.OrderId == order.Id);
        Assert.NotNull(history);
        Assert.Contains("không khớp", history.Reason);
    }

    [Fact]
    public async Task Cod_Flow_Confirms_Order_And_Requires_Otp_When_High_Value()
    {
        var (context, handler, provider) = CreateHandler(Guid.NewGuid().ToString());

        // Đơn COD thông thường (10 triệu)
        var normalOrder = new Order
        {
            Id = 31,
            OrderCode = "ORD-COD-NORMAL",
            Status = OrderStateMachine.Statuses.PendingPayment,
            Total = 10000000,
            PaymentMethod = "COD",
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IdempotencyKey = "key-cod-1"
        };
        // Đơn COD giá trị cao (25 triệu > ngưỡng 20 triệu)
        var highValueOrder = new Order
        {
            Id = 32,
            OrderCode = "ORD-COD-HIGH",
            Status = OrderStateMachine.Statuses.PendingPayment,
            Total = 25000000,
            PaymentMethod = "COD",
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IdempotencyKey = "key-cod-2"
        };
        context.Orders.AddRange(normalOrder, highValueOrder);
        await context.SaveChangesAsync();

        // 1. Đơn thường COD: xác nhận thẳng sang CONFIRMED
        var resNormal = await handler.ConfirmCodOrderAsync(normalOrder.OrderCode);
        Assert.True(resNormal.Success);
        var refreshedNormal = await context.Orders.FindAsync(31L);
        Assert.Equal(OrderStateMachine.Statuses.Confirmed, refreshedNormal!.Status);

        // 2. Đơn cao COD không có OTP -> bị từ chối
        var resHighNoOtp = await handler.ConfirmCodOrderAsync(highValueOrder.OrderCode, null);
        Assert.False(resHighNoOtp.Success);
        Assert.Contains("yêu cầu xác thực mã OTP", resHighNoOtp.ErrorMessage);

        // 3. Đơn cao COD OTP sai -> bị từ chối
        var resHighWrongOtp = await handler.ConfirmCodOrderAsync(highValueOrder.OrderCode, "999999");
        Assert.False(resHighWrongOtp.Success);
        Assert.Contains("không chính xác", resHighWrongOtp.ErrorMessage);

        // 4. Đơn cao COD OTP đúng -> xác nhận thành công sang CONFIRMED
        var resHighValidOtp = await handler.ConfirmCodOrderAsync(highValueOrder.OrderCode, "123456");
        Assert.True(resHighValidOtp.Success);
        var refreshedHigh = await context.Orders.FindAsync(32L);
        Assert.Equal(OrderStateMachine.Statuses.Confirmed, refreshedHigh!.Status);
    }
}
