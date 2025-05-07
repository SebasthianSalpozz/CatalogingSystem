namespace CatalogingSystem.Api.Controllers;

using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _service;

    public CatalogController(ICatalogService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves all catalog items.
    /// </summary>
    /// <returns>A list of catalog items.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CatalogItemDto>>> GetCatalogItems()
    {
        return Ok(await _service.GetCatalogItems());
    }

    /// <summary>
    /// Retrieves a catalog item by expediente.
    /// </summary>
    /// <param name="expediente">The expediente number.</param>
    /// <returns>The catalog item if found; otherwise, NotFound.</returns>
    [HttpGet("{expediente:long}")]
    public async Task<ActionResult<CatalogItemDto>> GetCatalogItem(long expediente)
    {
        var item = await _service.GetCatalogItem(expediente);
        return item == null ? NotFound() : Ok(item);
    }

    /// <summary>
    /// Searches catalog items by specified criteria.
    /// </summary>
    /// <param name="materialName">The material name to filter by.</param>
    /// <param name="authorName">The author name to filter by.</param>
    /// <param name="titleName">The title name to filter by.</param>
    /// <param name="genericClassification">The generic classification to filter by.</param>
    /// <returns>A list of matching catalog items.</returns>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CatalogItemDto>>> SearchCatalogItems(
        [FromQuery] string? materialName,
        [FromQuery] string? authorName,
        [FromQuery] string? titleName,
        [FromQuery] string? genericClassification)
    {
        var items = await _service.SearchCatalogItems(materialName, authorName, titleName, genericClassification);
        return Ok(items);
    }
}