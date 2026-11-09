using PhoneStoreAPI.Features.OnlineOrders.Hubs;
using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Extensions;
using PhoneStoreAPI.Common.Middlewares;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Data.Seeders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình Serilog Logging (P1-10)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{TraceId}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/phonestore-.log", 
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 14,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{TraceId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// 2. Cấu hình Database PostgreSQL qua EF Core
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Host=postgres;Port=5432;Database=PhoneStoreDB;Username=postgres;Password=postgres_secure_pass_2026";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// 3. Đăng ký toàn bộ Feature Services và FluentValidation (1 dòng duy nhất)
builder.Services.AddFeatureServices();

// 4. Cấu hình JWT Bearer Authentication & Authorization
builder.Services.AddAppAuth(builder.Configuration);

// Cấu hình dịch vụ Email SMTP
builder.Services.AddAppEmail(builder.Configuration);

// 5. Cấu hình Rate Limiting (P1-09)
builder.Services.AddAppRateLimiting();

// 6. Cấu hình CORS
builder.Services.AddAppCors(builder.Configuration);

// Đăng ký dịch vụ SignalR phục vụ realtime đơn hàng
builder.Services.AddSignalR();

// 7. Cấu hình Swagger / OpenAPI kèm Bearer Authorize
builder.Services.AddAppSwagger();

// 8. Controllers & JSON Options (camelCase, không bọc ApiResponse theo A1)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

// 9. Bắt lỗi tập trung (P1-01)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 10. Seed dữ liệu khởi tạo khi start server (P1-03)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await DbSeeder.SeedAsync(context, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Không thể seed dữ liệu khi khởi động: {Message}", ex.Message);
    }
}

// 11. Swagger chỉ bật ở môi trường Development (P1-05)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhoneStore API v1");
        c.RoutePrefix = "swagger";
    });
}

// 12. CORS Policy
app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "ProductionCors");

app.UseRouting();

// 13. Rate Limiter Middleware
app.UseRateLimiter();

// 14. Authentication TRƯỚC Authorization
app.UseAuthentication();
app.UseAuthorization();

// 15. Root & Health check endpoints
app.MapGet("/", (HttpContext context) =>
{
    var accept = context.Request.Headers["Accept"].ToString();
    if (accept.Contains("text/html"))
    {
        return Results.Content(@"
<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <title>PhoneStore API Server - SamaMobile</title>
    <style>
        body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; background: #0f172a; color: #f8fafc; display: flex; align-items: center; justify-content: center; height: 100vh; margin: 0; }
        .card { background: #1e293b; border: 1px solid #334155; border-radius: 16px; padding: 32px; max-width: 520px; box-shadow: 0 20px 25px -5px rgba(0,0,0,0.5); text-align: center; }
        h1 { color: #60a5fa; font-size: 22px; margin-bottom: 8px; }
        p { color: #94a3b8; font-size: 14px; line-height: 1.6; margin-bottom: 24px; }
        .badge { display: inline-block; background: #065f46; color: #34d399; font-weight: bold; font-size: 12px; padding: 4px 12px; border-radius: 9999px; margin-bottom: 16px; }
        .btn-group { display: flex; gap: 12px; justify-content: center; flex-wrap: wrap; }
        .btn { display: inline-block; padding: 10px 20px; border-radius: 8px; font-weight: bold; font-size: 13px; text-decoration: none; transition: 0.2s; }
        .btn-primary { background: #2563eb; color: white; }
        .btn-primary:hover { background: #1d4ed8; }
        .btn-secondary { background: #334155; color: #f8fafc; }
        .btn-secondary:hover { background: #475569; }
    </style>
</head>
<body>
    <div class='card'>
        <span class='badge'>● Backend API Đang Hoạt Động</span>
        <h1>SAMAMOBILE.COM - API Server</h1>
        <p>Cổng <strong>:5132</strong> là dịch vụ Backend (.NET 10 Web API, SignalR & PostgreSQL).<br>Giao diện người dùng (Frontend) được mở tại cổng <strong>:3000</strong>.</p>
        <div class='btn-group'>
            <a href='http://localhost:3000' class='btn btn-primary' target='_blank'>Mở Giao Diện Web (Cổng 3000)</a>
            <a href='/swagger' class='btn btn-secondary'>Xem Tài Liệu API (Swagger)</a>
        </div>
    </div>
</body>
</html>", "text/html; charset=utf-8");
    }
    return Results.Ok(new
    {
        service = "PhoneStoreAPI",
        status = "running",
        framework = ".NET 10",
        swagger = "/swagger",
        frontend = "http://localhost:3000"
    });
});

// 15. Health check endpoint cho Docker & Load Balancer
app.MapGet("/api/health", () => Results.Ok(new
{
    status = "healthy",
    framework = ".NET 10",
    database = "PostgreSQL",
    timestamp = DateTime.UtcNow
}));

app.MapControllers();
app.MapHub<OrderHub>("/hubs/order");

try
{
    Log.Information("Khởi động ứng dụng PhoneStoreAPI (.NET 10 & PostgreSQL)...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ứng dụng dừng đột ngột.");
}
finally
{
    Log.CloseAndFlush();
}
