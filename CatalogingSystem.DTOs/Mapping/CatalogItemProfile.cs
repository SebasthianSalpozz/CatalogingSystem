namespace CatalogingSystem.DTOs.Mapping;

using AutoMapper;
using CatalogingSystem.DTOs.Dtos;

public class CatalogItemProfile : Profile
{
    public CatalogItemProfile()
    {
        CreateMap<CatalogItemDto, CatalogItemDto>();
    }
}