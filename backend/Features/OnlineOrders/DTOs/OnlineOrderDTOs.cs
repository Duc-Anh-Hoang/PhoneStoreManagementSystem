namespace PhoneStoreAPI.Features.OnlineOrders.DTOs;

public class CreateOrderRequest
{
    public string? MaDH { get; set; }
    public string TenNguoiNhan { get; set; } = string.Empty;
    public string SdtNguoiNhan { get; set; } = string.Empty;
    public string DiaChiGiao { get; set; } = string.Empty;
    public string PhuongThucThanhToan { get; set; } = "cod"; // cod, chuyen_khoan, the
    public string? MaKH { get; set; }
    public string? GhiChu { get; set; }
    public string? Email { get; set; }
    public List<OrderItemRequest> Items { get; set; } = new();
}

public class OrderItemRequest
{
    public string MaHang { get; set; } = string.Empty;
    public string? TenHang { get; set; }
    public int SoLuong { get; set; }
    public decimal DonGia { get; set; } // Client gửi lên nhưng backend sẽ bỏ qua và đọc từ DB
}

public class OrderDto
{
    public string MaDH { get; set; } = string.Empty;
    public DateTime NgayDat { get; set; }
    public string MaKH { get; set; } = string.Empty;
    public string TenNguoiNhan { get; set; } = string.Empty;
    public string SdtNguoiNhan { get; set; } = string.Empty;
    public string DiaChiGiao { get; set; } = string.Empty;
    public string PhuongThucThanhToan { get; set; } = "cod";
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; } = "cho_xac_nhan";
    public string? MaNV { get; set; }
    public string? SoHD { get; set; }
    public List<OrderItemDto>? Items { get; set; }
}

public class OrderItemDto
{
    public string MaHang { get; set; } = string.Empty;
    public string TenHang { get; set; } = string.Empty;
    public int SoLuong { get; set; }
    public decimal DonGia { get; set; }
    public decimal ThanhTien { get; set; }
}

public class UpdateOrderStatusRequest
{
    public string TrangThai { get; set; } = string.Empty;
    public string? SoHD { get; set; } // Hỗ trợ nếu client gửi kèm
}


public class CancelOrderRequest
{
    public string Sdt { get; set; } = string.Empty;
    public string? LyDo { get; set; }
}

public class BulkApproveRequest
{
    public List<string>? OrderCodes { get; set; }
}

public class BulkApproveResultDto
{
    public int ApprovedCount { get; set; }
    public int FailedCount { get; set; }
    public List<string> ApprovedCodes { get; set; } = new();
    public List<string> ErrorMessages { get; set; } = new();
}

// ─── Giai đoạn 2: DTOs tra cứu công khai (đã che thông tin nhạy cảm) ───────────

/// <summary>
/// Một mốc trong lịch sử trạng thái đơn hàng (dùng cho timeline tra cứu).
/// </summary>
public class OrderStatusHistoryEntry
{
    public string TrangThai { get; set; } = string.Empty;
    public DateTime ThoiGian { get; set; }
    public string? GhiChu { get; set; }
}

/// <summary>
/// DTO tra cứu đơn hàng công khai — các trường nhạy cảm đã được che (mask).
/// </summary>
public class OrderLookupDto
{
    public string MaDH { get; set; } = string.Empty;
    public DateTime NgayDat { get; set; }
    public string TenNguoiNhanMask { get; set; } = string.Empty;  // "Nguyễn V*** n"
    public string SdtMask { get; set; } = string.Empty;           // "0987***321"
    public string? EmailMask { get; set; }                        // "ab***@gmail.com"
    public string DiaChiMask { get; set; } = string.Empty;        // "Phường X, Quận Y, Tỉnh Z"
    public string PhuongThucThanhToan { get; set; } = "cod";
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public List<OrderItemDto>? Items { get; set; }
    /// <summary>Lịch sử trạng thái (timeline). Tạm thời có 1 entry = trạng thái hiện tại.</summary>
    public List<OrderStatusHistoryEntry> StatusHistory { get; set; } = new();
    /// <summary>Thông tin vận chuyển (Giai đoạn 6).</summary>
    public OrderShippingDto? Shipping { get; set; }
}

// ─── Giai đoạn 6: Shipping DTOs ──────────────────────────────────────────────

public class ShippingEventDto
{
    public DateTime Time { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Location { get; set; }
}

public class OrderShippingDto
{
    public string Carrier { get; set; } = "NONE"; // GHN | SPX | NONE
    public string? TrackingCode { get; set; }
    public string ShippingStatus { get; set; } = string.Empty;
    public DateTime? ExpectedDelivery { get; set; }
    public string? TrackingUrl { get; set; }
    public bool IsDemo { get; set; }
    public List<ShippingEventDto> Events { get; set; } = new();
}

public class SetSpxTrackingRequest
{
    public string TrackingCode { get; set; } = string.Empty;
}

public class GhnWebhookPayload
{
    public string? OrderCode { get; set; }
    public string? ClientOrderCode { get; set; }
    public string? Status { get; set; }
    public string? Description { get; set; }
    public long? ShopID { get; set; }
    public DateTime? Time { get; set; }
}

// ─── Giai đoạn 4: OTP DTOs ───────────────────────────────────────────────────

public class OtpRequestDto
{
    public string Sdt { get; set; } = string.Empty;
}

public class OtpVerifyDto
{
    public string Sdt { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
}

/// <summary>Kết quả xác minh OTP nội bộ service.</summary>
public class OtpVerifyResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? Error { get; set; }
}

/// <summary>Response trả về client khi OTP đúng.</summary>
public class OtpVerifyResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

/// <summary>Kết quả yêu cầu gửi OTP.</summary>
public class OtpRequestResult
{
    public bool Blocked { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Code { get; set; } // Chỉ trả về khi OTP_MODE=demo
}
