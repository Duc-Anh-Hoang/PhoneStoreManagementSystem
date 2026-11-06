namespace PhoneStoreAPI.Features.OnlineOrders.DTOs;

public class CreateOrderRequest
{
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
