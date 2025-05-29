using CatalogingSystem.DTOs;

namespace CatalogingSystem.Services.Interfaces;

public interface IAuthService
{
    Task<string?> AuthenticateAsync(LoginRequestDto request);
}