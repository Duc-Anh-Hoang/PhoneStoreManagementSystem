using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Npgsql;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.OnlineOrders.DTOs;
using PhoneStoreAPI.Models.Entities;

namespace PhoneStoreAPI.Features.OnlineOrders;

public class ShippingService : IShippingService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly IMemoryCache _cache;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ShippingService> _logger;

    // Bộ nhớ đệm fallback cho unit test InMemory và cache nhanh
    private static readonly ConcurrentDictionary<string, OrderShippingDto> _memoryStore = new(StringComparer.OrdinalIgnoreCase);
    private static volatile bool _tableInitialized = false;
    private static readonly SemaphoreSlim _initLock = new(1, 1);

    private const string MockCharset = "23456789ABCDEFGHJKMNPQRSTUVWXYZ";

    private static readonly (string GhnCode, string Display, string Description)[] MockStages =
    {
        ("ready_to_pick", "Chờ lấy hàng", "Đơn hàng đã được tạo vận đơn (Chế độ giả lập)"),
        ("picked", "Đang vận chuyển", "Đơn vị vận chuyển đã lấy hàng và đang trung chuyển"),
        ("delivering", "Đang giao hàng", "Bưu tá đang giao hàng đến địa chỉ người nhận"),
        ("delivered", "Đã giao", "Giao hàng thành công đến người nhận")
    };

    public ShippingService(
        AppDbContext context,
        IConfiguration config,
        IMemoryCache cache,
        IHttpClientFactory httpClientFactory,
        ILogger<ShippingService> logger)
    {
        _context = context;
        _config = config;
        _cache = cache;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    private string GetShippingMode()
        => _config["SHIPPING_MODE"] ?? _config["ShippingMode"] ?? Environment.GetEnvironmentVariable("SHIPPING_MODE") ?? "mock";

    private string GetGhnBaseUrl()
        => (_config["GHN_BASE_URL"] ?? Environment.GetEnvironmentVariable("GHN_BASE_URL") ?? "https://dev-online-gateway.ghn.vn").TrimEnd('/');

    private string? GetGhnToken()
        => _config["GHN_TOKEN"] ?? Environment.GetEnvironmentVariable("GHN_TOKEN");

    private string? GetGhnShopId()
        => _config["GHN_SHOP_ID"] ?? Environment.GetEnvironmentVariable("GHN_SHOP_ID");

    private string GetSpxTrackingTemplate()
        => _config["SPX_TRACKING_URL"] ?? Environment.GetEnvironmentVariable("SPX_TRACKING_URL") ?? "https://spx.vn/track?{tracking_code}";

    /// <summary>
    /// Ánh xạ trạng thái GHN sang trạng thái hiển thị tiếng Việt.
    /// Không ném lỗi khi gặp trạng thái lạ (trả "Đang xử lý" và ghi log).
    /// </summary>
    public static string MapGhnStatus(string? ghnStatus, ILogger? logger = null)
    {
        var st = (ghnStatus ?? string.Empty).Trim().ToLowerInvariant();
        return st switch
        {
            "ready_to_pick" or "picking" or "money_collect_picking" => "Chờ lấy hàng",
            "picked" or "storing" or "transporting" or "sorting" => "Đang vận chuyển",
            "delivering" or "money_collect_delivering" => "Đang giao hàng",
            "delivered" => "Đã giao",
            "delivery_fail" or "waiting_to_return" or "return" or "returned"
                or "return_transporting" or "return_sorting" or "returning" or "return_fail" => "Giao thất bại / Hoàn hàng",
            "cancel" => "Đã hủy",
            _ => LogUnknownStatus(st, logger)
        };
    }

    private static string LogUnknownStatus(string status, ILogger? logger)
    {
        if (!string.IsNullOrWhiteSpace(status))
            logger?.LogWarning("[Shipping] Trạng thái GHN chưa xác định: {Status}", status);
        return "Đang xử lý";
    }

    public async Task<OrderShippingDto> CreateShipmentAsync(DonHangOnline order, IReadOnlyList<ChiTietDonHangOnline> items)
    {
        var mode = GetShippingMode();

        if (string.Equals(mode, "ghn_production", StringComparison.OrdinalIgnoreCase))
        {
            // TODO: Cấu hình GHN production base URL (https://online-gateway.ghn.vn) và Token/ShopId production
            _logger.LogWarning("[Shipping] Chế độ ghn_production chưa được cài đặt, fallback về NONE cho đơn {MaDH}", order.MaDH);
            var todoDto = new OrderShippingDto
            {
                Carrier = "NONE",
                ShippingStatus = "Đang xử lý",
                IsDemo = false
            };
            await SaveShipmentAsync(order.MaDH, todoDto);
            return todoDto;
        }

        if (string.Equals(mode, "ghn_staging", StringComparison.OrdinalIgnoreCase))
        {
            var token = GetGhnToken();
            var shopId = GetGhnShopId();
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(shopId))
            {
                _logger.LogWarning("[Shipping] Thiếu GHN_TOKEN hoặc GHN_SHOP_ID cho đơn {MaDH}. Đặt carrier=NONE.", order.MaDH);
                var missingDto = new OrderShippingDto { Carrier = "NONE", ShippingStatus = "Đang xử lý", IsDemo = false };
                await SaveShipmentAsync(order.MaDH, missingDto);
                return missingDto;
            }

            try
            {
                var client = _httpClientFactory.CreateClient("ghn");
                client.Timeout = TimeSpan.FromSeconds(5);

                var addressParts = (order.DiaChiGiao ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries);
                var wardName = addressParts.Length >= 3 ? addressParts[^3].Trim() : "Phường Bến Nghé";
                var districtName = addressParts.Length >= 2 ? addressParts[^2].Trim() : "Quận 1";
                var provinceName = addressParts.Length >= 1 ? addressParts[^1].Trim() : "TP. Hồ Chí Minh";

                var payload = new
                {
                    payment_type_id = order.PhuongThucThanhToan == "cod" ? 2 : 1,
                    required_note = "CHOXEMHANGKHONGTHU",
                    client_order_code = order.MaDH,
                    to_name = order.TenNguoiNhan,
                    to_phone = order.SdtNguoiNhan,
                    to_address = order.DiaChiGiao,
                    to_ward_name = wardName,
                    to_district_name = districtName,
                    to_province_name = provinceName,
                    cod_amount = order.PhuongThucThanhToan == "cod" ? (int)Math.Min(50_000_000m, order.TongTien) : 0,
                    weight = Math.Max(200, items.Sum(i => i.SoLuong) * 300),
                    length = 20,
                    width = 15,
                    height = 10,
                    service_type_id = 2,
                    items = items.Select(i => new
                    {
                        name = i.MatHang?.TenHang ?? i.MaHang,
                        code = i.MaHang,
                        quantity = i.SoLuong,
                        price = (int)i.DonGia
                    }).ToArray()
                };

                using var req = new HttpRequestMessage(HttpMethod.Post, $"{GetGhnBaseUrl()}/shiip/public-api/v2/shipping-order/create");
                // Bảo mật: không bao giờ log Token hoặc ShopId
                req.Headers.TryAddWithoutValidation("Token", token);
                req.Headers.TryAddWithoutValidation("ShopId", shopId);
                req.Content = JsonContent.Create(payload);

                using var res = await client.SendAsync(req);
                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogWarning("[Shipping] GHN staging tạo đơn thất bại (HTTP {Status}) cho {MaDH}", (int)res.StatusCode, order.MaDH);
                    var failDto = new OrderShippingDto { Carrier = "NONE", ShippingStatus = "Đang xử lý", IsDemo = false };
                    await SaveShipmentAsync(order.MaDH, failDto);
                    return failDto;
                }

                using var doc = await JsonDocument.ParseAsync(await res.Content.ReadAsStreamAsync());
                var data = doc.RootElement.TryGetProperty("data", out var d) ? d : default;
                var trackingCode = data.ValueKind == JsonValueKind.Object && data.TryGetProperty("order_code", out var oc)
                    ? oc.GetString() : null;
                DateTime? expected = null;
                if (data.ValueKind == JsonValueKind.Object && data.TryGetProperty("expected_delivery_time", out var edt) &&
                    DateTime.TryParse(edt.GetString(), out var parsedDt))
                {
                    expected = parsedDt.ToUniversalTime();
                }

                var createdDto = new OrderShippingDto
                {
                    Carrier = string.IsNullOrWhiteSpace(trackingCode) ? "NONE" : "GHN",
                    TrackingCode = trackingCode,
                    ShippingStatus = "Chờ lấy hàng",
                    ExpectedDelivery = expected,
                    TrackingUrl = string.IsNullOrWhiteSpace(trackingCode) ? null : $"https://donhang.ghn.vn/?order_code={Uri.EscapeDataString(trackingCode)}",
                    IsDemo = false,
                    Events = new List<ShippingEventDto>
                    {
                        new()
                        {
                            Time = DateTime.UtcNow,
                            Status = "ready_to_pick",
                            Description = "Đã tạo vận đơn trên GHN Staging",
                            Location = "Kho SamaMobile"
                        }
                    }
                };

                await SaveShipmentAsync(order.MaDH, createdDto);
                return createdDto;
            }
            catch (Exception ex)
            {
                // Không làm hỏng đơn nếu API GHN lỗi hoặc timeout
                _logger.LogWarning(ex, "[Shipping] Lỗi kết nối GHN staging khi tạo vận đơn cho {MaDH}", order.MaDH);
                var errDto = new OrderShippingDto { Carrier = "NONE", ShippingStatus = "Đang xử lý", IsDemo = false };
                await SaveShipmentAsync(order.MaDH, errDto);
                return errDto;
            }
        }

        // Chế độ mock (mặc định): sinh mã vận đơn giả GHNMOCK...
        var mockCode = GenerateMockTrackingCode();
        var mockDto = new OrderShippingDto
        {
            Carrier = "GHN",
            TrackingCode = mockCode,
            ShippingStatus = MockStages[0].Display,
            ExpectedDelivery = DateTime.UtcNow.AddDays(3),
            TrackingUrl = null,
            IsDemo = true,
            Events = new List<ShippingEventDto>
            {
                new()
                {
                    Time = DateTime.UtcNow,
                    Status = MockStages[0].GhnCode,
                    Description = MockStages[0].Description,
                    Location = "Kho SamaMobile - 241 Lê Văn Việt"
                }
            }
        };

        await SaveShipmentAsync(order.MaDH, mockDto);
        return mockDto;
    }

    public async Task<OrderShippingDto?> GetTrackingAsync(string maDH)
    {
        var shipment = await LoadShipmentAsync(maDH);
        if (shipment == null) return null;

        var mode = GetShippingMode();

        // Nếu đang ở chế độ ghn_staging và đơn còn đang xử lý → poll GHN (có cache 3 phút)
        if (string.Equals(mode, "ghn_staging", StringComparison.OrdinalIgnoreCase)
            && shipment.Carrier == "GHN"
            && !string.IsNullOrWhiteSpace(shipment.TrackingCode)
            && !IsTerminalStatus(shipment.ShippingStatus))
        {
            var pollCacheKey = $"ghn_poll:{shipment.TrackingCode}";
            if (!_cache.TryGetValue(pollCacheKey, out _))
            {
                await RefreshFromGhnStagingAsync(maDH, shipment);
                _cache.Set(pollCacheKey, true, TimeSpan.FromMinutes(3));
            }
        }

        // Luôn trả timeline mới nhất ở trên
        shipment.Events = shipment.Events.OrderByDescending(e => e.Time).ToList();
        return shipment;
    }

    public async Task<OrderShippingDto?> AdvanceMockShipmentAsync(string maDH)
    {
        var shipment = await LoadShipmentAsync(maDH);
        if (shipment == null) return null;

        var currentIdx = Array.FindIndex(MockStages, s => s.Display == shipment.ShippingStatus);
        var nextIdx = currentIdx < 0 ? 0 : Math.Min(MockStages.Length - 1, currentIdx + 1);
        var stage = MockStages[nextIdx];

        shipment.ShippingStatus = stage.Display;
        if (!shipment.Events.Any(e => e.Status == stage.GhnCode))
        {
            shipment.Events.Insert(0, new ShippingEventDto
            {
                Time = DateTime.UtcNow,
                Status = stage.GhnCode,
                Description = stage.Description,
                Location = nextIdx == 3 ? "Địa chỉ người nhận" : "Trung tâm phân loại TP.HCM"
            });
        }

        await SaveShipmentAsync(maDH, shipment);
        return shipment;
    }

    public async Task<OrderShippingDto> SetSpxTrackingAsync(string maDH, string trackingCode)
    {
        var cleanCode = trackingCode.Trim().ToUpperInvariant();
        var url = GetSpxTrackingTemplate().Replace("{tracking_code}", Uri.EscapeDataString(cleanCode));

        var dto = new OrderShippingDto
        {
            Carrier = "SPX",
            TrackingCode = cleanCode,
            ShippingStatus = "Đang vận chuyển",
            TrackingUrl = url,
            IsDemo = string.Equals(GetShippingMode(), "mock", StringComparison.OrdinalIgnoreCase),
            Events = new List<ShippingEventDto>
            {
                new()
                {
                    Time = DateTime.UtcNow,
                    Status = "picked",
                    Description = $"Đã bàn giao cho SPX Express (Mã vận đơn: {cleanCode})",
                    Location = "Kho SamaMobile"
                }
            }
        };

        await SaveShipmentAsync(maDH, dto);
        return dto;
    }

    public async Task<bool> HandleGhnWebhookAsync(GhnWebhookPayload payload, string? webhookSecretHeader)
    {
        var expectedSecret = _config["GHN_WEBHOOK_SECRET"] ?? Environment.GetEnvironmentVariable("GHN_WEBHOOK_SECRET");
        if (!string.IsNullOrWhiteSpace(expectedSecret) &&
            !string.Equals(expectedSecret, webhookSecretHeader, StringComparison.Ordinal))
        {
            _logger.LogWarning("[Shipping] Webhook GHN sai secret header.");
            return false;
        }

        var expectedShopId = GetGhnShopId();
        if (!string.IsNullOrWhiteSpace(expectedShopId) && payload.ShopID.HasValue &&
            !string.Equals(expectedShopId, payload.ShopID.Value.ToString(), StringComparison.Ordinal))
        {
            _logger.LogWarning("[Shipping] Webhook GHN ShopID không khớp.");
            return false;
        }

        var maDH = payload.ClientOrderCode?.Trim();
        if (string.IsNullOrWhiteSpace(maDH)) return false;

        var shipment = await LoadShipmentAsync(maDH);
        if (shipment == null) return false;
        if (!string.IsNullOrWhiteSpace(shipment.TrackingCode) &&
            !string.IsNullOrWhiteSpace(payload.OrderCode) &&
            !string.Equals(shipment.TrackingCode, payload.OrderCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var mappedStatus = MapGhnStatus(payload.Status, _logger);
        shipment.ShippingStatus = mappedStatus;
        shipment.Events.Insert(0, new ShippingEventDto
        {
            Time = (payload.Time ?? DateTime.UtcNow).ToUniversalTime(),
            Status = payload.Status ?? "unknown",
            Description = string.IsNullOrWhiteSpace(payload.Description) ? mappedStatus : payload.Description.Trim(),
            Location = "GHN"
        });

        await SaveShipmentAsync(maDH, shipment);
        return true;
    }

    private async Task RefreshFromGhnStagingAsync(string maDH, OrderShippingDto shipment)
    {
        var token = GetGhnToken();
        var shopId = GetGhnShopId();
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(shopId)) return;

        try
        {
            var client = _httpClientFactory.CreateClient("ghn");
            client.Timeout = TimeSpan.FromSeconds(5);

            using var req = new HttpRequestMessage(
                HttpMethod.Get,
                $"{GetGhnBaseUrl()}/shiip/public-api/v2/shipping-order/detail?order_code={Uri.EscapeDataString(shipment.TrackingCode!)}");
            req.Headers.TryAddWithoutValidation("Token", token);
            req.Headers.TryAddWithoutValidation("ShopId", shopId);

            using var res = await client.SendAsync(req);
            if (!res.IsSuccessStatusCode) return;

            using var doc = await JsonDocument.ParseAsync(await res.Content.ReadAsStreamAsync());
            if (!doc.RootElement.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Object) return;

            var ghnStatus = data.TryGetProperty("status", out var stEl) ? stEl.GetString() : null;
            var mapped = MapGhnStatus(ghnStatus, _logger);
            shipment.ShippingStatus = mapped;

            if (data.TryGetProperty("leadtime", out var ltEl) && DateTime.TryParse(ltEl.GetString(), out var lt))
                shipment.ExpectedDelivery = lt.ToUniversalTime();

            if (data.TryGetProperty("log", out var logs) && logs.ValueKind == JsonValueKind.Array)
            {
                var parsedEvents = new List<ShippingEventDto>();
                foreach (var entry in logs.EnumerateArray())
                {
                    var st = entry.TryGetProperty("status", out var es) ? es.GetString() ?? "" : "";
                    var desc = MapGhnStatus(st, _logger);
                    var time = entry.TryGetProperty("updated_date", out var ut) && DateTime.TryParse(ut.GetString(), out var dt)
                        ? dt.ToUniversalTime()
                        : DateTime.UtcNow;
                    parsedEvents.Add(new ShippingEventDto { Time = time, Status = st, Description = desc, Location = "GHN" });
                }
                if (parsedEvents.Count > 0) shipment.Events = parsedEvents;
            }

            await SaveShipmentAsync(maDH, shipment);
        }
        catch (Exception ex)
        {
            // Lỗi/timeout từ GHN: giữ nguyên trạng thái đã lưu lần cuối
            _logger.LogWarning(ex, "[Shipping] Không thể cập nhật hành trình GHN cho {MaDH}, dùng trạng thái cache.", maDH);
        }
    }

    private static bool IsTerminalStatus(string? status)
        => status is "Đã giao" or "Đã hủy" or "Giao thất bại / Hoàn hàng";

    private static string GenerateMockTrackingCode()
    {
        var bytes = RandomNumberGenerator.GetBytes(6);
        var chars = new char[6];
        for (int i = 0; i < 6; i++)
            chars[i] = MockCharset[bytes[i] % MockCharset.Length];
        return "GHNMOCK" + new string(chars);
    }

    private async Task EnsureTableAsync(string connStr)
    {
        if (_tableInitialized) return;
        await _initLock.WaitAsync();
        try
        {
            if (_tableInitialized) return;
            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS donhang_vandon (
                    madh VARCHAR(15) PRIMARY KEY,
                    carrier VARCHAR(10) NOT NULL DEFAULT 'NONE',
                    tracking_code VARCHAR(50),
                    shipping_status VARCHAR(80) NOT NULL DEFAULT '',
                    expected_delivery TIMESTAMPTZ,
                    tracking_url TEXT,
                    is_demo BOOLEAN NOT NULL DEFAULT TRUE,
                    events_json TEXT NOT NULL DEFAULT '[]',
                    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
                );";
            await cmd.ExecuteNonQueryAsync();
            _tableInitialized = true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[Shipping] Không thể tạo bảng donhang_vandon, dùng memory store.");
        }
        finally
        {
            _initLock.Release();
        }
    }

    private async Task SaveShipmentAsync(string maDH, OrderShippingDto dto)
    {
        _memoryStore[maDH] = dto;

        if (!_context.Database.IsRelational()) return;
        var connStr = _context.Database.GetConnectionString();
        if (string.IsNullOrWhiteSpace(connStr)) return;

        try
        {
            await EnsureTableAsync(connStr);
            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO donhang_vandon (madh, carrier, tracking_code, shipping_status, expected_delivery, tracking_url, is_demo, events_json, updated_at)
                VALUES (@madh, @carrier, @tracking_code, @shipping_status, @expected_delivery, @tracking_url, @is_demo, @events_json, NOW())
                ON CONFLICT (madh) DO UPDATE SET
                    carrier = EXCLUDED.carrier,
                    tracking_code = EXCLUDED.tracking_code,
                    shipping_status = EXCLUDED.shipping_status,
                    expected_delivery = EXCLUDED.expected_delivery,
                    tracking_url = EXCLUDED.tracking_url,
                    is_demo = EXCLUDED.is_demo,
                    events_json = EXCLUDED.events_json,
                    updated_at = NOW();";
            cmd.Parameters.AddWithValue("madh", maDH);
            cmd.Parameters.AddWithValue("carrier", dto.Carrier);
            cmd.Parameters.AddWithValue("tracking_code", (object?)dto.TrackingCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("shipping_status", dto.ShippingStatus);
            cmd.Parameters.AddWithValue("expected_delivery", (object?)dto.ExpectedDelivery ?? DBNull.Value);
            cmd.Parameters.AddWithValue("tracking_url", (object?)dto.TrackingUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("is_demo", dto.IsDemo);
            cmd.Parameters.AddWithValue("events_json", JsonSerializer.Serialize(dto.Events));
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[Shipping] Lỗi lưu vận đơn vào PostgreSQL cho {MaDH}", maDH);
        }
    }

    private async Task<OrderShippingDto?> LoadShipmentAsync(string maDH)
    {
        if (_memoryStore.TryGetValue(maDH, out var cached))
            return cached;

        if (!_context.Database.IsRelational()) return null;
        var connStr = _context.Database.GetConnectionString();
        if (string.IsNullOrWhiteSpace(connStr)) return null;

        try
        {
            await EnsureTableAsync(connStr);
            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT carrier, tracking_code, shipping_status, expected_delivery, tracking_url, is_demo, events_json
                FROM donhang_vandon WHERE madh = @madh LIMIT 1;";
            cmd.Parameters.AddWithValue("madh", maDH);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            var eventsJson = reader.IsDBNull(6) ? "[]" : reader.GetString(6);
            var events = JsonSerializer.Deserialize<List<ShippingEventDto>>(eventsJson) ?? new List<ShippingEventDto>();

            var dto = new OrderShippingDto
            {
                Carrier = reader.GetString(0),
                TrackingCode = reader.IsDBNull(1) ? null : reader.GetString(1),
                ShippingStatus = reader.GetString(2),
                ExpectedDelivery = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                TrackingUrl = reader.IsDBNull(4) ? null : reader.GetString(4),
                IsDemo = reader.GetBoolean(5),
                Events = events
            };
            _memoryStore[maDH] = dto;
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[Shipping] Lỗi đọc vận đơn từ PostgreSQL cho {MaDH}", maDH);
            return null;
        }
    }
}
