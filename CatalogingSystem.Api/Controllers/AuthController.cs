using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogingSystem.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var token = await _authService.LoginAsync(loginDto);
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Credenciales inválidas" });
        }
    }
}