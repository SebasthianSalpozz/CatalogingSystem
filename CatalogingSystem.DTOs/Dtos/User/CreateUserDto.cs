using CatalogingSystem.Core.Enums;
namespace CatalogingSystem.DTOs.Dtos;

public class CreateUserDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required AccessLevel AccessLevel { get; set; }
}