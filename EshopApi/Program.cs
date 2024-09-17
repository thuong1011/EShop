using DotNetEnv;
using Asp.Versioning;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using EshopApi.Domain.Interfaces;
using EshopApi.Infrastructure.Data;
using EshopApi.Infrastructure.Repositories;
using EshopApi.Application.Interfaces;
using EshopApi.Application.Services;
using EshopApi.Presentation.Middlewares;



Env.Load();
var builder = WebApplication.CreateBuilder(args);



// builder.Services.AddDbContext<EshopDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<EshopDbContext>(options =>
    options.UseSqlServer(Env.GetString("DB_CONNECTION_STR")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/v1/login";
        options.LogoutPath = "/api/v1/logout";
        options.AccessDeniedPath = "/api/v1/access-denied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = false;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

// builder.Services.AddAuthorizationBuilder()
//     .AddPolicy("RequireAdminPermission", policy => policy.RequireClaim(ClaimTypes.Role, "admin"))
//     .AddPolicy("RequireManagerPermission", policy => policy.RequireClaim(ClaimTypes.Role, "manager", "admin"))
//     .AddPolicy("RequireUserPermission", policy => policy.RequireClaim(ClaimTypes.Role, "user", "manager", "admin"));
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminPermission", policy => policy.RequireRole("admin"))
    .AddPolicy("RequireManagerPermission", policy => policy.RequireRole("manager", "admin"))
    .AddPolicy("RequireUserPermission", policy => policy.RequireRole("user", "manager", "admin"));



builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version")
        );
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Eshop API - v1", Version = "v1.0" });
        c.SwaggerDoc("v2", new OpenApiInfo { Title = "Eshop API - v2", Version = "v2.0" });
    });



var AllowedOrigins = "AllowedcOrigins";
builder.Services.AddCors(options =>
    {
        options.AddPolicy(AllowedOrigins, builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials()
                       .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
            });
    });



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("This is development environment!");
    app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        // Build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors(AllowedOrigins);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
