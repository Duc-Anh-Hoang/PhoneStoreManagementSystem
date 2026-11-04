namespace PhoneStoreAPI.Features.Staff.DTOs;

public class NhanVienDto
{
    public string MaNV { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string ChucVu { get; set; } = string.Empty;
    public string? Sdt { get; set; }
    public string? Email { get; set; }
    public DateTime NgayVaoLam { get; set; }
    public string TrangThai { get; set; } = "dang_lam";
}

public class CreateNhanVienRequest
{
    public string? MaNV { get; set; }
    public string HoTen { get; set; } = string.Empty;
    public string ChucVu { get; set; } = string.Empty;
    public string? Sdt { get; set; }
    public string? Email { get; set; }
    public string? TrangThai { get; set; } = "dang_lam";
}

public class UpdateNhanVienRequest
{
    public string HoTen { get; set; } = string.Empty;
    public string ChucVu { get; set; } = string.Empty;
    public string? Sdt { get; set; }
    public string? Email { get; set; }
    public string TrangThai { get; set; } = "dang_lam";
}
