namespace CatalogingSystem.DTOs.Dtos;

using CatalogingSystem.Core.Entities;

public class CatalogItemDto
{
    public long Expediente { get; set; }
    public ArchivoAdministrativoDto ArchivoAdministrativo { get; set; }
    public IdentificationDto? Identification { get; set; }
    public GraphicDocumentationDto? GraphicDocumentation { get; set; }
}