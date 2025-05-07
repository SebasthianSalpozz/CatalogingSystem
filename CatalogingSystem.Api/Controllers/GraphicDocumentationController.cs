namespace CatalogingSystem.Api.Controllers;

using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class GraphicDocumentationController : ControllerBase
{
    private readonly IGraphicDocumentationService _service;

    public GraphicDocumentationController(IGraphicDocumentationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GraphicDocumentationDto>>> GetGraphicDocumentations()
    {
        return Ok(await _service.GetGraphicDocumentations());
    }

    [HttpGet("{expediente:long}/{inventory:long}")]
    public async Task<ActionResult<GraphicDocumentationDto>> GetGraphicDocumentation(long expediente, long inventory)
    {
        var graphicDoc = await _service.GetGraphicDocumentation(expediente, inventory);
        return graphicDoc == null ? NotFound() : Ok(graphicDoc);
    }

    [HttpPost]
    public async Task<ActionResult<GraphicDocumentation>> PostGraphicDocumentation([FromBody] GraphicDocumentationDto dto)
    {
        try
        {
            var graphicDoc = await _service.CreateGraphicDocumentation(dto);
            return CreatedAtAction(nameof(GetGraphicDocumentation), new { expediente = graphicDoc.expediente, inventory = graphicDoc.inventory }, graphicDoc);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{expediente:long}/{inventory:long}")]
    public async Task<IActionResult> PutGraphicDocumentation(long expediente, long inventory, [FromBody] GraphicDocumentationDto dto)
    {
        try
        {
            var success = await _service.UpdateGraphicDocumentation(expediente, inventory, dto);
            return success ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{expediente:long}/{inventory:long}")]
    public async Task<IActionResult> DeleteGraphicDocumentation(long expediente, long inventory)
    {
        var success = await _service.DeleteGraphicDocumentation(expediente, inventory);
        return success ? NoContent() : NotFound();
    }
}