namespace CatalogingSystem.Data.DbContext;

using CatalogingSystem.Core.Entities;
using CatalogingSystem.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

public partial class ApplicationDbContext : DbContext
{
    private readonly ICurrentTenantService _tenantService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentTenantService tenantService) 
        : base(options)
    {
        _tenantService = tenantService;
    }

    public DbSet<ArchivoAdministrativo> ArchivosAdministrativos { get; set; }
    public DbSet<Identification> Identifications { get; set; }
    public DbSet<GraphicDocumentation> GraphicDocumentations { get; set; }
    public DbSet<User> Users { get; set; }

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

        // ArchivoAdministrativo configuration
        modelBuilder.Entity<ArchivoAdministrativo>()
            .Property(a => a.Id).HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<ArchivoAdministrativo>()
            .Property(a => a.institucion)
            .HasConversion<string>();

        modelBuilder.Entity<ArchivoAdministrativo>()
            .Property(a => a.documentoOrigen)
            .HasConversion<string>();

        modelBuilder.Entity<ArchivoAdministrativo>()
            .HasIndex(a => a.expediente)
            .IsUnique();

        // Identification configuration
        modelBuilder.Entity<Identification>()
            .Property(i => i.Id).HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<Identification>()
            .HasOne(i => i.ArchivoAdministrativo)
            .WithMany()
            .HasForeignKey(i => i.expediente)
            .HasPrincipalKey(a => a.expediente)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Identification>()
            .OwnsOne(i => i.section);

        modelBuilder.Entity<Identification>()
            .OwnsOne(i => i.typology);

        modelBuilder.Entity<Identification>()
            .OwnsOne(i => i.specificName);

        modelBuilder.Entity<Identification>()
            .OwnsOne(i => i.author);

        modelBuilder.Entity<Identification>()
            .OwnsOne(i => i.title);

        modelBuilder.Entity<Identification>()
            .OwnsOne(i => i.material);

        modelBuilder.Entity<Identification>()
            .OwnsOne(i => i.techniques);
        
        // GraphicDocumentation configuration
        modelBuilder.Entity<GraphicDocumentation>()
            .Property(g => g.Id).HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<GraphicDocumentation>()
            .HasOne(g => g.ArchivoAdministrativo)
            .WithOne()
            .HasForeignKey<GraphicDocumentation>(g => g.expediente)
            .HasPrincipalKey<ArchivoAdministrativo>(a => a.expediente)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GraphicDocumentation>()
            .OwnsOne(g => g.dimensions);

        modelBuilder.Entity<GraphicDocumentation>()
            .OwnsOne(g => g.imageAuthor);

        modelBuilder.Entity<GraphicDocumentation>()
            .HasIndex(g => g.expediente)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .Property(u => u.Id).HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<User>()
            .HasIndex(u => new { u.Username, u.TenantId })
            .IsUnique();
        
    }
}