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
