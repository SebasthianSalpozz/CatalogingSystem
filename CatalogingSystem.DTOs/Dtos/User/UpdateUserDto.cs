using CatalogingSystem.Core.Enums;
namespace CatalogingSystem.DTOs.Dtos;

public class UpdateUserDto
{
    public AccessLevel? AccessLevel { get; set; }
    public bool? IsActive { get; set; }
}