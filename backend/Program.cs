using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Common.Extensions;
using PhoneStoreAPI.Common.Middlewares;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Data.Seeders;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình Database PostgreSQL qua EF Core
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Host=postgres;Port=5432;Database=PhoneStoreDB;Username=postgres;Password=postgres";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// 2. Bước 4: Đăng ký toàn bộ Feature Services và FluentValidation (1 dòng duy nhất)
builder.Services.AddFeatureServices();

// 3. Bước 3: Cấu hình JWT Bearer Authentication & Authorization
builder.Services.AddAppAuth(builder.Configuration);

// 4. Cấu hình CORS
builder.Services.AddAppCors(builder.Configuration);

// 5. Bước 5: Cấu hình Swagger / OpenAPI kèm Bearer Authorize
builder.Services.AddAppSwagger();

// 6. Controllers & JSON Options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

// 7. Seed dữ liệu khởi tạo khi start server
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
        logger.LogError(ex, "Không thể seed dữ liệu khi khởi động.");
    }
}

// 8. Bắt lỗi tập trung (Middleware)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 9. Bước 5: Swagger chỉ bật ở môi trường Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhoneStore API v1");
        c.RoutePrefix = "swagger";
    });
}

// 10. CORS Policy
app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "ProductionCors");

app.UseRouting();

// 11. Bước 3: UseAuthentication() phải đứng TRƯỚC UseAuthorization()
app.UseAuthentication();
app.UseAuthorization();

// 12. Health check endpoint cho Docker & Load Balancer
app.MapGet("/api/health", () => Results.Ok(new
{
    status = "healthy",
    framework = ".NET 10",
    database = "PostgreSQL",
    timestamp = DateTime.UtcNow
}));

app.MapControllers();

app.Run();
