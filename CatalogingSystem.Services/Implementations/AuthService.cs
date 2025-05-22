using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CatalogingSystem.Core.Entities;
using CatalogingSystem.Core.Enums;
using CatalogingSystem.Core.Interfaces;
using CatalogingSystem.Data.DbContext;
using CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CatalogingSystem.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTenantService _tenantService;

    public AuthService(ApplicationDbContext context, IConfiguration configuration, ICurrentTenantService tenantService)
    {
        _context = context;
        _configuration = configuration;
        _tenantService = tenantService;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.IsActive);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        var token = GenerateJwtToken(user);
        return token;
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("tenantId", _tenantService.TenantId)
        };

        if (user.Role == Role.Investigador && user.AccessLevel.HasValue)
        {
            claims.Add(new Claim("accessLevel", user.AccessLevel.Value.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}