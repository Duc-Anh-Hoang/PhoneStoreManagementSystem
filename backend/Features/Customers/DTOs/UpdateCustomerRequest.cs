namespace PhoneStoreAPI.Features.Customers.DTOs;

public class UpdateCustomerRequest
{
    public string HoTen { get; set; } = string.Empty;
    public string Sdt { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? DiaChi { get; set; }
}

