using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CatalogingSystem.Services.Interfaces;
using CatalogingSystem.DTOs;

namespace CatalogingSystem.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Director")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Creates a new user with the Investigador role.
    /// </summary>
    /// <param name="request">The user creation request.</param>
    /// <returns>The created user details if successful; otherwise, an error.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
    {
        try
        {
            var user = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(CreateUser), new { id = user.Id }, new { user.Id, user.UserName, user.TenantId, Role = request.Role });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error creating user: {ex.Message}" });
        }
    }

    /// <summary>
    /// Gets all users for the current tenant.
    /// </summary>
    /// <returns>A list of users.</returns>
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error retrieving users: {ex.Message}" });
        }
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserRequestDto request)
    {
        try
        {
            var success = await _userService.UpdateUserAsync(userId, request);
            return success ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error al actualizar el usuario: {ex.Message}" });
        }
    }
}