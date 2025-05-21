namespace CatalogingSystem.DTOs.Dtos;

using CatalogingSystem.Core.Entities;

public class UpdateGraphicDocumentationDto
{
    public string? GenericControlNumber { get; set; }
    public string? SpecificControlNumber { get; set; }
    public DateTime? Date { get; set; }
    public List<string>? SupportTypes { get; set; }
    public Dimensions? Dimensions { get; set; }
    public ImageAuthor? ImageAuthor { get; set; }
    public required string Description { get; set; }
    public required string TechnicalData { get; set; }
    public string? GeneralObservations { get; set; }
    public List<string>? ImageUrls { get; set; }
}