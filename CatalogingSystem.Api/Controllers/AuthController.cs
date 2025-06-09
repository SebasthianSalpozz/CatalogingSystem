using Microsoft.AspNetCore.Mvc;
using CatalogingSystem.Services.Interfaces;
using CatalogingSystem.DTOs;
using CatalogingSystem.Services.Implementations;


namespace CatalogingSystem.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly SuperDirectorAuthService _superDirectorAuthService;

    public AuthController(IAuthService authService, SuperDirectorAuthService superDirectorAuthService)
    {
        _authService = authService;
        _superDirectorAuthService = superDirectorAuthService;
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
    
    /// <summary>
    /// Authenticates the Super Director and returns a JWT token.
    /// </summary>
    /// <param name="request">The login request with username and password.</param>
    /// <returns>A JWT token if successful; otherwise, Unauthorized.</returns>
    [HttpPost("super-login")]
    public async Task<IActionResult> SuperLogin([FromBody] LoginRequestDto request)
    {
        var token = await _superDirectorAuthService.AuthenticateAsync(request);
        if (token == null)
        {
            return Unauthorized(new { message = "Invalid credentials for Super Director" });
        }
        return Ok(new { Token = token });
    }
}