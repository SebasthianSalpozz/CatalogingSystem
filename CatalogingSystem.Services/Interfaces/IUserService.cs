using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs;

namespace CatalogingSystem.Services.Interfaces;

public interface IUserService
{
    Task<User> CreateUserAsync(CreateUserRequestDto request);
    Task<List<UserDto>> GetUsersAsync();
    Task<bool> UpdateUserAsync(string userId, UpdateUserRequestDto request);
}