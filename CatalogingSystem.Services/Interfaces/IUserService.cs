using CatalogingSystem.Core.Entities;
using CatalogingSystem.DTOs.Dtos;

namespace CatalogingSystem.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(CreateUserDto createDto);
    Task<IEnumerable<UserDto>> GetUsersAsync();
    Task<UserDto> GetUserAsync(Guid id);
    Task<bool> UpdateUserAsync(Guid id, UpdateUserDto updateDto);
    Task<bool> DeleteUserAsync(Guid id);
}