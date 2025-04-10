namespace CatalogingSystem.Data.DbContext;

using CatalogingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
public class BaseDbContext : DbContext
{
    public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }

    public DbSet<Tenant> Tenants { get; set; }
}