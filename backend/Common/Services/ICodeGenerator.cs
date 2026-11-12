namespace PhoneStoreAPI.Common.Services;

public interface ICodeGenerator
{
    Task<string> NextAsync(string prefix, string? sequenceName = null);

    /// <summary>
    /// Sinh mã đơn hàng ngẫu nhiên dạng "DH-XXXXXXX" (6 ký tự từ charset an toàn),
    /// kiểm tra trùng qua existsCheck, tối đa 10 lần thử.
    /// </summary>
    Task<string> GenerateOrderCode(Func<string, Task<bool>> existsCheck);
}
