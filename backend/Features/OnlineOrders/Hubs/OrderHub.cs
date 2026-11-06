using Microsoft.AspNetCore.SignalR;

namespace PhoneStoreAPI.Features.OnlineOrders.Hubs;

/// <summary>
/// SignalR Hub phục vụ xử lý realtime đơn hàng:
/// - Thông báo đơn đặt hàng mới đến Admin ngay tức thì
/// - Thông báo cập nhật tiến trình đơn hàng (Admin duyệt, đang giao, hoàn thành, hủy) đến User
/// </summary>
public class OrderHub : Hub
{
    public const string AdminGroup = "AdminGroup";

    /// <summary>
    /// Admin Dashboard tham gia nhóm quản trị để nhận mọi thông báo đơn đặt hàng mới
    /// </summary>
    public async Task JoinAdminGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, AdminGroup);
    }

    /// <summary>
    /// Rời khỏi nhóm quản trị
    /// </summary>
    public async Task LeaveAdminGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, AdminGroup);
    }

    /// <summary>
    /// Khách hàng lắng nghe theo số điện thoại đặt hàng
    /// </summary>
    public async Task JoinCustomerGroup(string phone)
    {
        if (!string.IsNullOrWhiteSpace(phone))
        {
            var cleanPhone = phone.Trim().Replace(" ", "").Replace(".", "").Replace("-", "");
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Customer_{cleanPhone}");
        }
    }

    /// <summary>
    /// Khách hàng hoặc Admin theo dõi tiến trình 1 đơn hàng cụ thể theo Mã Đơn (VD: DH-261106-1234)
    /// </summary>
    public async Task JoinOrderGroup(string maDH)
    {
        if (!string.IsNullOrWhiteSpace(maDH))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Order_{maDH.Trim()}");
        }
    }

    /// <summary>
    /// Client (User) thông báo trực tiếp khi vừa hoàn tất đặt đơn
    /// </summary>
    public async Task NotifyNewOrder(object orderData)
    {
        await Clients.Group(AdminGroup).SendAsync("ReceiveNewOrder", orderData);
        await Clients.All.SendAsync("OrderCreatedNotification", orderData);
    }

    /// <summary>
    /// Admin thông báo cập nhật trạng thái đơn (Duyệt đơn, Đang giao, v.v.)
    /// </summary>
    public async Task NotifyOrderStatusChanged(string maDH, string trangThai, string message, string? phone)
    {
        var payload = new
        {
            maDH,
            trangThai,
            statusText = message,
            message,
            sdt = phone,
            updatedAt = DateTime.UtcNow
        };

        // Gửi 1 lần duy nhất cho toàn bộ client kết nối (tránh duplicate khi client vừa trong group Order, Customer và All)
        await Clients.All.SendAsync("OrderStatusUpdated", payload);
    }
}
