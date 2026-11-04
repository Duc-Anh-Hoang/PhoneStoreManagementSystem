namespace PhoneStoreAPI.Features.Suppliers.DTOs;

public class SupplierDto
{
    public string MaNCC { get; set; } = string.Empty;
    public string TenNCC { get; set; } = string.Empty;
    public string? DiaChi { get; set; }
    public string? Sdt { get; set; }
    public string? Email { get; set; }
}

