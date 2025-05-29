namespace CatalogingSystem.DTOs;

public class UserDto
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string TenantId { get; set; }
    public required string Role { get; set; }
    public string? PermissionLevel { get; set; } 
}