namespace CatalogingSystem.Core.Entities;

public class Section
{
    public required string Room { get; set; }
    public string? Panel { get; set; }
    public string? DisplayCase { get; set; }
    public string? Easel { get; set; }
    public string? Storage { get; set; }
    public string? Courtyard { get; set; }
    public string? Pillar { get; set; }
    public string? Others { get; set; }
}