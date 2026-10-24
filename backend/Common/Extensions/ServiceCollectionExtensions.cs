using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
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
    public static IServiceCollection AddAppAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "PhoneStoreManagementSystem_SuperSecretKey_2026_AspNetCore10");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
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
    /// Bước 4. Đăng ký toàn bộ Services theo từng Feature
    /// </summary>
    public static IServiceCollection AddFeatureServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStaffService, StaffService>();
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

        // Bước 7. Đăng ký FluentValidation
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }

    /// <summary>
    /// Cấu hình CORS
    /// </summary>
    public static IServiceCollection AddAppCors(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost", "http://localhost:80" };

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });

            options.AddPolicy("ProductionCors", policy =>
            {
                policy.WithOrigins(origins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        return services;
    }

    /// <summary>
    /// Bước 5. Cấu hình Swagger / OpenAPI kèm nút Authorize Bearer Token
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
