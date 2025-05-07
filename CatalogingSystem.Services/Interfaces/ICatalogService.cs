namespace CatalogingSystem.Services.Interfaces;

using CatalogingSystem.DTOs.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICatalogService
{
    Task<IEnumerable<CatalogItemDto>> GetCatalogItems();
    Task<CatalogItemDto?> GetCatalogItem(long expediente);
    Task<IEnumerable<CatalogItemDto>> SearchCatalogItems(
        string? materialName,
        string? authorName,
        string? titleName,
        string? genericClassification);
}