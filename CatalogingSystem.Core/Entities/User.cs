
using Microsoft.AspNetCore.Identity;

namespace CatalogingSystem.Core.Entities;

public class User : IdentityUser
{
    public required string TenantId { get; set; }
}