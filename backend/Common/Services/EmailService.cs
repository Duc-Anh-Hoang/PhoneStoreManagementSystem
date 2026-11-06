using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using PhoneStoreAPI.Common.Configuration;

namespace PhoneStoreAPI.Common.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<SmtpSettings> settings, ILogger<EmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody, string? toName = null)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
        {
            _logger.LogWarning("Không thể gửi email: Địa chỉ người nhận trống.");
            return false;
        }

        // Chế độ mô phỏng khi chưa cấu hình mật khẩu SMTP thật
        bool isPlaceholder = string.IsNullOrWhiteSpace(_settings.Password)
            || _settings.Password.Contains("your-smtp-app-password", StringComparison.OrdinalIgnoreCase);

        if (isPlaceholder)
        {
            _logger.LogInformation(
                "[SMTP SIMULATION] Gửi email tới {ToEmail} ({ToName}) | Chủ đề: {Subject}\nNội dung:\n{Body}",
                toEmail, toName ?? "Khách hàng", subject, htmlBody);
            return true;
        }

        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(new MailboxAddress(toName ?? toEmail, toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            
            var socketOption = _settings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
            await client.ConnectAsync(_settings.Host, _settings.Port, socketOption);

            if (!string.IsNullOrWhiteSpace(_settings.Username) && !string.IsNullOrWhiteSpace(_settings.Password))
            {
                var cleanPassword = _settings.Password.Replace(" ", "").Trim();
                await client.AuthenticateAsync(_settings.Username.Trim(), cleanPassword);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Gửi email thành công qua SMTP tới: {ToEmail}", toEmail);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi gửi email qua SMTP tới {ToEmail}: {Message}", toEmail, ex.Message);
            return false;
        }
    }

    public async Task<bool> SendOtpEmailAsync(string toEmail, string otpCode, string purpose = "xác thực tài khoản")
    {
        string subject = $"[Sama Mobile] Mã OTP {purpose}: {otpCode}";
        string htmlBody = $@"
            <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; rounded: 12px;"">
                <div style=""background-color: #0057B7; padding: 16px; border-radius: 8px; text-align: center;"">
                    <h2 style=""color: #ffffff; margin: 0; font-size: 20px;"">SAMA MOBILE</h2>
                </div>
                <div style=""padding: 24px 0;"">
                    <p>Xin chào,</p>
                    <p>Bạn đã yêu cầu mã OTP để <strong>{purpose}</strong> tại hệ thống Sama Mobile.</p>
                    <div style=""background-color: #f1f5f9; padding: 16px; text-align: center; border-radius: 8px; margin: 20px 0;"">
                        <span style=""font-size: 32px; font-weight: bold; letter-spacing: 6px; color: #0057B7;"">{otpCode}</span>
                    </div>
                    <p style=""color: #64748b; font-size: 13px;"">Mã OTP này có hiệu lực trong 5 phút. Vui lòng không chia sẻ mã này cho bất kỳ ai.</p>
                </div>
                <hr style=""border: none; border-top: 1px solid #e2e8f0;"" />
                <p style=""color: #94a3b8; font-size: 12px; text-align: center;"">© 2026 Sama Mobile Store. Mọi quyền được bảo lưu.</p>
            </div>";

        return await SendEmailAsync(toEmail, subject, htmlBody);
    }

    public async Task<bool> SendOrderConfirmationEmailAsync(string toEmail, string customerName, string orderCode, decimal totalAmount, IEnumerable<OrderItemEmailDto> items)
    {
        string subject = $"[Sama Mobile] Xác nhận đơn hàng #{orderCode} thành công";
        
        var rows = string.Join("", items.Select(it => $@"
            <tr style=""border-bottom: 1px solid #f1f5f9;"">
                <td style=""padding: 10px 8px;"">{it.TenHang}</td>
                <td style=""padding: 10px 8px; text-align: center;"">{it.SoLuong}</td>
                <td style=""padding: 10px 8px; text-align: right;"">{it.DonGia:N0} đ</td>
                <td style=""padding: 10px 8px; text-align: right; font-weight: bold;"">{it.ThanhTien:N0} đ</td>
            </tr>
        "));

        string htmlBody = $@"
            <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 12px;"">
                <div style=""background-color: #0057B7; padding: 16px; border-radius: 8px; text-align: center;"">
                    <h2 style=""color: #ffffff; margin: 0; font-size: 20px;"">SAMA MOBILE</h2>
                </div>
                <div style=""padding: 24px 0;"">
                    <h3 style=""color: #0f172a; margin-top: 0;"">Cảm ơn bạn đã đặt hàng!</h3>
                    <p>Xin chào <strong>{customerName}</strong>, đơn hàng <strong>#{orderCode}</strong> của bạn đã được ghi nhận thành công.</p>
                    
                    <table style=""width: 100%; border-collapse: collapse; margin: 20px 0; font-size: 14px;"">
                        <thead>
                            <tr style=""background-color: #f8fafc; border-bottom: 2px solid #e2e8f0;"">
                                <th style=""padding: 10px 8px; text-align: left;"">Sản phẩm</th>
                                <th style=""padding: 10px 8px; text-align: center;"">SL</th>
                                <th style=""padding: 10px 8px; text-align: right;"">Đơn giá</th>
                                <th style=""padding: 10px 8px; text-align: right;"">Thành tiền</th>
                            </tr>
                        </thead>
                        <tbody>
                            {rows}
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan=""3"" style=""padding: 12px 8px; text-align: right; font-weight: bold;"">Tổng thanh toán:</td>
                                <td style=""padding: 12px 8px; text-align: right; font-weight: bold; color: #dc2626; font-size: 16px;"">{totalAmount:N0} đ</td>
                            </tr>
                        </tfoot>
                    </table>

                    <p style=""color: #475569; font-size: 13px;"">Nhân viên Sama Mobile sẽ liên hệ với bạn trong thời gian sớm nhất để xác nhận và giao hàng.</p>
                </div>
                <hr style=""border: none; border-top: 1px solid #e2e8f0;"" />
                <p style=""color: #94a3b8; font-size: 12px; text-align: center;"">Hotline hỗ trợ: 1900 6868 | Email: support@samamobile.vn</p>
            </div>";

        return await SendEmailAsync(toEmail, subject, htmlBody, customerName);
    }

    public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetLinkOrCode)
    {
        string subject = "[Sama Mobile] Yêu cầu đặt lại mật khẩu";
        string htmlBody = $@"
            <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 12px;"">
                <div style=""background-color: #0057B7; padding: 16px; border-radius: 8px; text-align: center;"">
                    <h2 style=""color: #ffffff; margin: 0; font-size: 20px;"">SAMA MOBILE</h2>
                </div>
                <div style=""padding: 24px 0;"">
                    <p>Xin chào,</p>
                    <p>Bạn đã gửi yêu cầu đặt lại mật khẩu cho tài khoản tại Sama Mobile.</p>
                    <div style=""background-color: #f1f5f9; padding: 16px; text-align: center; border-radius: 8px; margin: 20px 0;"">
                        <span style=""font-size: 24px; font-weight: bold; color: #0057B7;"">{resetLinkOrCode}</span>
                    </div>
                    <p style=""color: #64748b; font-size: 13px;"">Nếu bạn không gửi yêu cầu này, vui lòng bỏ qua email và tài khoản của bạn vẫn an toàn.</p>
                </div>
                <hr style=""border: none; border-top: 1px solid #e2e8f0;"" />
                <p style=""color: #94a3b8; font-size: 12px; text-align: center;"">© 2026 Sama Mobile Store.</p>
            </div>";

        return await SendEmailAsync(toEmail, subject, htmlBody);
    }
}
