using CatalogingSystem.Core.Entities;
using CatalogingSystem.Core.Interfaces;
using CatalogingSystem.Data.DbContext;
using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace CatalogingSystem.Services.Implementations;
 
public class TenantService : ITenantService
{
    private readonly BaseDbContext _baseDbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public TenantService(BaseDbContext baseDbContext, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _baseDbContext = baseDbContext;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public async Task<Tenant> CreateTenantAsync(CreateTenantRequest request)
    {
        string tenantId = $"tenant-{request.ISIL}";

        if (await _baseDbContext.Tenants.AnyAsync(t => t.Id == tenantId))
            throw new InvalidOperationException($"El tenant con ISIL {request.ISIL} ya existe.");

        string defaultConnection = _configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("La cadena de conexión por defecto no está configurada.");
        var builder = new NpgsqlConnectionStringBuilder(defaultConnection);
        string tenantDbName = $"CatalogingSystem-db-{tenantId}";
        builder.Database = tenantDbName;
        string tenantConnectionString = builder.ToString();

        var tenant = new Tenant
        {
            Id = tenantId,
            Name = request.Name,
            ISIL = request.ISIL,
            Description = request.Description,
            ConnectionString = tenantConnectionString
        };

        try
        {
            // Crear la base de datos manualmente si no existe
            var masterConnection = new NpgsqlConnectionStringBuilder(defaultConnection)
            {
                Database = "postgres" // Conectar al catálogo principal de PostgreSQL
            }.ToString();

            using (var connection = new NpgsqlConnection(masterConnection))
            {
                await connection.OpenAsync();
                using var command = new NpgsqlCommand($"CREATE DATABASE \"{tenantDbName}\"", connection);
                await command.ExecuteNonQueryAsync();
            }

            // Aplicar migraciones al nuevo tenant
            using var scope = _serviceProvider.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            appDbContext.Database.SetConnectionString(tenantConnectionString);
            await appDbContext.Database.MigrateAsync();

            // Guardar el tenant en la base central
            _baseDbContext.Tenants.Add(tenant);
            await _baseDbContext.SaveChangesAsync();

            await CreateDefaultDirectorUserAsync(tenantId, tenantConnectionString);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al crear el tenant: {ex.Message}");
        }

        return tenant;
    }

    public async Task<List<Tenant>> GetAllTenantsAsync()
    {
        return await _baseDbContext.Tenants.ToListAsync();
    }
    private async Task CreateDefaultDirectorUserAsync(string tenantId, string tenantConnectionString)
    {
        using var scope = _serviceProvider.CreateScope();
        var tenantService = scope.ServiceProvider.GetRequiredService<ICurrentTenantService>();
        await tenantService.SetTenantAsync(tenantId);

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var defaultDirector = new User
        {
            UserName = "director",
            TenantId = tenantId,
            PermissionLevel = null
        };

        var result = await userManager.CreateAsync(defaultDirector, "director123");
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Error al crear el usuario director por defecto: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roleManager.RoleExistsAsync("Director"))
        {
            await roleManager.CreateAsync(new IdentityRole("Director"));
        }

        await userManager.AddToRoleAsync(defaultDirector, "Director");
    }
}