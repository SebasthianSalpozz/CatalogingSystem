namespace CatalogingSystem.Core.Entities;

public class Tenant
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string ISIL { get; set; }
    public string? Description { get; set; }
    public string? ConnectionString { get; set; }
}