using CatalogingSystem.Core.Enums;

namespace CatalogingSystem.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required Role Role { get; set; }
    public AccessLevel? AccessLevel { get; set; } // Solo para Investigadores
    public bool IsActive { get; set; } = true;
    public required string TenantId { get; set; } // Asociar al tenant
}