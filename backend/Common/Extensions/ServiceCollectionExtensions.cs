using Microsoft.EntityFrameworkCore;
using PhoneStoreAPI.Data;
using PhoneStoreAPI.Common.Constants;
using PhoneStoreAPI.Features.Payment;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using PhoneStoreAPI.Common.Configuration;
using PhoneStoreAPI.Common.Services;
using PhoneStoreAPI.Features.Auth;
using PhoneStoreAPI.Features.Customers;
using PhoneStoreAPI.Features.Dashboard;
using PhoneStoreAPI.Features.Inventory;
using PhoneStoreAPI.Features.OnlineOrders;
using PhoneStoreAPI.Features.Products;
using PhoneStoreAPI.Features.Products.Attributes;
using PhoneStoreAPI.Features.Products.Categories;
using PhoneStoreAPI.Features.Promotions;
using PhoneStoreAPI.Features.Purchasing;
using PhoneStoreAPI.Features.Sales;
using PhoneStoreAPI.Features.Sales.Returns;
using PhoneStoreAPI.Features.Staff;
using PhoneStoreAPI.Features.Suppliers;
using PhoneStoreAPI.Features.SystemSettings;
using PhoneStoreAPI.Features.Warranty;

namespace PhoneStoreAPI.Common.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Bước 3. Cấu hình JWT Authentication & Authorization
    /// </summary>
    /// <summary>
    /// Cấu hình dịch vụ Email SMTP
    /// </summary>
    public static IServiceCollection AddAppEmail(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }

    public static IServiceCollection AddAppAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "PhoneStoreManagementSystem_SuperSecretKey_2026_AspNetCore10_StrongSecretKey!");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var principal = context.Principal;
                    if (principal?.FindFirstValue("manv") is not string staffId) return;

                    var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
                    var username = principal!.FindFirstValue(ClaimTypes.Name);
                    var currentRole = await db.TaiKhoans.AsNoTracking()
                        .Where(a => a.UserName == username && a.MaNV == staffId &&
                            a.TrangThai == AppConstants.AccountStatus.HoatDong &&
                            a.NhanVien!.TrangThai == AppConstants.EmployeeStatus.DangLam)
                        .Select(a => a.PhanQuyen!.QuyenCoSo ?? a.MaQuyen)
                        .SingleOrDefaultAsync(context.HttpContext.RequestAborted);

                    if (currentRole == null || !principal.IsInRole(currentRole))
                        context.Fail("Quyền tài khoản đã thay đổi. Vui lòng đăng nhập lại.");
                }
            };
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"] ?? "PhoneStoreAPI",
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"] ?? "PhoneStoreClient",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();

        return services;
    }

    /// <summary>
    /// Đăng ký toàn bộ Services, CodeGenerator và CurrentUser
    /// </summary>
    public static IServiceCollection AddFeatureServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddHttpContextAccessor();
        services.AddHttpClient();

        // Common Shared Services
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ICodeGenerator, CodeGenerator>();
        services.AddScoped<IThamSoProvider, ThamSoProvider>();

        // Feature Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<ITaiKhoanService, TaiKhoanService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAttributeService, AttributeService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IPurchasingService, PurchasingService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IPromotionService, PromotionService>();
        services.AddScoped<ISalesService, SalesService>();
        services.AddScoped<IReturnsService, ReturnsService>();
        services.AddScoped<IOnlineOrderService, OnlineOrderService>();
        services.AddScoped<IWarrantyService, WarrantyService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<ISystemSettingService, SystemSettingService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPaymentService, PaymentService>();
        // Checkout & Cart Module
        services.AddScoped<PhoneStoreAPI.Features.Checkout.Services.ICartService, PhoneStoreAPI.Features.Checkout.Services.CartService>();
        services.AddScoped<PhoneStoreAPI.Features.Checkout.Services.IOrderStateMachine, PhoneStoreAPI.Features.Checkout.Services.OrderStateMachine>();
        services.AddScoped<PhoneStoreAPI.Features.Checkout.Services.IOrderService, PhoneStoreAPI.Features.Checkout.Services.OrderService>();
        services.AddScoped<PhoneStoreAPI.Features.Checkout.PaymentProviders.IPaymentProvider, PhoneStoreAPI.Features.Checkout.PaymentProviders.MockQrPaymentProvider>();
        services.AddScoped<PhoneStoreAPI.Features.Checkout.Services.IPaymentWebhookHandler, PhoneStoreAPI.Features.Checkout.Services.PaymentWebhookHandler>();
        services.AddScoped<PhoneStoreAPI.Features.Checkout.Services.IOrderExpirationService, PhoneStoreAPI.Features.Checkout.Services.OrderExpirationService>();
        services.AddScoped<PhoneStoreAPI.Features.Checkout.Services.IRefundAndCancellationService, PhoneStoreAPI.Features.Checkout.Services.RefundAndCancellationService>();
        services.AddScoped<PhoneStoreAPI.Features.Checkout.Services.IReconciliationService, PhoneStoreAPI.Features.Checkout.Services.ReconciliationService>();
        services.AddHostedService<PhoneStoreAPI.Features.Checkout.Background.OrderExpirationBackgroundService>();
        // Giai đoạn 4: Đăng ký OTP Service
        services.AddScoped<IOtpService, OtpService>();
        // Giai đoạn 6: Đăng ký Shipping Service
        services.AddScoped<ShippingQuoteService>();
        services.AddScoped<IShippingService, ShippingService>();
        // plan (8).md: Đăng ký OrderSettingsService & OrderDecisionEngine
        services.AddScoped<PhoneStoreAPI.Features.OnlineOrders.Services.IOrderSettingsService, PhoneStoreAPI.Features.OnlineOrders.Services.OrderSettingsService>();
        services.AddScoped<PhoneStoreAPI.Features.OnlineOrders.Services.IOrderDecisionEngine, PhoneStoreAPI.Features.OnlineOrders.Services.OrderDecisionEngine>();
        services.AddHostedService<PhoneStoreAPI.Features.OnlineOrders.Background.AutoApproveBackgroundService>();

        // FluentValidation tự động đăng ký tất cả validator trong assembly
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }

    /// <summary>
    /// Cấu hình CORS
    /// </summary>
    public static IServiceCollection AddAppCors(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
            ?? new[] { "http://localhost", "http://localhost:80", "http://localhost:5500" };

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.SetIsOriginAllowed(_ => true)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .WithExposedHeaders("X-Total-Count");
            });

            options.AddPolicy("ProductionCors", policy =>
            {
                policy.WithOrigins(origins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .WithExposedHeaders("X-Total-Count");
            });
        });

        return services;
    }

    /// <summary>
    /// P1-09: Rate Limiter (login 5/phút/IP, public-lookup 30/phút/IP)
    /// </summary>
    public static IServiceCollection AddAppRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.ContentType = "application/json";
                var payload = new
                {
                    message = "Bạn thao tác quá nhiều, vui lòng thử lại sau ít phút.",
                    errors = new[] { "Giới hạn tần suất yêu cầu (Rate limit exceeded)." }
                };
                await context.HttpContext.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(payload), token);
            };

            // Login policy: 5 lần/phút/IP (P1-09)
            options.AddPolicy("login", httpContext =>
            {
                var forwarded = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                var clientIp = !string.IsNullOrWhiteSpace(forwarded) ? forwarded.Split(",")[0].Trim() : (httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
                return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                });
            });

            // OTP policy: 10 req/phút/IP (kết hợp 3 req/giờ/SĐT trong OtpService)
            options.AddPolicy("otp", httpContext =>
            {
                var forwarded = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                var clientIp = !string.IsNullOrWhiteSpace(forwarded) ? forwarded.Split(",")[0].Trim() : (httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
                return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                });
            });

            // Checkout & Cart policy (Task 9): 60 req/phút/IP
            options.AddPolicy("checkout", httpContext =>
            {
                var forwarded = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                var clientIp = !string.IsNullOrWhiteSpace(forwarded) ? forwarded.Split(",")[0].Trim() : (httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
                return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 60,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                });
            });

            // Public lookup policy: 5 lần/10 phút/IP (Giai đoạn 3)
            options.AddPolicy("public-lookup", httpContext =>
            {
                var forwarded = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                var clientIp = !string.IsNullOrWhiteSpace(forwarded) ? forwarded.Split(",")[0].Trim() : (httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
                return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(10),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                });
            });
        });

        return services;
    }

    /// <summary>
    /// Cấu hình Swagger / OpenAPI kèm nút Authorize Bearer Token
    /// </summary>
    public static IServiceCollection AddAppSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PhoneStore Management API",
                Version = "v1",
                Description = "Hệ thống Quản lý Cửa hàng Điện thoại CellphoneS (.NET 10 & PostgreSQL)"
            });

            // Cấu hình Bearer Token Authorize button
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Nhập JWT Bearer token theo định dạng: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer"),
                    new List<string>()
                }
            });
        });

        return services;
    }
}