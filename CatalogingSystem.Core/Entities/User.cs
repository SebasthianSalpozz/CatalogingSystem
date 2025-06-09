using CatalogingSystem.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace CatalogingSystem.Core.Entities;

public class User : IdentityUser
{
    public string? TenantId { get; set; }
    public InvestigatorPermissionLevel? PermissionLevel { get; set; } 
}