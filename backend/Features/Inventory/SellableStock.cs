using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Inventory;

internal static class SellableStock
{
    public static async Task LockItemsAsync(AppDbContext context, IEnumerable<string> itemCodes)
    {
        if (!context.Database.IsNpgsql()) return;

        foreach (var code in itemCodes.Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal))
        {
            await context.Database.ExecuteSqlInterpolatedAsync($"SELECT 1 FROM mathang WHERE mahang = {code} FOR UPDATE");
        }
    }

    public static async Task EnsureAvailableAsync(
        AppDbContext context, MatHang item, int quantity, string? excludedOrder = null)
    {
        var (available, onHand, totalReserved) = await GetAvailabilityBreakdownAsync(context, item, excludedOrder);
        if (quantity > available)
        {
            var reason = onHand > available && totalReserved > 0
                ? $" (kho còn {onHand}, đang giữ {totalReserved} cho đơn online chờ xử lý)"
                : string.Empty;
            throw new ConflictException($"Mặt hàng {item.TenHang} chỉ còn {available} sản phẩm khả dụng{reason}.");
        }
    }

    public static async Task<int> GetAvailableAsync(AppDbContext context, MatHang item, string? excludedOrder = null)
    {
        var (available, _, _) = await GetAvailabilityBreakdownAsync(context, item, excludedOrder);
        return available;
    }

    private static async Task<(int Available, int OnHand, int TotalReserved)> GetAvailabilityBreakdownAsync(
        AppDbContext context, MatHang item, string? excludedOrder = null)
    {
        int onHand;
        if (item.QuanLyImei)
        {
            onHand = await context.IMEIs.CountAsync(i => i.MaHang == item.MaHang && i.TrangThai == "con_hang");
        }
        else
        {
            var received = await context.ChiTietPhieuNhaps.Where(i => i.MaHang == item.MaHang).SumAsync(i => i.SlNhap);
            var sold = await context.ChiTietHoaDons.Where(i => i.MaHang == item.MaHang).SumAsync(i => i.SlBan);
            onHand = received - sold;
        }

        var unpaidCutoff = DateTime.UtcNow.AddMinutes(-15);
        var reserved = await (from line in context.ChiTietDonHangOnlines
                              join order in context.DonHangOnlines on line.MaDH equals order.MaDH
                              where line.MaHang == item.MaHang && order.MaDH != excludedOrder && order.SoHD == null &&
                                    order.TrangThai != "hoan_thanh" && order.TrangThai != "da_huy" &&
                                    order.TrangThai != "hoan_hang" &&
                                    (order.TrangThai != "cho_thanh_toan" || order.NgayDat > unpaidCutoff)
                              select line.SoLuong).SumAsync();
        var checkoutReserved = await context.OrderItems
            .Where(i => i.Variant!.Product!.StoreItemCode == item.MaHang &&
                (i.Order!.Status == "PENDING_PAYMENT" || i.Order.Status == "PAID" ||
                 i.Order.Status == "CONFIRMED" || i.Order.Status == "SHIPPING" || i.Order.Status == "COMPLETED"))
            .SumAsync(i => i.Qty);
        var totalReserved = reserved + checkoutReserved;
        return (Math.Max(0, onHand - totalReserved), onHand, totalReserved);
    }

    public static async Task<bool> ReserveVariantAsync(AppDbContext context, long variantId, int qty, CancellationToken ct)
    {
        var code = await context.ProductVariants.Where(v => v.Id == variantId)
            .Select(v => v.Product!.StoreItemCode).SingleOrDefaultAsync(ct);
        if (code == null)
            return await context.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE variants SET stock=stock-{qty}, updated_at=now() WHERE id={variantId} AND stock>={qty}", ct) == 1;

        await LockItemsAsync(context, [code]);
        var item = await context.MatHangs.SingleOrDefaultAsync(m => m.MaHang == code, ct);
        return item != null && qty <= await GetAvailableAsync(context, item);
    }
}
