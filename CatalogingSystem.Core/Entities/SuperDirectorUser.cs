namespace CatalogingSystem.Core.Entities;

public class SuperDirectorUser
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string PasswordHash { get; set; }
}