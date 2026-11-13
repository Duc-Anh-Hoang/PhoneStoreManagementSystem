using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PhoneStoreAPI.Features.OnlineOrders.Helpers;

public static class AddressHashHelper
{
    private static readonly Regex PunctuationRegex = new(@"[,\.\-\/\s]+", RegexOptions.Compiled);

    public static string ComputeAddressHash(string? address)
    {
        if (string.IsNullOrWhiteSpace(address)) return string.Empty;

        // Bỏ thẻ GHN tag [GHN:...:...] nếu có
        var clean = Regex.Replace(address, @"\[GHN:[^\]]+\]", "").Trim().ToLowerInvariant();
        clean = PunctuationRegex.Replace(clean, " ");

        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(clean);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
