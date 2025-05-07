namespace CatalogingSystem.Services.Interfaces;

using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;

public interface IGraphicDocumentationService
{
    Task<IEnumerable<GraphicDocumentationDto>> GetGraphicDocumentations();
    Task<GraphicDocumentationDto?> GetGraphicDocumentation(long expediente, long inventory);
    Task<GraphicDocumentation> CreateGraphicDocumentation(GraphicDocumentationDto dto);
    Task<bool> UpdateGraphicDocumentation(long expediente, long inventory, GraphicDocumentationDto dto);
    Task<bool> DeleteGraphicDocumentation(long expediente, long inventory);
}