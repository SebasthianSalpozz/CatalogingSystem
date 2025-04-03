namespace CatalogingSystem.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ArchivoAdministrativoController : ControllerBase
{
    private readonly IArchivoAdministrativoService _service;

    public ArchivoAdministrativoController(IArchivoAdministrativoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArchivoAdministrativoDto>>> GetArchivosAdministrativos()
    {
        return Ok(await _service.GetArchivosAdministrativos());
    }

    [HttpGet("{expediente:long}")]
    public async Task<ActionResult<ArchivoAdministrativoDto>> GetArchivoAdministrativo(long expediente)
    {
        var archivo = await _service.GetArchivoAdministrativo(expediente);
        return archivo == null ? NotFound() : Ok(archivo);
    }

    [HttpPost]
    public async Task<ActionResult<ArchivoAdministrativo>> PostArchivoAdministrativo(ArchivoAdministrativoDto dto)
    {
        try
        {
            var archivo = await _service.CreateArchivoAdministrativo(dto);
            return CreatedAtAction(nameof(GetArchivoAdministrativo), new { expediente = archivo.expediente }, archivo);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{expediente:long}")]
    public async Task<IActionResult> PutArchivoAdministrativo(long expediente, ArchivoAdministrativoDto dto)
    {
        var success = await _service.UpdateArchivoAdministrativo(expediente, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{expediente:long}")]
    public async Task<IActionResult> DeleteArchivoAdministrativo(long expediente)
    {
        var success = await _service.DeleteArchivoAdministrativo(expediente);
        return success ? NoContent() : NotFound();
    }
}
