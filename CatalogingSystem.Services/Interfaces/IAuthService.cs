using System.Threading.Tasks;
using CatalogingSystem.DTOs.Dtos;

namespace CatalogingSystem.Services.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto loginDto);
}