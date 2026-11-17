namespace PhoneStoreAPI.Common.Configs;

public class CheckoutOptions
{
    public const string SectionName = "Checkout";

    public int ReservationTtlMinutes { get; set; } = 10;
    public int GracePeriodMinutes { get; set; } = 5;
    public int MaxQtyPerOrder { get; set; } = 2;
    public int ExpireJobIntervalMinutes { get; set; } = 1;

    public TimeSpan ReservationTtl => TimeSpan.FromMinutes(ReservationTtlMinutes);
    public TimeSpan GracePeriod => TimeSpan.FromMinutes(GracePeriodMinutes);
    public TimeSpan ExpireJobInterval => TimeSpan.FromMinutes(ExpireJobIntervalMinutes);
}
