using CatalogingSystem.Core.Entities;
using CatalogingSystem.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CatalogingSystem.Api;

public static class ProgramHelper
{
    public static async Task EnsureSuperDirectorExists(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var baseDbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

        var superDirector = await baseDbContext.SuperDirectorUsers
            .FirstOrDefaultAsync(u => u.UserName == "superadmin");
        if (superDirector == null)
        {
            superDirector = new SuperDirectorUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "superadmin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("SuperAdmin123!")
            };
            baseDbContext.SuperDirectorUsers.Add(superDirector);
            await baseDbContext.SaveChangesAsync();
        }
    }
}