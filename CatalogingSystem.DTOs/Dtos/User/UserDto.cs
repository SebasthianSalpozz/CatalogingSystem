using CatalogingSystem.Core.Enums;

namespace CatalogingSystem.DTOs.Dtos;
public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public Role Role { get; set; }
    public AccessLevel? AccessLevel { get; set; }
    public bool IsActive { get; set; }
    public string TenantId { get; set; }
}