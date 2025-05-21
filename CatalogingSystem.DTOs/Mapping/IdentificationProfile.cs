namespace CatalogingSystem.DTOs.Mapping;

using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;
using AutoMapper;

public class IdentificationProfile : Profile
{
    public IdentificationProfile()
    {
        CreateMap<IdentificationDto, Identification>();
        CreateMap<Identification, IdentificationDto>();
        CreateMap<UpdateIdentificationDto, Identification>();
    }
}