namespace PhoneStoreAPI.Common.Configuration;

public class SmtpSettings
{
    public string Host { get; set; } = "smtp.gmail.com";
    public int Port { get; set; } = 587;
    public bool EnableSsl { get; set; } = true;
    public string SenderName { get; set; } = "Sama Mobile Store";
    public string SenderEmail { get; set; } = "support@samamobile.vn";
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
