namespace PhoneStoreAPI.Common.Services;

public class OrderItemEmailDto
{
    public string TenHang { get; set; } = string.Empty;
    public int SoLuong { get; set; }
    public decimal DonGia { get; set; }
    public decimal ThanhTien => SoLuong * DonGia;
}

public interface IEmailService
{
    Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody, string? toName = null);
    Task<bool> SendOtpEmailAsync(string toEmail, string otpCode, string purpose = "xác thực tài khoản");
    Task<bool> SendOrderConfirmationEmailAsync(string toEmail, string customerName, string orderCode, decimal totalAmount, IEnumerable<OrderItemEmailDto> items);
    Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetLinkOrCode);
}
