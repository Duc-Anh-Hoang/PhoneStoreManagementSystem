using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.OnlineOrders;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;
using PhoneStoreAPI.Features.Warranty;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class OrderLookupTests
{
    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private static IConfiguration CreateConfig(string otpMode = "demo")
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["OTP_MODE"] = otpMode,
                ["Jwt:Key"] = "PhoneStoreManagementSystem_SuperSecretKey_2026_AspNetCore10_StrongSecretKey!",
                ["Jwt:Issuer"] = "PhoneStoreAPI",
                ["Jwt:Audience"] = "PhoneStoreClient"
            })
            .Build();
    }

    [Fact]
    public async Task GenerateOrderCode_100Consecutive_AllMatchFormatAndUnique()
    {
        using var context = CreateDbContext();
        var generator = new CodeGenerator(context);
        var generated = new HashSet<string>();
        var regex = new Regex(@"^DH-[2-9A-HJKMNP-Z]{6}$");

        for (int i = 0; i < 100; i++)
        {
            var code = await generator.GenerateOrderCode(c => Task.FromResult(generated.Contains(c)));
            Assert.Matches(regex, code);
            Assert.True(generated.Add(code), $"Trùng mã đơn hàng: {code}");
        }

        Assert.Equal(100, generated.Count);
    }

    [Fact]
    public void OrderMaskHelper_MasksAllSensitiveFields()
    {
        var nameMask = OrderMaskHelper.MaskName("Nguyễn Văn A");
        Assert.StartsWith("Nguyễn", nameMask);
        Assert.Contains("***", nameMask);
        Assert.EndsWith("A", nameMask);
        Assert.DoesNotContain("Văn", nameMask);

        var phoneMask = OrderMaskHelper.MaskPhone("0987654321");
        Assert.Equal("0987***321", phoneMask);

        var emailMask = OrderMaskHelper.MaskEmail("abcdef@gmail.com");
        Assert.Equal("ab***@gmail.com", emailMask);

        var addrMask = OrderMaskHelper.MaskAddress("123 Lê Lợi, Phường Bến Nghé, Quận 1, TP.HCM");
        Assert.Equal("Phường Bến Nghé, Quận 1, TP.HCM", addrMask);
        Assert.DoesNotContain("123", addrMask);
        Assert.DoesNotContain("Lê Lợi", addrMask);
    }

    [Fact]
    public async Task GetOrderForLookupAsync_MatchingAndNonMatchingCases()
    {
        using var context = CreateDbContext();
        context.KhachHangs.Add(new KhachHang
        {
            MaKH = "KH001",
            HoTen = "Nguyễn Văn A",
            Sdt = "0987654321",
            Email = "abcdef@gmail.com",
            DiaChi = "123 Lê Lợi, Phường Bến Nghé, Quận 1, TP.HCM"
        });
        context.MatHangs.Add(new MatHang
        {
            MaHang = "MH001",
            TenHang = "iPhone 16 Pro",
            DonGia = 28990000m,
            Dvt = "Cái",
            MaLoai = "APPLE",
            MaNhom = "DIENTHOAI"
        });
        context.DonHangOnlines.Add(new DonHangOnline
        {
            MaDH = "DH-7K3F9Q",
            NgayDat = DateTime.UtcNow,
            MaKH = "KH001",
            TenNguoiNhan = "Nguyễn Văn A",
            SdtNguoiNhan = "0987654321",
            DiaChiGiao = "123 Lê Lợi, Phường Bến Nghé, Quận 1, TP.HCM",
            PhuongThucThanhToan = "cod",
            TongTien = 28990000m,
            TrangThai = "cho_xac_nhan"
        });
        context.ChiTietDonHangOnlines.Add(new ChiTietDonHangOnline
        {
            MaDH = "DH-7K3F9Q",
            MaHang = "MH001",
            SoLuong = 1,
            DonGia = 28990000m
        });
        await context.SaveChangesAsync();

        var service = new OnlineOrderService(
            context,
            null!,
            new CodeGenerator(context),
            null!);

        // Đúng mã + SĐT dạng +84 có khoảng trắng
        var found = await service.GetOrderForLookupAsync("DH-7K3F9Q", "+84 987 654 321");
        Assert.NotNull(found);
        Assert.Equal("DH-7K3F9Q", found.MaDH);
        Assert.Equal("0987***321", found.SdtMask);
        Assert.Equal("ab***@gmail.com", found.EmailMask);
        Assert.Equal("Phường Bến Nghé, Quận 1, TP.HCM", found.DiaChiMask);
        Assert.Single(found.Items!);
        Assert.Single(found.StatusHistory);

        // Sai SĐT hoặc sai mã đều trả null (để controller trả cùng thông báo chung)
        Assert.Null(await service.GetOrderForLookupAsync("DH-7K3F9Q", "0911111111"));
        Assert.Null(await service.GetOrderForLookupAsync("DH-XXXXXX", "0987654321"));
    }

    [Fact]
    public async Task OtpService_DemoAndSmsModes_SingleUse_LockAfter5Fails_Limit3PerHour()
    {
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var configDemo = CreateConfig("demo");
        var otpService = new OtpService(cache, configDemo, NullLogger<OtpService>.Instance);

        // 1. Xin OTP thành công trong demo mode -> trả code + message chuẩn
        var req1 = await otpService.RequestOtpAsync("0987654321");
        Assert.False(req1.Blocked);
        Assert.Equal("Nếu số điện thoại hợp lệ, mã OTP đã được gửi.", req1.Message);
        Assert.NotNull(req1.Code);
        Assert.Matches(@"^\d{6}$", req1.Code);

        // 2. Xác thực đúng -> trả token và xóa OTP (single-use)
        var verifyOk = await otpService.VerifyOtpAsync("0987654321", req1.Code!);
        Assert.True(verifyOk.Success);
        Assert.NotNull(verifyOk.Token);

        // Validate token
        var services = new ServiceCollection()
            .AddSingleton(configDemo)
            .BuildServiceProvider();
        var extractedPhone = OtpTokenValidator.ValidateAndExtractPhone(verifyOk.Token!, services);
        Assert.Equal("0987654321", extractedPhone);

        // Dùng lại OTP lần 2 -> thất bại (single-use)
        var verifyReuse = await otpService.VerifyOtpAsync("0987654321", req1.Code!);
        Assert.False(verifyReuse.Success);

        // 3. Sai 5 lần -> hủy OTP
        var req2 = await otpService.RequestOtpAsync("0912345678");
        for (int i = 0; i < 5; i++)
        {
            var bad = await otpService.VerifyOtpAsync("0912345678", "000000");
            // Nếu tình cờ trùng 000000 thì bỏ qua
            if (req2.Code != "000000") Assert.False(bad.Success);
        }
        // Dù nhập đúng sau 5 lần sai cũng bị từ chối vì OTP đã bị hủy
        if (req2.Code != "000000")
        {
            var after5 = await otpService.VerifyOtpAsync("0912345678", req2.Code!);
            Assert.False(after5.Success);
        }

        // 4. Xin quá 3 lần/giờ cho cùng SĐT -> lần 4 bị chặn
        var phoneSpam = "0909000111";
        Assert.False((await otpService.RequestOtpAsync(phoneSpam)).Blocked);
        Assert.False((await otpService.RequestOtpAsync(phoneSpam)).Blocked);
        Assert.False((await otpService.RequestOtpAsync(phoneSpam)).Blocked);
        var req4 = await otpService.RequestOtpAsync(phoneSpam);
        Assert.True(req4.Blocked);

        // 5. Chế độ sms không trả code về client
        var otpSms = new OtpService(cache, CreateConfig("sms"), NullLogger<OtpService>.Instance);
        var smsReq = await otpSms.RequestOtpAsync("0908888999");
        Assert.False(smsReq.Blocked);
        Assert.Null(smsReq.Code);
        Assert.Equal("Nếu số điện thoại hợp lệ, mã OTP đã được gửi.", smsReq.Message);
    }
}
