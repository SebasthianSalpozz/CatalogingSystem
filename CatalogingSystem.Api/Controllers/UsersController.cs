using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createDto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(createDto);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        return Ok(await _userService.GetUsersAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var user = await _userService.GetUserAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto updateDto)
    {
        var success = await _userService.UpdateUserAsync(id, updateDto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var success = await _userService.DeleteUserAsync(id);
        return success ? NoContent() : NotFound();
    }
}