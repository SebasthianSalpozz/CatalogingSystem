namespace CatalogingSystem.Core.Interfaces;

public interface ICurrentTenantService
{
    string? TenantId { get; set; }
    string? ConnectionString { get; set; }
    Task<bool> SetTenantAsync(string tenantId);
}