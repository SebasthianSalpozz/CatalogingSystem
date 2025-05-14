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
    /// Retrieves paginated catalog items.
    /// </summary>
    /// <param name="page">The page number (default is 1).</param>
    /// <param name="size">The number of items per page (default is 10).</param>
    /// <returns>A paginated list of catalog items with metadata.</returns>
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<CatalogItemDto>>> GetCatalogItems(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        return Ok(await _service.GetCatalogItems(page, size));
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
    /// Searches catalog items by specified criteria with pagination.
    /// </summary>
    /// <param name="materialName">The material name to filter by.</param>
    /// <param name="authorName">The author name to filter by.</param>
    /// <param name="titleName">The title name to filter by.</param>
    /// <param name="genericClassification">The generic classification to filter by.</param>
    /// <param name="page">The page number (default is 1).</param>
    /// <param name="size">The number of items per page (default is 10).</param>
    /// <returns>A paginated list of matching catalog items with metadata.</returns>
    [HttpGet("search")]
    public async Task<ActionResult<PagedResultDto<CatalogItemDto>>> SearchCatalogItems(
        [FromQuery] string? materialName,
        [FromQuery] string? authorName,
        [FromQuery] string? titleName,
        [FromQuery] string? genericClassification,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        var items = await _service.SearchCatalogItems(materialName, authorName, titleName, genericClassification, page, size);
        return Ok(items);
    }
}