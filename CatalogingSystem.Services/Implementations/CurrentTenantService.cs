namespace CatalogingSystem.Services.Implementations;

using CatalogingSystem.Core.Interfaces;
using CatalogingSystem.Data.DbContext;
using Microsoft.EntityFrameworkCore;

public class CurrentTenantService : ICurrentTenantService
{
    private readonly BaseDbContext _baseDbContext;

    public CurrentTenantService(BaseDbContext baseDbContext)
    {
        _baseDbContext = baseDbContext;
    }

    public string? TenantId { get; set; }
    public string? ConnectionString { get; set; }

    public async Task<bool> SetTenantAsync(string tenantId)
    {
        var tenant = await _baseDbContext.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);
        if (tenant == null)
            throw new Exception("Tenant no válido");

        TenantId = tenant.Id;
        ConnectionString = tenant.ConnectionString;
        return true;
    }
}