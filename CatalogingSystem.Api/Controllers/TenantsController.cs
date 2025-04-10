// CatalogingSystem.Api/Controllers/TenantsController.cs
using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogingSystem.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;

    public TenantsController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request)
    {
        var tenant = await _tenantService.CreateTenantAsync(request);
        return Ok(new { Message = $"Tenant {tenant.Name} creado correctamente" });
    }
}