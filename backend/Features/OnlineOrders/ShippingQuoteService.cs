using System.Text.Json;

namespace PhoneStoreAPI.Features.OnlineOrders;

public class ShippingQuoteService(IConfiguration _config, IHttpClientFactory _httpClientFactory, ILogger<ShippingQuoteService> _logger)
{
    private string GetBaseUrl() =>
        (_config["GHN_BASE_URL"] ?? Environment.GetEnvironmentVariable("GHN_BASE_URL") ?? "https://dev-online-gateway.ghn.vn").TrimEnd('/');

    private string? GetToken() =>
        _config["GHN_TOKEN"] ?? Environment.GetEnvironmentVariable("GHN_TOKEN");

    private string? GetShopId() =>
        _config["GHN_SHOP_ID"] ?? Environment.GetEnvironmentVariable("GHN_SHOP_ID") ?? "239081";

    public async Task<ShippingQuoteResponse> CalculateAsync(ShippingQuoteRequest request)
    {
        // 1. Nhận tại cửa hàng: phí = 0
        if (string.Equals(request.Fulfillment, "pickup", StringComparison.OrdinalIgnoreCase))
        {
            return new ShippingQuoteResponse
            {
                ActualFee = 0,
                CustomerFee = 0,
                IsFreeShip = false,
                FreeShipSaved = 0,
                ExpectedDeliveryAt = null,
                Source = "pickup"
            };
        }

        bool isHcm = request.ToDistrictId == 3695 || (request.ToDistrictId >= 1442 && request.ToDistrictId <= 1463);
        int fallbackFee = isHcm ? 25000 : 40000;
        int actualFee = fallbackFee;
        string source = "fallback";
        DateTime? expectedDelivery = DateTime.UtcNow.AddDays(isHcm ? 1 : 3);

        var token = GetToken();
        var shopId = GetShopId();

        if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(shopId) && request.ToDistrictId > 0)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ghn");
                client.Timeout = TimeSpan.FromSeconds(5);

                // Gọi GHN fee API
                using var feeReq = new HttpRequestMessage(HttpMethod.Post, $"{GetBaseUrl()}/shiip/public-api/v2/shipping-order/fee");
                feeReq.Headers.TryAddWithoutValidation("Token", token);
                feeReq.Headers.TryAddWithoutValidation("ShopId", shopId);
                feeReq.Content = JsonContent.Create(new
                {
                    service_type_id = 2,
                    from_district_id = 3695,
                    to_district_id = request.ToDistrictId,
                    to_ward_code = request.ToWardCode,
                    weight = Math.Max(200, (request.Items?.Sum(i => i.Quantity) ?? 1) * 300),
                    length = 20,
                    width = 15,
                    height = 10,
                    insurance_value = Math.Min(5_000_000, (int)Math.Min(5_000_000m, request.SubtotalAfterDiscount))
                });

                using var feeRes = await client.SendAsync(feeReq);
                if (feeRes.IsSuccessStatusCode)
                {
                    using var feeDoc = await JsonDocument.ParseAsync(await feeRes.Content.ReadAsStreamAsync());
                    if (feeDoc.RootElement.TryGetProperty("data", out var d) && d.TryGetProperty("total", out var t))
                    {
                        actualFee = t.GetInt32();
                        source = "ghn";
                    }
                }

                // Gọi GHN leadtime API
                if (!string.IsNullOrWhiteSpace(request.ToWardCode))
                {
                    using var ltReq = new HttpRequestMessage(HttpMethod.Post, $"{GetBaseUrl()}/shiip/public-api/v2/shipping-order/leadtime");
                    ltReq.Headers.TryAddWithoutValidation("Token", token);
                    ltReq.Headers.TryAddWithoutValidation("ShopId", shopId);
                    ltReq.Content = JsonContent.Create(new
                    {
                        from_district_id = 3695,
                        from_ward_code = "90754",
                        to_district_id = request.ToDistrictId,
                        to_ward_code = request.ToWardCode,
                        service_id = 53320
                    });

                    using var ltRes = await client.SendAsync(ltReq);
                    if (ltRes.IsSuccessStatusCode)
                    {
                        using var ltDoc = await JsonDocument.ParseAsync(await ltRes.Content.ReadAsStreamAsync());
                        if (ltDoc.RootElement.TryGetProperty("data", out var ltd) && ltd.TryGetProperty("leadtime_order", out var lto) &&
                            lto.TryGetProperty("to_estimate_date", out var ted) && DateTime.TryParse(ted.GetString(), out var parsedDate))
                        {
                            expectedDelivery = parsedDate.ToUniversalTime();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Gọi GHN tính cước phí/leadtime thất bại, dùng fallback");
            }
        }

        // Chính sách Freeship thông minh theo khu vực (tránh bù lỗ cước vận chuyển khi giao xa):
        // - Nội thành TP.HCM (gần kho Thủ Đức): Đơn từ 5.000.000đ trở lên được Freeship.
        // - Các tỉnh thành khác (toàn quốc): Đơn từ 20.000.000đ trở lên mới được Freeship.
        decimal freeShipThreshold = isHcm ? 5_000_000m : 20_000_000m;
        bool isFreeShip = request.SubtotalAfterDiscount >= freeShipThreshold;
        int customerFee = isFreeShip ? 0 : actualFee;
        int freeShipSaved = isFreeShip ? actualFee : 0;

        return new ShippingQuoteResponse
        {
            ActualFee = actualFee,
            CustomerFee = customerFee,
            IsFreeShip = isFreeShip,
            FreeShipSaved = freeShipSaved,
            ExpectedDeliveryAt = expectedDelivery,
            Source = source
        };
    }
}
