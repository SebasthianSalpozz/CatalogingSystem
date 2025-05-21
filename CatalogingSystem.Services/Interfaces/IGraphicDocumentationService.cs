namespace CatalogingSystem.Services.Interfaces;

using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;

public interface IGraphicDocumentationService
{
    Task<IEnumerable<GraphicDocumentationDto>> GetGraphicDocumentations();
    Task<GraphicDocumentationDto?> GetGraphicDocumentation(long expediente);
    Task<GraphicDocumentation> CreateGraphicDocumentation(GraphicDocumentationDto dto);
    Task<bool> UpdateGraphicDocumentation(long expediente, UpdateGraphicDocumentationDto dto);
    Task<bool> DeleteGraphicDocumentation(long expediente);
}