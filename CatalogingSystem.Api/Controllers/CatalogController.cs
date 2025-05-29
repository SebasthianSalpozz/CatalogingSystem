using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;

namespace CatalogingSystem.Api.Controllers;

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
    [Authorize(Policy = "ArchivoAdminRead")]
    public async Task<ActionResult<PagedResultDto<CatalogItemDto>>> GetCatalogItems(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        return Ok(await _service.GetCatalogItems(page, size));
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
    [Authorize(Policy = "ArchivoAdminRead")]
    public async Task<ActionResult<PagedResultDto<CatalogItemDto>>> GetCatalogItems(
        [FromQuery] long? expediente = null,
        [FromQuery] string? materialName = null,
        [FromQuery] string? authorName = null,
        [FromQuery] string? titleName = null,
        [FromQuery] string? genericClassification = null,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        if (expediente.HasValue)
        {
            var item = await _service.GetCatalogItem(expediente.Value);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(new PagedResultDto<CatalogItemDto>
            {
                Items = new[] { item },
                TotalItems = 1,
                TotalPages = 1,
                CurrentPage = 1,
                PageSize = 1
            });
        }

        var items = await _service.SearchCatalogItems(materialName, authorName, titleName, genericClassification, page, size);
        return Ok(items);
    }

    /// <summary>
    /// Deletes a catalog item and all related data by expediente.
    /// </summary>
    /// <param name="expediente">The expediente number.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpDelete("{expediente:long}")]
    [Authorize(Policy = "ArchivoAdminWrite")]
    public async Task<IActionResult> DeleteCatalogItem(long expediente)
    {
        var success = await _service.DeleteCatalogItem(expediente);
        return success ? NoContent() : NotFound();
    }
}