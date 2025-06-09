namespace CatalogingSystem.DTOs;

public class UpdateUserRequestDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
    public string? PermissionLevel { get; set; }
}