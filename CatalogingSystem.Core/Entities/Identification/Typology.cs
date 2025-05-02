namespace CatalogingSystem.Core.Entities;

public class Typology
{
    public required string Type { get; set; }
    public string? Subtype { get; set; }
    public string? Class { get; set; }
    public string? Subclass { get; set; }
    public string? Order { get; set; }
    public string? Suborder { get; set; }
}