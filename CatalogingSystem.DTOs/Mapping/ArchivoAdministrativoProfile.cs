namespace CatalogingSystem.DTOs.Mapping;
using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;
using AutoMapper;
public class ArchivoAdministrativoProfile : Profile
{
    public ArchivoAdministrativoProfile()
    {
        CreateMap<ArchivoAdministrativoDto, ArchivoAdministrativo>();
        CreateMap<ArchivoAdministrativo, ArchivoAdministrativoDto>();
    }
}
