namespace CatalogingSystem.Services.Interfaces;

using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;

public interface IIdentificationService
{
    Task<IEnumerable<IdentificationDto>> GetIdentifications();
    Task<IdentificationDto?> GetIdentification(long expediente);
    Task<Identification> CreateIdentification(IdentificationDto dto);
    Task<bool> UpdateIdentification(long expediente, IdentificationDto dto);
    Task<bool> DeleteIdentification(long expediente);
}