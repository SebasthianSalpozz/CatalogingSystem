namespace CatalogingSystem.Core.Entities;

public class Material
{
    public string? DescribedPart { get; set; }
    public required string MaterialName { get; set; }
    public string? Colors { get; set; }
}