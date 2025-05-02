namespace CatalogingSystem.Core.Entities;

public class Identification
{
    public Guid Id { get; set; }
    public required Section section { get; set; }
    public required long inventory { get; set; }
    public required int numberOfObjects { get; set; }
    public required string genericClassification { get; set; }
    public required string objectName { get; set; }
    public required Typology typology { get; set; }
    public required SpecificName specificName { get; set; }
    public required Author author { get; set; }
    public required Title title { get; set; }
    public required Material material { get; set; }
    public required Technique techniques { get; set; }
    public required string observations { get; set; }
    public required long expediente { get; set; }
    public ArchivoAdministrativo ArchivoAdministrativo { get; set; }
}