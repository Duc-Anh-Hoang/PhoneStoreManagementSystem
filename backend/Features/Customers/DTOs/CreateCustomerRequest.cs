namespace PhoneStoreAPI.Features.Customers.DTOs;

public class CreateCustomerRequest
{
    public string? MaKH { get; set; }
    public string HoTen { get; set; } = string.Empty;
    public string Sdt { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? DiaChi { get; set; }
}

