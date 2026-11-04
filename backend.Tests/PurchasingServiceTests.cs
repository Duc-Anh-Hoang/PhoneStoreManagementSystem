using System.Net;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Purchasing;
using PhoneStoreAPI.Features.Purchasing.DTOs;
using PhoneStoreAPI.Features.Purchasing.Validators;
using PhoneStoreAPI.Models.Entities;
using Xunit;

namespace backend.Tests;

public class TestCurrentUser : ICurrentUser
{
    public string? UserName { get; set; } = "admin";
    public string? MaNV { get; set; } = "NV001";
    public string? Role { get; set; } = AppConstants.Roles.Admin;
    public string? MaKH { get; set; } = null;
    public bool IsAuthenticated { get; set; } = true;
    public bool IsInRole(string role) => Role == role;
}

public class PurchasingServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        // Seed basic dependencies
        context.NhanViens.Add(new NhanVien
        {
            MaNV = "NV001",
            HoTen = "Nguyễn Văn Kho",
            ChucVu = "Kho",
            Email = "kho@cellphones.vn"
        });

        context.NhaCungCaps.Add(new NhaCungCap
        {
            MaNCC = "NCC01",
            TenNCC = "Apple Vietnam",
            Sdt = "02838234567"
        });

        context.MatHangs.AddRange(
            new MatHang
            {
                MaHang = "IP15PM",
                TenHang = "iPhone 15 Pro Max",
                DonGia = 30000000,
                QuanLyImei = true,
                MaLoai = "APPLE",
                MaNhom = "DIENTHOAI"
            },
            new MatHang
            {
                MaHang = "CAP_C",
                TenHang = "Cáp Sạc C",
                DonGia = 200000,
                QuanLyImei = false,
                MaLoai = "APPLE",
                MaNhom = "PHUKIEN"
            }
        );

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task CreateAsync_ValidInput_CreatesPhieuNhapAndImeis()
    {
        // Arrange
        using var context = CreateDbContext();
        var currentUser = new TestCurrentUser { MaNV = "NV001" };
        var codeGen = new CodeGenerator(context);
        var validator = new CreatePhieuNhapRequestValidator();
        var service = new PurchasingService(context, currentUser, codeGen, validator);

        var request = new CreatePhieuNhapRequest
        {
            MaNCC = "NCC01",
            ChiTiet = new List<ChiTietNhapItemRequest>
            {
                new()
                {
                    MaHang = "IP15PM",
                    SlNhap = 2,
                    DonGiaNhap = 25000000,
                    Imeis = new List<string> { "123456789012345", "123456789012346" }
                },
                new()
                {
                    MaHang = "CAP_C",
                    SlNhap = 5,
                    DonGiaNhap = 150000,
                    Imeis = new List<string>()
                }
            }
        };

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("PN", result.SoPN);
        Assert.Equal("NCC01", result.MaNCC);
        Assert.Equal(2, result.ChiTiet.Count);
        Assert.Equal(25000000 * 2 + 150000 * 5, result.TongTien);

        // Verify database records
        var pnInDb = await context.PhieuNhaps.FirstOrDefaultAsync(p => p.SoPN == result.SoPN);
        Assert.NotNull(pnInDb);

        var imeis = await context.IMEIs.Where(i => i.SoPN == result.SoPN).ToListAsync();
        Assert.Equal(2, imeis.Count);
        Assert.All(imeis, i => Assert.Equal("con_hang", i.TrangThai));
    }

    [Fact]
    public async Task CreateAsync_DuplicateImeiInDatabase_ThrowsConflictException()
    {
        // Arrange
        using var context = CreateDbContext();
        // Add existing IMEI
        context.IMEIs.Add(new IMEI
        {
            MaIMEI = "123456789012345",
            SoIMEI = "123456789012345",
            MaHang = "IP15PM",
            SoPN = "PN_EXISTING",
            TrangThai = "con_hang"
        });
        await context.SaveChangesAsync();

        var currentUser = new TestCurrentUser { MaNV = "NV001" };
        var codeGen = new CodeGenerator(context);
        var validator = new CreatePhieuNhapRequestValidator();
        var service = new PurchasingService(context, currentUser, codeGen, validator);

        var request = new CreatePhieuNhapRequest
        {
            MaNCC = "NCC01",
            ChiTiet = new List<ChiTietNhapItemRequest>
            {
                new()
                {
                    MaHang = "IP15PM",
                    SlNhap = 1,
                    DonGiaNhap = 25000000,
                    Imeis = new List<string> { "123456789012345" }
                }
            }
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ConflictException>(() => service.CreateAsync(request));
        Assert.Equal(HttpStatusCode.Conflict, ex.StatusCode);
        Assert.Contains("đã tồn tại trong hệ thống", ex.Message);
    }

    [Fact]
    public async Task CreateAsync_ImeiCountMismatchWithSlNhap_ThrowsBadRequestException()
    {
        // Arrange
        using var context = CreateDbContext();
        var currentUser = new TestCurrentUser { MaNV = "NV001" };
        var codeGen = new CodeGenerator(context);
        var validator = new CreatePhieuNhapRequestValidator();
        var service = new PurchasingService(context, currentUser, codeGen, validator);

        var request = new CreatePhieuNhapRequest
        {
            MaNCC = "NCC01",
            ChiTiet = new List<ChiTietNhapItemRequest>
            {
                new()
                {
                    MaHang = "IP15PM",
                    SlNhap = 2,
                    DonGiaNhap = 25000000,
                    Imeis = new List<string> { "123456789012345" } // Only 1 IMEI while slNhap is 2
                }
            }
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.CreateAsync(request));
        Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        Assert.Contains("yêu cầu nhập đúng 2 số IMEI", ex.Message);
    }

    [Fact]
    public async Task CreateAsync_InvalidImeiFormat_ThrowsBadRequestException()
    {
        // Arrange
        using var context = CreateDbContext();
        var currentUser = new TestCurrentUser { MaNV = "NV001" };
        var codeGen = new CodeGenerator(context);
        var validator = new CreatePhieuNhapRequestValidator();
        var service = new PurchasingService(context, currentUser, codeGen, validator);

        var request = new CreatePhieuNhapRequest
        {
            MaNCC = "NCC01",
            ChiTiet = new List<ChiTietNhapItemRequest>
            {
                new()
                {
                    MaHang = "IP15PM",
                    SlNhap = 1,
                    DonGiaNhap = 25000000,
                    Imeis = new List<string> { "12345" } // Invalid length (not 15 digits)
                }
            }
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.CreateAsync(request));
        Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        Assert.Contains("15 chữ số", ex.Message);
    }

    [Fact]
    public async Task CreateAsync_NonImeiItemWithImeisProvided_ThrowsBadRequestException()
    {
        // Arrange
        using var context = CreateDbContext();
        var currentUser = new TestCurrentUser { MaNV = "NV001" };
        var codeGen = new CodeGenerator(context);
        var validator = new CreatePhieuNhapRequestValidator();
        var service = new PurchasingService(context, currentUser, codeGen, validator);

        var request = new CreatePhieuNhapRequest
        {
            MaNCC = "NCC01",
            ChiTiet = new List<ChiTietNhapItemRequest>
            {
                new()
                {
                    MaHang = "CAP_C", // QuanLyImei = false
                    SlNhap = 1,
                    DonGiaNhap = 150000,
                    Imeis = new List<string> { "123456789012345" }
                }
            }
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.CreateAsync(request));
        Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        Assert.Contains("không quản lý IMEI", ex.Message);
    }

    [Fact]
    public async Task CreateAsync_DuplicateMaHangInRequest_ThrowsBadRequestException()
    {
        // Arrange
        using var context = CreateDbContext();
        var currentUser = new TestCurrentUser { MaNV = "NV001" };
        var codeGen = new CodeGenerator(context);
        var validator = new CreatePhieuNhapRequestValidator();
        var service = new PurchasingService(context, currentUser, codeGen, validator);

        var request = new CreatePhieuNhapRequest
        {
            MaNCC = "NCC01",
            ChiTiet = new List<ChiTietNhapItemRequest>
            {
                new() { MaHang = "CAP_C", SlNhap = 1, DonGiaNhap = 150000 },
                new() { MaHang = "CAP_C", SlNhap = 2, DonGiaNhap = 140000 }
            }
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => service.CreateAsync(request));
        Assert.Contains("Không được lặp lại mặt hàng", ex.Message);
    }
}

