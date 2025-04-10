namespace CatalogingSystem.Data.DbContext;

using CatalogingSystem.Core.Entities;
using CatalogingSystem.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentTenantService _tenantService;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentTenantService tenantService) 
        : base(options)
    {
        _tenantService = tenantService;
    }

    public DbSet<ArchivoAdministrativo> ArchivosAdministrativos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrEmpty(_tenantService?.ConnectionString))
        {
            optionsBuilder.UseNpgsql(_tenantService.ConnectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ArchivoAdministrativo>()
        .Property(a => a.Id).HasDefaultValueSql("gen_random_uuid()");
        
        modelBuilder.Entity<ArchivoAdministrativo>()
            .Property(a => a.institucion)
            .HasConversion<string>();

        modelBuilder.Entity<ArchivoAdministrativo>()
            .Property(a => a.documentoOrigen)
            .HasConversion<string>();
    }
    
}
