namespace CatalogingSystem.DTOs.Mapping;

using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;
using AutoMapper;

public class GraphicDocumentationProfile : Profile
{
    public GraphicDocumentationProfile()
    {
        CreateMap<GraphicDocumentationDto, GraphicDocumentation>()
            .ForMember(dest => dest.inventory, opt => opt.Ignore());
        CreateMap<GraphicDocumentation, GraphicDocumentationDto>();
        CreateMap<UpdateGraphicDocumentationDto, GraphicDocumentation>()
            .ForMember(dest => dest.inventory, opt => opt.Ignore());
    }
}