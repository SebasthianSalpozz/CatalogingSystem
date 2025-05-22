namespace CatalogingSystem.Api.Controllers;

using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Authorize]
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

    [HttpGet("{expediente:long}")]
    public async Task<ActionResult<GraphicDocumentationDto>> GetGraphicDocumentation(long expediente)
    {
        var graphicDoc = await _service.GetGraphicDocumentation(expediente);
        return graphicDoc == null ? NotFound() : Ok(graphicDoc);
    }

    [HttpPost]
    [Authorize(Policy = "FullAccessPolicy")]
    public async Task<ActionResult<GraphicDocumentation>> PostGraphicDocumentation([FromBody] GraphicDocumentationDto dto)
    {
        try
        {
            var graphicDoc = await _service.CreateGraphicDocumentation(dto);
            return CreatedAtAction(nameof(GetGraphicDocumentation), new { expediente = graphicDoc.expediente }, graphicDoc);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{expediente:long}")]
    [Authorize(Policy = "FullAccessPolicy")]
    public async Task<IActionResult> PutGraphicDocumentation(long expediente, [FromBody] UpdateGraphicDocumentationDto dto)
    {
        try
        {
            var success = await _service.UpdateGraphicDocumentation(expediente, dto);
            return success ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{expediente:long}")]
    [Authorize(Policy = "FullAccessPolicy")]
    public async Task<IActionResult> DeleteGraphicDocumentation(long expediente)
    {
        var success = await _service.DeleteGraphicDocumentation(expediente);
        return success ? NoContent() : NotFound();
    }
}