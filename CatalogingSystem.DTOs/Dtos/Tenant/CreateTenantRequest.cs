namespace CatalogingSystem.DTOs.Dtos;

public class CreateTenantRequest
{
    public required string Name { get; set; } 
    public required string ISIL { get; set; } 
    public required string Description { get; set; }
}