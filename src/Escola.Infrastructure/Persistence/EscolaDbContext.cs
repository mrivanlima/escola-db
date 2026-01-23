using Escola.Domain.Assets;
using Escola.Domain.Content;
using Escola.Domain.Game;
using Escola.Domain.Identity;
using Escola.Domain.School;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infrastructure.Persistence;

/// <summary>
/// Entity Framework Core database context for the Escola Platform.
/// </summary>
public class EscolaDbContext : DbContext
{
    public EscolaDbContext(DbContextOptions<EscolaDbContext> options) : base(options)
    {
    }

    // Identity Schema
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    // Assets Schema
    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();

    // School Schema
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Guardian> Guardians => Set<Guardian>();
    public DbSet<StudentGuardian> StudentGuardians => Set<StudentGuardian>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<ClassStudent> ClassStudents => Set<ClassStudent>();

    // Content Schema
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<ActivityResource> ActivityResources => Set<ActivityResource>();

    // Game Schema
    public DbSet<StudentProgress> StudentProgress => Set<StudentProgress>();
    public DbSet<Badge> Badges => Set<Badge>();
    public DbSet<StudentBadge> StudentBadges => Set<StudentBadge>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EscolaDbContext).Assembly);
    }
}
