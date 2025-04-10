namespace CatalogingSystem.Services.Interfaces;

using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;

public interface ITenantService
{
    Task<Tenant> CreateTenantAsync(CreateTenantRequest request);
}