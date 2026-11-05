using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using PhoneStoreAPI.Common.Configuration;
using PhoneStoreAPI.Common.Services;
using Xunit;

namespace backend.Tests;

public class EmailServiceTests
{
    [Fact]
    public async Task SendEmailAsync_WithEmptyRecipient_ReturnsFalse()
    {
        var settings = Options.Create(new SmtpSettings());
        var service = new EmailService(settings, NullLogger<EmailService>.Instance);

        var result = await service.SendEmailAsync("", "Test Subject", "<p>Test Content</p>");

        Assert.False(result);
    }

    [Fact]
    public async Task SendEmailAsync_InSimulationMode_ReturnsTrue()
    {
        var settings = Options.Create(new SmtpSettings
        {
            Password = "your-smtp-app-password" // Placeholder simulation mode
        });
        var service = new EmailService(settings, NullLogger<EmailService>.Instance);

        var result = await service.SendEmailAsync("customer@test.com", "Test Subject", "<p>Test Content</p>", "Test User");

        Assert.True(result);
    }

    [Fact]
    public async Task SendOtpEmailAsync_InSimulationMode_ReturnsTrue()
    {
        var settings = Options.Create(new SmtpSettings());
        var service = new EmailService(settings, NullLogger<EmailService>.Instance);

        var result = await service.SendOtpEmailAsync("user@test.com", "123456", "xác thực đơn hàng");

        Assert.True(result);
    }

    [Fact]
    public async Task SendOrderConfirmationEmailAsync_InSimulationMode_ReturnsTrue()
    {
        var settings = Options.Create(new SmtpSettings());
        var service = new EmailService(settings, NullLogger<EmailService>.Instance);

        var items = new List<OrderItemEmailDto>
        {
            new() { TenHang = "iPhone 15 Pro Max 256GB", SoLuong = 1, DonGia = 28990000 }
        };

        var result = await service.SendOrderConfirmationEmailAsync(
            "buyer@test.com", "Nguyễn Văn A", "DH20261105001", 28990000, items);

        Assert.True(result);
    }
}
