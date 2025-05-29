using Microsoft.AspNetCore.Mvc;
using CatalogingSystem.Services.Interfaces;
using CatalogingSystem.DTOs;


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

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="request">The login request with username and password.</param>
    /// <returns>A JWT token if successful; otherwise, Unauthorized.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var token = await _authService.AuthenticateAsync(request);
        if (token == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        return Ok(new { Token = token });
    }
}