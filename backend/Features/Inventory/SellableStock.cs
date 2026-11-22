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
        if (quantity > onHand - reserved)
        {
            throw new ConflictException($"Mặt hàng {item.TenHang} chỉ còn {Math.Max(0, onHand - reserved)} sản phẩm khả dụng.");
        }
    }
}
