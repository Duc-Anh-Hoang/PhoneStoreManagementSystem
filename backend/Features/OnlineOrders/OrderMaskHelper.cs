namespace PhoneStoreAPI.Features.OnlineOrders;

/// <summary>
/// Helper che thông tin nhạy cảm trong đơn hàng để tra cứu công khai (P4-06).
/// </summary>
public static class OrderMaskHelper
{
    /// <summary>
    /// Che tên: giữ họ + chữ đầu tên đệm + *** + chữ cuối tên.
    /// Ví dụ: "Nguyễn Văn An" → "Nguyễn V*** n"
    /// </summary>
    public static string MaskName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "***";

        var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
        {
            // Chỉ có 1 từ: giữ ký tự đầu và ký tự cuối, che giữa
            var word = parts[0];
            if (word.Length <= 2) return word[0] + "***";
            return word[0] + "***" + word[^1];
        }

        // Nhiều từ: họ (từ đầu) + ký tự đầu tên đệm (từ 2..n-1) + *** + ký tự cuối tên (từ cuối)
        var ho = parts[0];
        var tenCuoi = parts[^1];
        // Lấy chữ đầu của tên đệm (nếu có)
        string dem = parts.Length > 2 ? (" " + parts[1][0]) : string.Empty;
        return $"{ho}{dem}*** {tenCuoi[^1]}";
    }

    /// <summary>
    /// Che số điện thoại: giữ 4 số đầu và 3 số cuối, che giữa bằng ***.
    /// Ví dụ: "0987654321" → "0987***321"
    /// </summary>
    public static string MaskPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return "***";

        var clean = phone.Trim();
        if (clean.Length <= 7) return clean[0] + "***" + clean[^1];

        // Giữ 4 đầu + *** + 3 cuối
        return clean[..4] + "***" + clean[^3..];
    }

    /// <summary>
    /// Che email: giữ 2 ký tự đầu local-part + *** + phần domain.
    /// Ví dụ: "abcdef@gmail.com" → "ab***@gmail.com"
    /// </summary>
    public static string MaskEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return string.Empty;

        var atIdx = email.IndexOf('@');
        if (atIdx <= 0) return "***";

        var local = email[..atIdx];
        var domain = email[atIdx..]; // includes '@'

        // Giữ tối đa 2 ký tự đầu
        var keep = local.Length >= 2 ? local[..2] : local[..1];
        return $"{keep}***{domain}";
    }

    /// <summary>
    /// Che địa chỉ: chỉ giữ phần phường/xã, quận/huyện, tỉnh/thành (ẩn số nhà, tên đường).
    /// Quy ước: địa chỉ theo dạng "số nhà, tên đường, phường, quận, tỉnh" (phân cách bằng dấu phẩy).
    /// Nếu ít hơn 3 phần thì trả "***".
    /// </summary>
    public static string MaskAddress(string? address)
    {
        if (string.IsNullOrWhiteSpace(address)) return string.Empty;

        var parts = address.Split(',', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 3) return "***";

        // Giữ 3 phần cuối (phường, quận, tỉnh)
        var kept = parts[^Math.Min(3, parts.Length)..];
        return string.Join(", ", kept.Select(p => p.Trim()));
    }
}
