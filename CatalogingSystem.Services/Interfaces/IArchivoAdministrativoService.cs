namespace CatalogingSystem.Services.Interfaces;
using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;
public interface IArchivoAdministrativoService
{
    Task<IEnumerable<ArchivoAdministrativoDto>> GetArchivosAdministrativos();
    Task<ArchivoAdministrativoDto?> GetArchivoAdministrativo(long expediente);
    Task<ArchivoAdministrativo> CreateArchivoAdministrativo(ArchivoAdministrativoDto dto);
    Task<bool> UpdateArchivoAdministrativo(long expediente, ArchivoAdministrativoDto dto);
    Task<bool> DeleteArchivoAdministrativo(long expediente);
}
