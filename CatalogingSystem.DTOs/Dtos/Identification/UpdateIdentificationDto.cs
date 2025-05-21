namespace CatalogingSystem.DTOs.Dtos;

using CatalogingSystem.Core.Entities;

public class UpdateIdentificationDto
{
    public required Section Section { get; set; }
    public required long Inventory { get; set; }
    public required int NumberOfObjects { get; set; }
    public required string GenericClassification { get; set; }
    public required string ObjectName { get; set; }
    public required Typology Typology { get; set; }
    public required SpecificName SpecificName { get; set; }
    public required Author Author { get; set; }
    public required Title Title { get; set; }
    public required Material Material { get; set; }
    public required Technique Techniques { get; set; }
    public required string Observations { get; set; }
}