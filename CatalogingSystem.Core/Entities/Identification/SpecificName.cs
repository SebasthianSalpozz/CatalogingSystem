namespace CatalogingSystem.Core.Entities;

public class SpecificName
{
    public required string GenericName { get; set; }
    public string? RelatedTerms { get; set; }
    public string? SpecificTerms { get; set; }
    public string? UsedBy { get; set; }
    public string? Notes { get; set; }
}