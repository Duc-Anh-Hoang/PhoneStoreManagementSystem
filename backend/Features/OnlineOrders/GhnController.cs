using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;
using System.Text.Json;

namespace PhoneStoreAPI.Features.OnlineOrders;

[ApiController]
[Route("api/ghn")]
public class GhnController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly ILogger<GhnController> _logger;

    public GhnController(
        IConfiguration config,
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        ILogger<GhnController> logger)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _logger = logger;
    }

    private string GetBaseUrl() =>
        (_config["GHN_BASE_URL"] ?? Environment.GetEnvironmentVariable("GHN_BASE_URL") ?? "https://dev-online-gateway.ghn.vn").TrimEnd('/');

    private string? GetToken() =>
        _config["GHN_TOKEN"] ?? Environment.GetEnvironmentVariable("GHN_TOKEN");

    private string? GetShopId() =>
        _config["GHN_SHOP_ID"] ?? Environment.GetEnvironmentVariable("GHN_SHOP_ID") ?? "239081";

    /// <summary>
    /// GET /api/ghn/provinces - Danh sách tỉnh / thành phố (Cache 24h)
    /// </summary>
    [HttpGet("provinces")]
    public async Task<IActionResult> GetProvinces()
    {
        const string cacheKey = "ghn_provinces";
        if (_cache.TryGetValue(cacheKey, out object? cached) && cached != null)
        {
            return Ok(cached);
        }

        var token = GetToken();
        if (string.IsNullOrWhiteSpace(token))
            return BadRequest(new { message = "Chưa cấu hình GHN Token." });

        try
        {
            var client = _httpClientFactory.CreateClient("ghn");
            using var req = new HttpRequestMessage(HttpMethod.Get, $"{GetBaseUrl()}/shiip/public-api/master-data/province");
            req.Headers.TryAddWithoutValidation("Token", token);

            using var res = await client.SendAsync(req);
            if (!res.IsSuccessStatusCode)
                return StatusCode((int)res.StatusCode, new { message = "Lỗi khi lấy danh sách tỉnh thành từ GHN." });

            using var doc = await JsonDocument.ParseAsync(await res.Content.ReadAsStreamAsync());
            var data = doc.RootElement.TryGetProperty("data", out var d) ? d.Clone() : default;

            _cache.Set(cacheKey, data, TimeSpan.FromHours(24));
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi kết nối GHN lấy danh sách tỉnh thành");
            return StatusCode(500, new { message = "Không thể kết nối đến hệ thống GHN." });
        }
    }

    /// <summary>
    /// GET /api/ghn/districts?provinceId=202 - Danh sách quận / huyện theo tỉnh (Cache 24h)
    /// </summary>
    [HttpGet("districts")]
    public async Task<IActionResult> GetDistricts([FromQuery] int provinceId)
    {
        if (provinceId <= 0) return BadRequest(new { message = "Mã tỉnh không hợp lệ." });

        string cacheKey = $"ghn_districts_{provinceId}";
        if (_cache.TryGetValue(cacheKey, out object? cached) && cached != null)
        {
            return Ok(cached);
        }

        var token = GetToken();
        if (string.IsNullOrWhiteSpace(token))
            return BadRequest(new { message = "Chưa cấu hình GHN Token." });

        try
        {
            var client = _httpClientFactory.CreateClient("ghn");
            using var req = new HttpRequestMessage(HttpMethod.Post, $"{GetBaseUrl()}/shiip/public-api/master-data/district");
            req.Headers.TryAddWithoutValidation("Token", token);
            req.Content = JsonContent.Create(new { province_id = provinceId });

            using var res = await client.SendAsync(req);
            if (!res.IsSuccessStatusCode)
                return StatusCode((int)res.StatusCode, new { message = "Lỗi khi lấy danh sách quận huyện từ GHN." });

            using var doc = await JsonDocument.ParseAsync(await res.Content.ReadAsStreamAsync());
            var data = doc.RootElement.TryGetProperty("data", out var d) ? d.Clone() : default;

            _cache.Set(cacheKey, data, TimeSpan.FromHours(24));
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi kết nối GHN lấy danh sách quận huyện cho tỉnh {ProvinceId}", provinceId);
            return StatusCode(500, new { message = "Không thể kết nối đến hệ thống GHN." });
        }
    }

    /// <summary>
    /// GET /api/ghn/wards?districtId=3695 - Danh sách phường / xã theo quận (Cache 24h)
    /// </summary>
    [HttpGet("wards")]
    public async Task<IActionResult> GetWards([FromQuery] int districtId)
    {
        if (districtId <= 0) return BadRequest(new { message = "Mã quận không hợp lệ." });

        string cacheKey = $"ghn_wards_{districtId}";
        if (_cache.TryGetValue(cacheKey, out object? cached) && cached != null)
        {
            return Ok(cached);
        }

        var token = GetToken();
        if (string.IsNullOrWhiteSpace(token))
            return BadRequest(new { message = "Chưa cấu hình GHN Token." });

        try
        {
            var client = _httpClientFactory.CreateClient("ghn");
            using var req = new HttpRequestMessage(HttpMethod.Get, $"{GetBaseUrl()}/shiip/public-api/master-data/ward?district_id={districtId}");
            req.Headers.TryAddWithoutValidation("Token", token);

            using var res = await client.SendAsync(req);
            if (!res.IsSuccessStatusCode)
                return StatusCode((int)res.StatusCode, new { message = "Lỗi khi lấy danh sách phường xã từ GHN." });

            using var doc = await JsonDocument.ParseAsync(await res.Content.ReadAsStreamAsync());
            var data = doc.RootElement.TryGetProperty("data", out var d) ? d.Clone() : default;

            _cache.Set(cacheKey, data, TimeSpan.FromHours(24));
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi kết nối GHN lấy danh sách phường xã cho quận {DistrictId}", districtId);
            return StatusCode(500, new { message = "Không thể kết nối đến hệ thống GHN." });
        }
    }

    /// <summary>
    /// POST /api/ghn/fee - Tính cước phí vận chuyển GHN
    /// </summary>
    [HttpPost("fee")]
    public async Task<IActionResult> CalculateFee([FromBody] GhnFeeRequest request)
    {
        var token = GetToken();
        var shopId = GetShopId();

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(shopId))
        {
            // Trả fallback fee theo tài liệu
            bool isHcm = request.ToDistrictId == 3695 || (request.ToDistrictId >= 1442 && request.ToDistrictId <= 1463);
            int fallbackFee = isHcm ? 25000 : 40000;
            return Ok(new { total = fallbackFee, service_fee = fallbackFee, insurance_fee = 0, source = "fallback" });
        }

        try
        {
            var client = _httpClientFactory.CreateClient("ghn");
            using var req = new HttpRequestMessage(HttpMethod.Post, $"{GetBaseUrl()}/shiip/public-api/v2/shipping-order/fee");
            req.Headers.TryAddWithoutValidation("Token", token);
            req.Headers.TryAddWithoutValidation("ShopId", shopId);

            var payload = new
            {
                service_type_id = request.ServiceTypeId > 0 ? request.ServiceTypeId : 2,
                from_district_id = 3695, // Kho Lê Văn Việt, TP. Thủ Đức
                to_district_id = request.ToDistrictId,
                to_ward_code = request.ToWardCode,
                weight = request.Weight > 0 ? request.Weight : 500,
                length = request.Length > 0 ? request.Length : 20,
                width = request.Width > 0 ? request.Width : 12,
                height = request.Height > 0 ? request.Height : 8,
                insurance_value = Math.Min(5_000_000, request.InsuranceValue)
            };

            req.Content = JsonContent.Create(payload);

            using var res = await client.SendAsync(req);
            if (!res.IsSuccessStatusCode)
            {
                bool isHcm = request.ToDistrictId == 3695 || (request.ToDistrictId >= 1442 && request.ToDistrictId <= 1463);
                int fallbackFee = isHcm ? 25000 : 40000;
                return Ok(new { total = fallbackFee, service_fee = fallbackFee, insurance_fee = 0, source = "fallback" });
            }

            using var doc = await JsonDocument.ParseAsync(await res.Content.ReadAsStreamAsync());
            var data = doc.RootElement.TryGetProperty("data", out var d) ? d.Clone() : default;
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Tính phí GHN lỗi, chuyển sang fallback");
            bool isHcm = request.ToDistrictId == 3695 || (request.ToDistrictId >= 1442 && request.ToDistrictId <= 1463);
            int fallbackFee = isHcm ? 25000 : 40000;
            return Ok(new { total = fallbackFee, service_fee = fallbackFee, insurance_fee = 0, source = "fallback" });
        }
    }

    /// <summary>
    /// POST /api/shipping/quote & POST /api/ghn/quote - Hợp đồng tính phí chuẩn theo plan (7).md
    /// </summary>
    [HttpPost("quote")]
    [HttpPost("/api/shipping/quote")]
    public async Task<IActionResult> ShippingQuote([FromBody] ShippingQuoteRequest request)
    {
        // 1. Nhận tại cửa hàng: phí = 0
        if (string.Equals(request.Fulfillment, "pickup", StringComparison.OrdinalIgnoreCase))
        {
            return Ok(new ShippingQuoteResponse
            {
                ActualFee = 0,
                CustomerFee = 0,
                IsFreeShip = false,
                FreeShipSaved = 0,
                ExpectedDeliveryAt = null,
                Source = "pickup"
            });
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
                    insurance_value = Math.Min(5_000_000, (int)request.SubtotalAfterDiscount)
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

        return Ok(new ShippingQuoteResponse
        {
            ActualFee = actualFee,
            CustomerFee = customerFee,
            IsFreeShip = isFreeShip,
            FreeShipSaved = freeShipSaved,
            ExpectedDeliveryAt = expectedDelivery,
            Source = source
        });
    }

    /// <summary>
    /// POST /api/ghn/orders/{orderCode}/print-url - Sinh URL in phiếu giao vận GHN khổ A5
    /// </summary>
    [HttpPost("orders/{orderCode}/print-url")]
    [HttpPost("/api/donhangonline/{orderCode}/shipping/print-url")]
    public async Task<IActionResult> GeneratePrintUrl(string orderCode)
    {
        var token = GetToken();
        var shopId = GetShopId();
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(shopId))
            return BadRequest(new { message = "Chưa cấu hình GHN Token." });

        try
        {
            var client = _httpClientFactory.CreateClient("ghn");
            client.Timeout = TimeSpan.FromSeconds(5);

            using var req = new HttpRequestMessage(HttpMethod.Post, $"{GetBaseUrl()}/shiip/public-api/v2/a5/gen-token");
            req.Headers.TryAddWithoutValidation("Token", token);
            req.Headers.TryAddWithoutValidation("ShopId", shopId);
            req.Content = JsonContent.Create(new { order_codes = new[] { orderCode } });

            using var res = await client.SendAsync(req);
            if (!res.IsSuccessStatusCode)
                return StatusCode((int)res.StatusCode, new { message = "Không thể tạo mã in vận đơn từ GHN." });

            using var doc = await JsonDocument.ParseAsync(await res.Content.ReadAsStreamAsync());
            var printToken = doc.RootElement.TryGetProperty("data", out var d) && d.TryGetProperty("token", out var t)
                ? t.GetString() : null;

            if (string.IsNullOrWhiteSpace(printToken))
                return BadRequest(new { message = "Không nhận được token in từ GHN." });

            var printUrl = $"{GetBaseUrl()}/a5/public-api/printA5?token={Uri.EscapeDataString(printToken)}";
            return Ok(new { printToken, url = printUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi sinh mã in GHN cho đơn {OrderCode}", orderCode);
            return StatusCode(500, new { message = "Lỗi kết nối GHN." });
        }
    }
}

public class GhnFeeRequest
{
    public int ToDistrictId { get; set; }
    public string ToWardCode { get; set; } = string.Empty;
    public int Weight { get; set; } = 500;
    public int Length { get; set; } = 20;
    public int Width { get; set; } = 12;
    public int Height { get; set; } = 8;
    public int ServiceTypeId { get; set; } = 2;
    public int InsuranceValue { get; set; } = 0;
}

public class ShippingQuoteItem
{
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
}

public class ShippingQuoteRequest
{
    public int ToDistrictId { get; set; }
    public string ToWardCode { get; set; } = string.Empty;
    public decimal SubtotalAfterDiscount { get; set; }
    public string Fulfillment { get; set; } = "delivery"; // "delivery" | "pickup"
    public List<ShippingQuoteItem>? Items { get; set; }
}

public class ShippingQuoteResponse
{
    public int ActualFee { get; set; }
    public int CustomerFee { get; set; }
    public bool IsFreeShip { get; set; }
    public int FreeShipSaved { get; set; }
    public DateTime? ExpectedDeliveryAt { get; set; }
    public string Source { get; set; } = "ghn";
}
