using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Data;

namespace PhoneStoreAPI.Features.TradeIn;

[ApiController]
[Route("api/trade-in/bookings")]
public class TradeInController(AppDbContext db) : ControllerBase
{
    [HttpPost, AllowAnonymous, EnableRateLimiting("checkout"), RequestSizeLimit(16384)]
    public async Task<IActionResult> Create(BookingRequest request, CancellationToken ct)
    {
        if (request.RequestId == Guid.Empty || request.TargetProduct.ValueKind != JsonValueKind.Object ||
            request.UsedDevice.ValueKind != JsonValueKind.Object)
            return BadRequest(new { message = "Thông tin phiếu thu cũ chưa đầy đủ." });

        var code = $"TC-{request.RequestId:N}";
        var target = request.TargetProduct.GetRawText();
        var device = request.UsedDevice.GetRawText();
        var updated = await db.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO trade_in_bookings(code,name,phone,branch,target_product,used_device)
            VALUES ({code},{request.Name},{request.Phone},{request.Branch},CAST({target} AS jsonb),CAST({device} AS jsonb))
            ON CONFLICT (code) DO UPDATE SET code=excluded.code
            WHERE (trade_in_bookings.name, trade_in_bookings.phone, trade_in_bookings.branch,
                   trade_in_bookings.target_product, trade_in_bookings.used_device) =
                  (excluded.name, excluded.phone, excluded.branch, excluded.target_product, excluded.used_device)
            """, ct);
        return updated == 0 ? Conflict(new { message = "Mã phiếu đã được sử dụng." }) : Ok(new { code });
    }

    [HttpGet, Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales}")]
    public async Task<IActionResult> List([FromQuery] int page = 1, CancellationToken ct = default)
    {
        var offset = (Math.Clamp(page, 1, 1000000) - 1) * 50;
        var json = await db.Database.SqlQuery<string>($"""
            SELECT coalesce(json_agg(b),'[]')::text AS "Value" FROM
                (SELECT code,name,phone,branch,status,created_at FROM trade_in_bookings
                 ORDER BY created_at DESC,code LIMIT 50 OFFSET {offset}) b
            """).SingleAsync(ct);
        return Content(json, "application/json");
    }

    [HttpGet("{code}"), Authorize(Roles = $"{AppConstants.Roles.Admin},{AppConstants.Roles.Manager},{AppConstants.Roles.Sales}")]
    public async Task<IActionResult> Get(string code, CancellationToken ct)
    {
        var json = await db.Database.SqlQuery<string>($"""
            SELECT row_to_json(b)::text AS "Value" FROM trade_in_bookings b WHERE code={code}
            """).SingleOrDefaultAsync(ct);
        return json == null ? NotFound() : Content(json, "application/json");
    }
}

public record BookingRequest(Guid RequestId, [Required, StringLength(100)] string Name,
    [Required, RegularExpression(@"(0|\+84)[35789][0-9]{8}")] string Phone, [Required, StringLength(200)] string Branch,
    JsonElement TargetProduct, JsonElement UsedDevice);
