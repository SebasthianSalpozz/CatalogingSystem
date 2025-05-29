
using System.Security.Claims;
using CatalogingSystem.Core.Entities;
using CatalogingSystem.Core.Interfaces;
using CatalogingSystem.DTOs;
using CatalogingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CatalogingSystem.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ICurrentTenantService _tenantService;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<User> userManager, ICurrentTenantService tenantService, IConfiguration configuration)
    {
        _userManager = userManager;
        _tenantService = tenantService;
        _configuration = configuration;
    }

    public async Task<string?> AuthenticateAsync(LoginRequestDto request)
    {
        // Validar el contexto del tenant
        if (string.IsNullOrEmpty(_tenantService.TenantId))
        {
            throw new InvalidOperationException("Tenant context is not set.");
        }

        // Buscar el usuario y verificar credenciales
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null || user.TenantId != _tenantService.TenantId || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return null;
        }

        // Obtener roles y crear claims
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim("tenantId", user.TenantId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Leer las variables de entorno directamente
        var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key");
        var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer");
        var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience");

        // Validar que las variables no sean nulas
        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
        {
            throw new InvalidOperationException("JWT configuration is missing in AuthService.");
        }

        // Crear la clave y las credenciales para el token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Generar el token JWT
        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}