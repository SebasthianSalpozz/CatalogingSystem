using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CatalogingSystem.Data.DbContext;
using CatalogingSystem.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CatalogingSystem.Api.Controllers;

[ApiController]
[Route("SuperDirector")]
[Authorize(Roles = "SuperDirector")]
public class SuperDirectorController : ControllerBase
{
    private readonly BaseDbContext _baseDbContext;

    public SuperDirectorController(BaseDbContext baseDbContext)
    {
        _baseDbContext = baseDbContext;
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateSuperDirectorRequestDto request)
    {
        var superDirector = await _baseDbContext.SuperDirectorUsers
            .FirstOrDefaultAsync(u => u.UserName == "superadmin");
        if (superDirector == null)
        {
            return NotFound(new { message = "Super Director no encontrado." });
        }

        if (!string.IsNullOrEmpty(request.NewUsername))
        {
            superDirector.UserName = request.NewUsername;
        }

        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            superDirector.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        }

        await _baseDbContext.SaveChangesAsync();
        return NoContent();
    }
}