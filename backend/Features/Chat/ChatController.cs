using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.Chat.Hubs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.Chat;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(AppDbContext context, IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Khách hàng khởi tạo hoặc lấy phiên chat đang mở
    /// </summary>
    [HttpPost("sessions")]
    public async Task<IActionResult> InitOrCreateSession([FromBody] CreateSessionRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { success = false, message = "Dữ liệu yêu cầu không hợp lệ" });
        }

        // Kiểm tra xem khách đã có phiên đang chờ hoặc đang hoạt động theo SĐT chưa
        if (!string.IsNullOrWhiteSpace(request.Sdt))
        {
            var existingSession = await _context.PhienChats
                .Where(p => p.KhachHangSdt == request.Sdt && p.TrangThai != "CLOSED")
                .OrderByDescending(p => p.NgayTao)
                .FirstOrDefaultAsync();

            if (existingSession != null)
            {
                return Ok(new { success = true, data = existingSession });
            }
        }

        // Tìm MaKH nếu số điện thoại này đã có trong bảng KhachHang
        string? maKh = null;
        if (!string.IsNullOrWhiteSpace(request.Sdt))
        {
            var kh = await _context.KhachHangs.FirstOrDefaultAsync(k => k.Sdt == request.Sdt);
            if (kh != null)
            {
                maKh = kh.MaKH;
            }
        }

        var newSession = new PhienChat
        {
            MaPhienChat = $"CHAT_{DateTime.UtcNow:yyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..4].ToUpper()}",
            MaKH = maKh,
            KhachHangTen = string.IsNullOrWhiteSpace(request.HoTen) ? "Khách hàng" : request.HoTen.Trim(),
            KhachHangSdt = request.Sdt?.Trim(),
            TrangThai = "WAITING",
            NgayTao = DateTime.UtcNow
        };

        _context.PhienChats.Add(newSession);

        // Tạo tin nhắn chào tự động ban đầu từ BOT
        var welcomeMsg = new TinNhan
        {
            MaPhienChat = newSession.MaPhienChat,
            NguoiGui = "BOT",
            NoiDung = "👋 Chào mừng bạn đến với Cửa hàng Điện thoại SamaMobile! Bạn cần tư vấn về dòng điện thoại hoặc phụ kiện nào ạ?",
            ThoiGian = DateTime.UtcNow,
            DaDoc = false
        };
        _context.TinNhans.Add(welcomeMsg);

        await _context.SaveChangesAsync();

        // Báo cho toàn thể nhân viên CSKH trong phòng trực AgentPool biết có phiên chat mới vừa mở
        await _hubContext.Clients.Group("AgentPool").SendAsync("NewWaitingSession", new
        {
            newSession.MaPhienChat,
            newSession.KhachHangTen,
            newSession.KhachHangSdt,
            newSession.TrangThai,
            newSession.NgayTao,
            firstMessage = welcomeMsg.NoiDung
        });

        return Ok(new { success = true, data = newSession });
    }

    /// <summary>
    /// Lấy chi tiết thông tin một phiên chat
    /// </summary>
    [HttpGet("sessions/{maPhienChat}")]
    public async Task<IActionResult> GetSession(string maPhienChat)
    {
        var session = await _context.PhienChats
            .Include(p => p.NhanVien)
            .FirstOrDefaultAsync(p => p.MaPhienChat == maPhienChat);

        if (session == null)
        {
            return NotFound(new { success = false, message = "Không tìm thấy phiên chat" });
        }

        return Ok(new
        {
            success = true,
            data = new
            {
                session.MaPhienChat,
                session.MaKH,
                session.KhachHangTen,
                session.KhachHangSdt,
                session.MaNV,
                NhanVienTen = session.NhanVien?.HoTen,
                session.TrangThai,
                session.NgayTao,
                session.NgayKetThuc
            }
        });
    }

    /// <summary>
    /// Lấy lịch sử tin nhắn của một phiên chat
    /// </summary>
    [HttpGet("sessions/{maPhienChat}/messages")]
    public async Task<IActionResult> GetMessages(string maPhienChat)
    {
        var messages = await _context.TinNhans
            .Where(m => m.MaPhienChat == maPhienChat)
            .OrderBy(m => m.ThoiGian)
            .Select(m => new
            {
                m.MaTinNhan,
                m.MaPhienChat,
                m.NguoiGui,
                m.NguoiGuiId,
                m.NoiDung,
                m.DaDoc,
                m.ThoiGian
            })
            .ToListAsync();

        return Ok(new { success = true, data = messages });
    }

    /// <summary>
    /// [Dành cho CSKH/Admin] Lấy danh sách các phiên chat đang chờ hoặc đang hoạt động
    /// </summary>
    [HttpGet("admin/waiting-sessions")]
    public async Task<IActionResult> GetWaitingSessions([FromQuery] string? trangThai = null)
    {
        var query = _context.PhienChats.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(trangThai))
        {
            query = query.Where(p => p.TrangThai == trangThai.ToUpper());
        }
        else
        {
            query = query.Where(p => p.TrangThai == "WAITING" || p.TrangThai == "ACTIVE");
        }

        var sessions = await query
            .OrderByDescending(p => p.NgayTao)
            .Select(p => new
            {
                p.MaPhienChat,
                p.MaKH,
                p.KhachHangTen,
                p.KhachHangSdt,
                p.MaNV,
                p.TrangThai,
                p.NgayTao,
                p.NgayKetThuc,
                TinNhanCuoi = _context.TinNhans
                    .Where(m => m.MaPhienChat == p.MaPhienChat)
                    .OrderByDescending(m => m.ThoiGian)
                    .Select(m => m.NoiDung)
                    .FirstOrDefault(),
                SoTinChuaDoc = _context.TinNhans
                    .Count(m => m.MaPhienChat == p.MaPhienChat && !m.DaDoc && m.NguoiGui == "CUSTOMER")
            })
            .ToListAsync();

        return Ok(new { success = true, data = sessions });
    }

    /// <summary>
    /// [Dành cho CSKH/Admin] Nhân viên nhận xử lý phiên chat
    /// </summary>
    [HttpPost("admin/sessions/{maPhienChat}/assign")]
    public async Task<IActionResult> AssignSession(string maPhienChat, [FromBody] AssignSessionRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.MaNV))
        {
            return BadRequest(new { success = false, message = "Mã nhân viên (MaNV) là bắt buộc" });
        }

        var session = await _context.PhienChats.FirstOrDefaultAsync(p => p.MaPhienChat == maPhienChat);
        if (session == null)
        {
            return NotFound(new { success = false, message = "Không tìm thấy phiên chat" });
        }

        // Kiểm tra nhân viên có tồn tại trong hệ thống không
        var nv = await _context.NhanViens.FirstOrDefaultAsync(n => n.MaNV == request.MaNV);
        if (nv == null)
        {
            return BadRequest(new { success = false, message = $"Không tìm thấy nhân viên có mã {request.MaNV}" });
        }

        session.MaNV = request.MaNV;
        session.TrangThai = "ACTIVE";
        await _context.SaveChangesAsync();

        // Bắn realtime thông báo cho phiên chat và agent pool
        await _hubContext.Clients.Group($"Session_{maPhienChat}").SendAsync("SessionAssigned", new
        {
            maPhienChat,
            maNV = session.MaNV,
            hoTenNV = nv.HoTen,
            trangThai = "ACTIVE"
        });

        await _hubContext.Clients.Group("AgentPool").SendAsync("SessionStatusChanged", new
        {
            maPhienChat,
            trangThai = "ACTIVE",
            maNV = session.MaNV
        });

        return Ok(new { success = true, message = "Đã tiếp nhận phiên chat thành công", data = session });
    }

    /// <summary>
    /// [Dành cho CSKH/Admin/Khách] Đóng phiên chat
    /// </summary>
    [HttpPost("sessions/{maPhienChat}/close")]
    public async Task<IActionResult> CloseSession(string maPhienChat)
    {
        var session = await _context.PhienChats.FirstOrDefaultAsync(p => p.MaPhienChat == maPhienChat);
        if (session == null)
        {
            return NotFound(new { success = false, message = "Không tìm thấy phiên chat" });
        }

        session.TrangThai = "CLOSED";
        session.NgayKetThuc = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // Thông báo cho cả phòng chat rằng phiên đã kết thúc
        await _hubContext.Clients.Group($"Session_{maPhienChat}").SendAsync("SessionClosed", new
        {
            maPhienChat,
            trangThai = "CLOSED",
            ngayKetThuc = session.NgayKetThuc
        });

        await _hubContext.Clients.Group("AgentPool").SendAsync("SessionStatusChanged", new
        {
            maPhienChat,
            trangThai = "CLOSED"
        });

        return Ok(new { success = true, message = "Đã đóng phiên chat thành công" });
    }
}

public class CreateSessionRequest
{
    public string? HoTen { get; set; }
    public string? Sdt { get; set; }
}

public class AssignSessionRequest
{
    public string MaNV { get; set; } = string.Empty;
}
