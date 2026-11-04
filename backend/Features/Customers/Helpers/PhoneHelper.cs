using System.Text.RegularExpressions;

namespace PhoneStoreAPI.Features.Customers.Helpers;

public static class PhoneHelper
{
    private static readonly Regex CleanRegex = new(@"[\s\.\-\(\)]", RegexOptions.Compiled);
    private static readonly Regex VnPhoneRegex = new(@"^0[35789][0-9]{8}$", RegexOptions.Compiled);

    public static string? NormalizeVietnamesePhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return null;

        var clean = CleanRegex.Replace(phone.Trim(), "");

        if (clean.StartsWith("+84"))
        {
            clean = "0" + clean.Substring(3);
        }
        else if (clean.StartsWith("84") && clean.Length == 11)
        {
            clean = "0" + clean.Substring(2);
        }

        return clean;
    }

    public static bool IsValidVietnamesePhone(string? phone)
    {
        var normalized = NormalizeVietnamesePhone(phone);
        if (string.IsNullOrEmpty(normalized)) return false;
        return VnPhoneRegex.IsMatch(normalized);
    }
}

