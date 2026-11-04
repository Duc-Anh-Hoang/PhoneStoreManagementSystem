namespace PhoneStoreAPI.Features.Customers.DTOs;

public class CustomerDto
{
    public string MaKH { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string? DiaChi { get; set; }
    public string? Sdt { get; set; }
    public string? Email { get; set; }
    public int DiemTichLuy { get; set; }
    public decimal TongChiTieu { get; set; }
}

