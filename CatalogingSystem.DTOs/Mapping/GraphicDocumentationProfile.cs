namespace CatalogingSystem.DTOs.Mapping;

using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;
using AutoMapper;

public class GraphicDocumentationProfile : Profile
{
    public GraphicDocumentationProfile()
    {
        CreateMap<GraphicDocumentationDto, GraphicDocumentation>();
        CreateMap<GraphicDocumentation, GraphicDocumentationDto>();
    }
}