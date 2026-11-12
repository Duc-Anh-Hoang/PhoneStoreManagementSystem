using PhoneStoreAPI.Features.OnlineOrders.DTOs;

namespace PhoneStoreAPI.Features.OnlineOrders;

public interface IOtpService
{
    /// <summary>
    /// Yêu cầu gửi OTP đến số điện thoại.
    /// Luôn trả cùng một message (khi chưa vượt giới hạn) để tránh lộ thông tin SĐT có đơn hay không.
    /// </summary>
    Task<OtpRequestResult> RequestOtpAsync(string phone);

    /// <summary>
    /// Xác minh OTP. Nếu đúng trả token JWT short-lived (10 phút) claim "phone_lookup".
    /// </summary>
    Task<OtpVerifyResult> VerifyOtpAsync(string phone, string code);
}
