using Microsoft.EntityFrameworkCore;
using CatalogingSystem.Data.DbContext;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogingSystem.Api;

public class ApplyTenantMigrations
{
    public static async Task ApplyAllTenantMigrationsAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var baseDbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

        var tenants = await baseDbContext.Tenants.ToListAsync();
        var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        foreach (var tenant in tenants)
        {
            Console.WriteLine($"Applying migrations for tenant: {tenant.Id}");

            appDbContext.Database.SetConnectionString(tenant.ConnectionString);

            try
            {
                await appDbContext.Database.MigrateAsync();
                Console.WriteLine($"Migrations applied successfully for tenant: {tenant.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying migrations for tenant {tenant.Id}: {ex.Message}");
            }
        }
    }
}