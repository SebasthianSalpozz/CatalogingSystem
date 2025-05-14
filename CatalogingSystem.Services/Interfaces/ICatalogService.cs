namespace CatalogingSystem.Services.Interfaces;

using CatalogingSystem.DTOs.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICatalogService
{
    Task<PagedResultDto<CatalogItemDto>> GetCatalogItems(int page = 1, int size = 10);
    Task<CatalogItemDto?> GetCatalogItem(long expediente);
    Task<PagedResultDto<CatalogItemDto>> SearchCatalogItems(
        string? materialName,
        string? authorName,
        string? titleName,
        string? genericClassification,
        int page = 1,
        int size = 10);
}