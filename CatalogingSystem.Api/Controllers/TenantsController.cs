namespace CatalogingSystem.Api.Controllers;

using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;

    public TenantsController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTenants()
    {
        try
        {
            var tenants = await _tenantService.GetAllTenantsAsync();
            return Ok(tenants);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al listar los tenants: {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request)
    {
        try
        {
            var tenant = await _tenantService.CreateTenantAsync(request);
            return CreatedAtAction(nameof(GetAllTenants), new { id = tenant.Id }, tenant);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al crear el tenant: {ex.Message}" });
        }
    }
}