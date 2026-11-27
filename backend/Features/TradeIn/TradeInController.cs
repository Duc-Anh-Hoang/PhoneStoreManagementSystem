using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Features.OnlineOrders.Hubs;

namespace PhoneStoreAPI.Features.TradeIn;

[ApiController]
[Route("api/trade-in/bookings")]
public class TradeInController(AppDbContext db, IHubContext<OrderHub> hubContext) : ControllerBase
{
    private const string BranchAddress = "241 Lê Văn Việt, TP. Thủ Đức, TP.HCM";

    [HttpPost, AllowAnonymous, EnableRateLimiting("checkout"), RequestSizeLimit(16384)]
    public async Task<IActionResult> Create(BookingRequest request, CancellationToken ct)
    {
        if (request.RequestId == Guid.Empty || !HasText(request.TargetProduct, "name") ||
            !HasText(request.UsedDevice, "model") ||
            (request.Branch != BranchAddress && request.Branch != $"Chi nhánh 1: {BranchAddress}"))
            return BadRequest(new { message = "Thông tin phiếu thu cũ chưa đầy đủ." });

        var code = $"TC-{request.RequestId:N}";
        var target = request.TargetProduct.GetRawText();
        var device = request.UsedDevice.GetRawText();
        var updated = await db.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO trade_in_bookings(code,name,phone,branch,target_product,used_device)
            VALUES ({code},{request.Name},{request.Phone},{BranchAddress},CAST({target} AS jsonb),CAST({device} AS jsonb))
            ON CONFLICT (code) DO UPDATE SET code=excluded.code
            WHERE (trade_in_bookings.name, trade_in_bookings.phone, replace(trade_in_bookings.branch, 'Chi nhánh 1: ', ''),
                   trade_in_bookings.target_product, trade_in_bookings.used_device) =
                  (excluded.name, excluded.phone, excluded.branch, excluded.target_product, excluded.used_device)
            """, ct);
        if (updated == 0) return Conflict(new { message = "Mã phiếu đã được sử dụng." });

        try
        {
            await hubContext.Clients.Group(OrderHub.AdminGroup).SendAsync("ReceiveTradeInBooking", new
            {
                code,
                name = request.Name,
                phone = request.Phone,
                branch = BranchAddress,
                createdAt = DateTime.UtcNow
            }, ct);
        }
        catch { /* Không làm gián đoạn phản hồi nếu SignalR chưa có client */ }

        return Ok(new { code });
    }

    private static bool HasText(JsonElement value, string key) =>
        value.ValueKind == JsonValueKind.Object && value.TryGetProperty(key, out var text) &&
        text.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(text.GetString());

    [HttpGet, Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales}")]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] string? phone = null, CancellationToken ct = default)
    {
        var localPhone = phone?.Trim();
        if (localPhone?.StartsWith("+84", StringComparison.Ordinal) == true)
            localPhone = "0" + localPhone[3..];
        var internationalPhone = localPhone?.StartsWith('0') == true ? "+84" + localPhone[1..] : localPhone;
        var showAll = string.IsNullOrEmpty(localPhone);
        var offset = (Math.Clamp(page, 1, 1000000) - 1) * 50;
        var json = await db.Database.SqlQuery<string>($"""
            SELECT coalesce(json_agg(b),'[]')::text AS "Value" FROM
                (SELECT code,name,phone,branch,status,created_at,target_product,used_device FROM trade_in_bookings
                 WHERE {showAll} OR phone IN ({localPhone}, {internationalPhone})
                 ORDER BY created_at DESC,code LIMIT 50 OFFSET {offset}) b
            """).SingleAsync(ct);
        return Content(json, "application/json");
    }

    [HttpGet("{code}"), Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales}")]
    public async Task<IActionResult> Get(string code, CancellationToken ct)
    {
        if (code.StartsWith("TC-", StringComparison.OrdinalIgnoreCase))
            code = "TC-" + code[3..].ToLowerInvariant();
        var json = await db.Database.SqlQuery<string>($"""
            SELECT row_to_json(b)::text AS "Value" FROM trade_in_bookings b WHERE code={code}
            """).SingleOrDefaultAsync(ct);
        return json == null ? NotFound() : Content(json, "application/json");
    }
}

public record BookingRequest(Guid RequestId, [Required, StringLength(100)] string Name,
    [Required, RegularExpression(@"(0|\+84)[35789][0-9]{8}")] string Phone, [Required, StringLength(200)] string Branch,
    JsonElement TargetProduct, JsonElement UsedDevice);
