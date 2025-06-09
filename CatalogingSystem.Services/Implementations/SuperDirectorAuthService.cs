namespace CatalogingSystem.Services.Implementations;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using CatalogingSystem.Core.Entities;
using CatalogingSystem.Data.DbContext;
using CatalogingSystem.DTOs;
using Microsoft.EntityFrameworkCore;

public class SuperDirectorAuthService
{
    private readonly BaseDbContext _baseDbContext;

    public SuperDirectorAuthService(BaseDbContext baseDbContext)
    {
        _baseDbContext = baseDbContext;
    }

    public async Task<string?> AuthenticateAsync(LoginRequestDto request)
    {
        var superDirector = await _baseDbContext.SuperDirectorUsers
            .FirstOrDefaultAsync(u => u.UserName == request.Username);
        if (superDirector == null || !BCrypt.Net.BCrypt.Verify(request.Password, superDirector.PasswordHash))
        {
            return null;
        }

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, superDirector.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "SuperDirector")
        };

        var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key");
        var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer");
        var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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