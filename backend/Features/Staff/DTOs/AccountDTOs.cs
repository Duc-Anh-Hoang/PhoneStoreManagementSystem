namespace PhoneStoreAPI.Features.Staff.DTOs;

public class TaiKhoanDto
{
    public string Username { get; set; } = string.Empty;
    public string MaNV { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string TenNhanVien => HoTen;
    public string MaQuyen { get; set; } = string.Empty;
    public string TenQuyen { get; set; } = string.Empty;
    public string TrangThai { get; set; } = "hoat_dong";
    public DateTime NgayTao { get; set; }
}

public class CreateTaiKhoanRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string MaNV { get; set; } = string.Empty;
    public string MaQuyen { get; set; } = string.Empty;
}

public class UpdateTaiKhoanRequest
{
    public string? MaQuyen { get; set; }
    public string? NewPassword { get; set; }
}

public class PhanQuyenDto
{
    public string MaQuyen { get; set; } = string.Empty;
    public string TenQuyen { get; set; } = string.Empty;
    public string? MoTa { get; set; }
}
