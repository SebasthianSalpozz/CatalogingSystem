using Microsoft.EntityFrameworkCore;
using CatalogingSystem.Data.DbContext;
using CatalogingSystem.Services.Interfaces;
using CatalogingSystem.Services.Implementations;
using CatalogingSystem.DTOs.Mapping;
using CatalogingSystem.Core.Interfaces;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

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

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CatalogingSystem API", Version = "v1" });
    c.OperationFilter<AddTenantHeaderParameter>(); // Añade el filtro
});
builder.Services.AddAutoMapper(typeof(ArchivoAdministrativoProfile));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BaseDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

    // options.UseNpgsql(connectionString));
builder.Services.AddScoped<IArchivoAdministrativoService, ArchivoAdministrativoService>();
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
builder.Services.AddScoped<ITenantService, TenantService>();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<TenantResolver>();
app.UseAuthorization();
app.MapControllers();

app.Run();
