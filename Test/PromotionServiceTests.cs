using System.Net;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Promotions;
using PhoneStoreAPI.Features.Promotions.DTOs;
using PhoneStoreAPI.Features.Promotions.Validators;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class PromotionServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ValidPromotion_Succeeds()
    {
        // Arrange
        using var context = CreateDbContext();
        var service = new PromotionService(context, new CreatePromotionRequestValidator(), new UpdatePromotionRequestValidator());

        var req = new CreatePromotionRequest
        {
            MaKM = "KM_TET2026",
            TenKM = "Khuyến mãi Tết 2026",
            PhanTramGiam = 15,
            NgayBD = DateTime.UtcNow.AddDays(-1),
            NgayKT = DateTime.UtcNow.AddDays(10),
            TamDung = false
        };

        // Act
        var res = await service.CreatePromotionAsync(req);

        // Assert
        Assert.NotNull(res);
        Assert.Equal("KM_TET2026", res.MaKM);
        Assert.Equal("dang_ap_dung", res.TrangThai);
    }

    [Fact]
    public async Task CreateAsync_InvalidPercentage_ThrowsValidationException()
    {
        // Arrange
        using var context = CreateDbContext();
        var service = new PromotionService(context, new CreatePromotionRequestValidator(), new UpdatePromotionRequestValidator());

        var req = new CreatePromotionRequest
        {
            MaKM = "KM_FAIL",
            TenKM = "Khuyến mãi Lỗi",
            PhanTramGiam = 120, // > 100%
            NgayBD = DateTime.UtcNow,
            NgayKT = DateTime.UtcNow.AddDays(5)
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => service.CreatePromotionAsync(req));
    }

    [Fact]
    public async Task CreateAsync_NgayKTBeforeNgayBD_ThrowsValidationException()
    {
        // Arrange
        using var context = CreateDbContext();
        var service = new PromotionService(context, new CreatePromotionRequestValidator(), new UpdatePromotionRequestValidator());

        var req = new CreatePromotionRequest
        {
            MaKM = "KM_DATE_ERR",
            TenKM = "Khuyến mãi Lỗi Ngày",
            PhanTramGiam = 10,
            NgayBD = DateTime.UtcNow.AddDays(5),
            NgayKT = DateTime.UtcNow.AddDays(2) // End before start
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => service.CreatePromotionAsync(req));
    }

    [Fact]
    public async Task GetActivePromotions_ReturnsOnlyActiveNotPausedPromotions()
    {
        // Arrange
        using var context = CreateDbContext();
        var now = DateTime.UtcNow;

        context.KhuyenMais.AddRange(
            new KhuyenMai
            {
                MaKM = "KM_ACTIVE",
                TenKM = "Đang chạy",
                PhanTramGiam = 10,
                NgayBD = now.AddDays(-2),
                NgayKT = now.AddDays(5),
                TamDung = false
            },
            new KhuyenMai
            {
                MaKM = "KM_PAUSED",
                TenKM = "Tạm dừng",
                PhanTramGiam = 10,
                NgayBD = now.AddDays(-2),
                NgayKT = now.AddDays(5),
                TamDung = true
            },
            new KhuyenMai
            {
                MaKM = "KM_EXPIRED",
                TenKM = "Hết hạn",
                PhanTramGiam = 10,
                NgayBD = now.AddDays(-10),
                NgayKT = now.AddDays(-1),
                TamDung = false
            }
        );
        await context.SaveChangesAsync();

        var service = new PromotionService(context, new CreatePromotionRequestValidator(), new UpdatePromotionRequestValidator());

        // Act
        var activeList = await service.GetActivePromotionsAsync();

        // Assert
        Assert.Single(activeList);
        Assert.Equal("KM_ACTIVE", activeList[0].MaKM);
    }

    [Fact]
    public async Task DeleteAsync_PromotionLinkedToInvoice_ThrowsConflictException()
    {
        // Arrange
        using var context = CreateDbContext();
        context.KhuyenMais.Add(new KhuyenMai
        {
            MaKM = "KM_LINKED",
            TenKM = "Khuyến mãi gắn hóa đơn",
            PhanTramGiam = 5,
            NgayBD = DateTime.UtcNow.AddDays(-1),
            NgayKT = DateTime.UtcNow.AddDays(1)
        });

        context.HoaDons.Add(new HoaDon
        {
            SoHD = "HD0001",
            MaKM = "KM_LINKED",
            MaNV = "NV001",
            NgayLap = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        var service = new PromotionService(context, new CreatePromotionRequestValidator(), new UpdatePromotionRequestValidator());

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ConflictException>(() => service.DeletePromotionAsync("KM_LINKED"));
        Assert.Equal(HttpStatusCode.Conflict, ex.StatusCode);
        Assert.Contains("hóa đơn", ex.Message);
    }
}
