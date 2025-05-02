namespace CatalogingSystem.Core.Entities;

public class Author
{
    public required string Name { get; set; }
    public required string BirthPlace { get; set; }
    public required DateTime BirthDate { get; set; }
    public string? DeathPlace { get; set; }
    public DateTime? DeathDate { get; set; }
}