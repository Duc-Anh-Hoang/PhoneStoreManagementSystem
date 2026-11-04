using System.Net;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.SystemSettings;
using PhoneStoreAPI.Features.SystemSettings.DTOs;
using PhoneStoreAPI.Features.SystemSettings.Validators;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class ThamSoServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        context.ThamSos.AddRange(
            new ThamSo { MaThamSo = "VAT_PHANTRAM", TenThamSo = "VAT", GiaTri = 10, DonVi = "%" },
            new ThamSo { MaThamSo = "DIEM_MOI_VND", TenThamSo = "Điểm mới", GiaTri = 100000, DonVi = "VND" },
            new ThamSo { MaThamSo = "HAN_DOITRA_NGAY", TenThamSo = "Hạn đổi trả", GiaTri = 30, DonVi = "ngày" }
        );
        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GetAsync_HitsCacheOnSecondCall()
    {
        // Arrange
        using var context = CreateDbContext();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new ThamSoProvider(context, cache);

        // Act 1
        var val1 = await provider.GetAsync("VAT_PHANTRAM");
        Assert.Equal(10, val1);

        // Mutate DB directly without going through cache invalidation
        var entity = await context.ThamSos.FirstAsync(t => t.MaThamSo == "VAT_PHANTRAM");
        entity.GiaTri = 15;
        await context.SaveChangesAsync();

        // Act 2: Should still read cached 10
        var val2 = await provider.GetAsync("VAT_PHANTRAM");
        Assert.Equal(10, val2);

        // Act 3: Invalidate cache and read again
        provider.InvalidateCache("VAT_PHANTRAM");
        var val3 = await provider.GetAsync("VAT_PHANTRAM");
        Assert.Equal(15, val3);
    }

    [Fact]
    public async Task GetAsync_NonExistentParameter_ThrowsNotFoundException()
    {
        // Arrange
        using var context = CreateDbContext();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new ThamSoProvider(context, cache);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => provider.GetAsync("UNKNOWN_PARAM"));
        Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesDbAndInvalidatesCache()
    {
        // Arrange
        using var context = CreateDbContext();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new ThamSoProvider(context, cache);
        var service = new SystemSettingService(context, provider, new UpdateThamSoRequestValidator());

        // Cache initial value
        var initVal = await provider.GetAsync("HAN_DOITRA_NGAY");
        Assert.Equal(30, initVal);

        // Act: Update to 45
        var updated = await service.UpdateAsync("HAN_DOITRA_NGAY", new UpdateThamSoRequest
        {
            GiaTri = 45
        });

        // Assert
        Assert.Equal(45, updated.GiaTri);

        // Next call to provider should immediately reflect the new value because cache was invalidated
        var providerVal = await provider.GetAsync("HAN_DOITRA_NGAY");
        Assert.Equal(45, providerVal);
    }

    [Fact]
    public async Task UpdateAsync_NegativeValue_ThrowsValidationException()
    {
        // Arrange
        using var context = CreateDbContext();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var provider = new ThamSoProvider(context, cache);
        var service = new SystemSettingService(context, provider, new UpdateThamSoRequestValidator());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => service.UpdateAsync("VAT_PHANTRAM", new UpdateThamSoRequest
        {
            GiaTri = -5
        }));
    }
}

