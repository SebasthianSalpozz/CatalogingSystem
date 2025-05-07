namespace CatalogingSystem.Core.Entities;

public class ImageAuthor
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string IdentityCard { get; set; }
    public string? InstitutionalId { get; set; }
    public string? Institution { get; set; }
    public required string Address { get; set; }
    public required string Locality { get; set; }
    public required string Province { get; set; }
    public required string Department { get; set; }
    public required string Country { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public string? References { get; set; }
}