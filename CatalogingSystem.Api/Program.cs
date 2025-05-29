using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CatalogingSystem.Api.Middleware;
using CatalogingSystem.Core.Entities;
using CatalogingSystem.Data.DbContext;
using CatalogingSystem.Services.Implementations;
using CatalogingSystem.Services.Interfaces;
using CatalogingSystem.DTOs.Mapping;
using DotNetEnv;
using System.Text;
using CatalogingSystem.Api;
using CatalogingSystem.Core.Interfaces;
using Cataloging.Api.Swagger;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Load .env file
try
{
    Env.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading .env file: {ex.Message}");
}

// Validate JWT configuration
var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key");
var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer");
var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience");

// Debug output to verify values
Console.WriteLine($"Jwt__Key: {jwtKey}");
Console.WriteLine($"Jwt__Issuer: {jwtIssuer}");
Console.WriteLine($"Jwt__Audience: {jwtAudience}");

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    throw new InvalidOperationException("JWT configuration is missing. Ensure Jwt__Key, Jwt__Issuer, and Jwt__Audience are set in .env.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add ASP.NET Core Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddRoleManager<RoleManager<IdentityRole>>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CatalogingSystem API", Version = "v1" });
    c.OperationFilter<AddTenantHeaderParameter>();
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field (e.g., Bearer {token})",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAutoMapper(typeof(ArchivoAdministrativoProfile), typeof(IdentificationProfile), typeof(GraphicDocumentationProfile), typeof(CatalogItemProfile));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BaseDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IArchivoAdministrativoService, ArchivoAdministrativoService>();
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IIdentificationService, IdentificationService>();
builder.Services.AddScoped<IGraphicDocumentationService, GraphicDocumentationService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Apply migrations for all existing tenants
await ApplyTenantMigrations.ApplyAllTenantMigrationsAsync(app.Services);

app.UseCors(MyAllowSpecificOrigins);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<TenantResolver>();
app.UseAuthorization();
app.MapControllers();

app.Run();