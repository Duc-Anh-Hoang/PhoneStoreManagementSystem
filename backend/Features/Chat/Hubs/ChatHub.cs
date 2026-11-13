using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Chat.Hubs;

public class ChatHub : Hub
{
    private readonly AppDbContext _context;
    private readonly ILogger<ChatHub> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    // Lưu các phiên đang chờ bot phản hồi để tránh gửi trùng lặp (dùng ConcurrentDictionary an toàn luồng)
    private static readonly ConcurrentDictionary<string, CancellationTokenSource> _waitingTimers = new();

    public ChatHub(AppDbContext context, ILogger<ChatHub> logger, IServiceScopeFactory scopeFactory)
    {
        _context = context;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Khách hàng kết nối và tham gia phiên chat của mình
    /// </summary>
    public async Task JoinSession(string maPhienChat)
    {
        if (!string.IsNullOrWhiteSpace(maPhienChat))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Session_{maPhienChat}");
            _logger.LogInformation("Connection {ConnectionId} joined session {SessionId}", Context.ConnectionId, maPhienChat);
        }
    }

    /// <summary>
    /// Nhân viên CSKH tham gia vào phòng trực tổng đài
    /// </summary>
    public async Task JoinAgentPool()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "AgentPool");
        _logger.LogInformation("Connection {ConnectionId} joined AgentPool", Context.ConnectionId);
    }

    /// <summary>
    /// Gửi tin nhắn trong phiên chat (Khách gửi hoặc Nhân viên gửi)
    /// </summary>
    public async Task SendMessage(string maPhienChat, string nguoiGui, string noiDung, string? nguoiGuiId = null)
    {
        if (string.IsNullOrWhiteSpace(maPhienChat) || string.IsNullOrWhiteSpace(noiDung)) return;

        var senderRole = nguoiGui.Trim().ToUpperInvariant(); // "CUSTOMER" hoặc "AGENT"

        // 1. Lưu tin nhắn vào Database
        var message = new TinNhan
        {
            MaPhienChat = maPhienChat,
            NguoiGui = senderRole,
            NguoiGuiId = nguoiGuiId,
            NoiDung = noiDung.Trim(),
            ThoiGian = DateTime.UtcNow,
            DaDoc = false
        };

        _context.TinNhans.Add(message);

        var session = await _context.PhienChats.FirstOrDefaultAsync(p => p.MaPhienChat == maPhienChat);
        if (session != null)
        {
            if (senderRole == "AGENT")
            {
                if (session.TrangThai == "WAITING")
                {
                    session.TrangThai = "ACTIVE";
                    if (string.IsNullOrWhiteSpace(session.MaNV) && !string.IsNullOrWhiteSpace(nguoiGuiId))
                    {
                        session.MaNV = nguoiGuiId;
                    }
                    CancelWaitingBot(maPhienChat);

                    // Báo cho toàn bộ nhân viên biết phiên này đã có người tiếp nhận
                    await Clients.Group("AgentPool").SendAsync("SessionStatusChanged", new
                    {
                        maPhienChat,
                        trangThai = "ACTIVE",
                        maNV = session.MaNV
                    });
                }
            }
        }

        await _context.SaveChangesAsync();

        var messagePayload = new
        {
            message.MaTinNhan,
            message.MaPhienChat,
            message.NguoiGui,
            message.NguoiGuiId,
            message.NoiDung,
            message.DaDoc,
            message.ThoiGian
        };

        // 2. Broadcast tin nhắn tới người trong phiên chat
        await Clients.Group($"Session_{maPhienChat}").SendAsync("ReceiveMessage", messagePayload);

        // 3. Nếu khách hàng gửi tin nhắn:
        if (senderRole == "CUSTOMER" && session != null)
        {
            // Bắn realtime cho CSKH ở AgentPool biết có tin nhắn mới từ khách
            await Clients.Group("AgentPool").SendAsync("NewIncomingMessage", new
            {
                maPhienChat,
                khachHangTen = session.KhachHangTen,
                khachHangSdt = session.KhachHangSdt,
                tinNhanMoi = message.NoiDung,
                thoiGian = message.ThoiGian,
                trangThai = session.TrangThai
            });

            // Nếu phiên vẫn đang WAITING (chưa CSKH nào nhận) -> kích hoạt đếm ngược 8s bắn bot auto-reply
            if (session.TrangThai == "WAITING")
            {
                TriggerWaitingBot(maPhienChat);
            }
        }
    }

    /// <summary>
    /// Đánh dấu các tin nhắn trong phiên đã được đọc
    /// </summary>
    public async Task MarkAsRead(string maPhienChat, string readerRole)
    {
        if (string.IsNullOrWhiteSpace(maPhienChat)) return;

        var unreadMessages = await _context.TinNhans
            .Where(m => m.MaPhienChat == maPhienChat && !m.DaDoc && m.NguoiGui != readerRole.ToUpperInvariant())
            .ToListAsync();

        if (unreadMessages.Any())
        {
            foreach (var msg in unreadMessages)
            {
                msg.DaDoc = true;
            }
            await _context.SaveChangesAsync();

            await Clients.Group($"Session_{maPhienChat}").SendAsync("MessagesRead", new
            {
                maPhienChat,
                readBy = readerRole
            });
        }
    }

    private void TriggerWaitingBot(string maPhienChat)
    {
        CancelWaitingBot(maPhienChat);

        var cts = new CancellationTokenSource();
        _waitingTimers[maPhienChat] = cts;

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(8), cts.Token);
                if (cts.IsCancellationRequested) return;

                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var currentSession = await db.PhienChats.FirstOrDefaultAsync(p => p.MaPhienChat == maPhienChat);
                // Sau 8 giây kiểm tra lại: Nếu vẫn còn WAITING thì gửi tin nhắn bot hỗ trợ
                if (currentSession != null && currentSession.TrangThai == "WAITING")
                {
                    var botMsg = new TinNhan
                    {
                        MaPhienChat = maPhienChat,
                        NguoiGui = "BOT",
                        NoiDung = "🤖 Chào bạn, hiện tại các tư vấn viên của SamaMobile đang bận hỗ trợ các khách hàng khác. Tin nhắn của bạn đã được xếp hàng và sẽ có nhân viên phản hồi trong giây lát!",
                        ThoiGian = DateTime.UtcNow,
                        DaDoc = false
                    };

                    db.TinNhans.Add(botMsg);
                    await db.SaveChangesAsync();

                    await Clients.Group($"Session_{maPhienChat}").SendAsync("ReceiveMessage", new
                    {
                        botMsg.MaTinNhan,
                        botMsg.MaPhienChat,
                        botMsg.NguoiGui,
                        botMsg.NguoiGuiId,
                        botMsg.NoiDung,
                        botMsg.DaDoc,
                        botMsg.ThoiGian
                    });
                }
            }
            catch (OperationCanceledException)
            {
                // Task bị hủy bình thường khi nhân viên tiếp nhận
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi chạy tin nhắn chờ tự động cho phiên {MaPhienChat}", maPhienChat);
            }
            finally
            {
                _waitingTimers.TryRemove(maPhienChat, out _);
                cts.Dispose();
            }
        }, cts.Token);
    }

    private void CancelWaitingBot(string maPhienChat)
    {
        if (_waitingTimers.TryRemove(maPhienChat, out var cts))
        {
            try
            {
                cts.Cancel();
                cts.Dispose();
            }
            catch (ObjectDisposedException) { }
        }
    }
}
