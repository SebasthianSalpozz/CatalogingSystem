namespace CatalogingSystem.Core.Entities;

public class Title
{
    public required string Name { get; set; }
    public string? Attribution { get; set; }
    public string? Translation { get; set; }
}