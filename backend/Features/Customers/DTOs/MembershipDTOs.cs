namespace PhoneStoreAPI.Features.Customers.DTOs;

public class MembershipLookupRequest
{
    public string Sdt { get; set; } = string.Empty;
    public decimal CartTotal { get; set; } = 0;
    public bool UsePoints { get; set; } = false;
    public int PointsToUse { get; set; } = 0;
    public string? VoucherCode { get; set; }
}

public class MembershipLookupResponse
{
    public bool Exists { get; set; }
    public string HangThanhVien { get; set; } = "Thành viên";
    public int DiscountPct { get; set; } = 0;
    public decimal DiscountAmount { get; set; } = 0;
    public string TenVietTat { get; set; } = string.Empty; // Masked: "H. Đ. A***"
    public string SdtMasked { get; set; } = string.Empty;   // Masked: "0387***342"
    public bool RequiresOtp { get; set; } = false;
    public string? OtpReason { get; set; }
    public int DiemTichLuyHienCo { get; set; } = 0;
}

public class SendVipOtpRequest
{
    public string Sdt { get; set; } = string.Empty;
}

public class VerifyVipOtpRequest
{
    public string Sdt { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
