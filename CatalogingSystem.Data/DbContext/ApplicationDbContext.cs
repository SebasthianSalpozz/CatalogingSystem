namespace CatalogingSystem.Data.DbContext;
using CatalogingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<ArchivoAdministrativo> ArchivosAdministrativos { get; set; }
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
