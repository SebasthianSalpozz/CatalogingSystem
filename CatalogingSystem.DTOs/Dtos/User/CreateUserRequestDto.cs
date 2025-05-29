namespace CatalogingSystem.DTOs;

public class CreateUserRequestDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string TenantId { get; set; }
    public required string Role { get; set; }
    public string? PermissionLevel { get; set; }
}