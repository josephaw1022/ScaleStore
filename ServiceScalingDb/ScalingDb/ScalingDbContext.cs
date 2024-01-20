namespace ServiceScalingDb.ScalingDb;

using Microsoft.EntityFrameworkCore;



public interface IScalingDbContext
{
    DbSet<Project> Projects { get; set; }
    DbSet<Environment> Environments { get; set; }
    DbSet<Application> Applications { get; set; }
    DbSet<ScalingConfiguration> ScalingConfigurations { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class ScalingDbContext : DbContext , IScalingDbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Environment> Environments { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<ScalingConfiguration> ScalingConfigurations { get; set; }

    public ScalingDbContext(DbContextOptions<ScalingDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>()
            .HasIndex(p => p.ProjectName)
            .IsUnique();

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Environments)
            .WithOne(e => e.Project);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Applications)
            .WithOne(a => a.Project);

        modelBuilder.Entity<Application>()
            .HasMany(a => a.ScalingConfigurations)
            .WithOne(sc => sc.Application);

        modelBuilder.Entity<Environment>()
            .HasMany(e => e.ScalingConfigurations)
            .WithOne(sc => sc.Environment);

        modelBuilder.Entity<Application>()
        .HasIndex(a => new { a.ApplicationName, a.ProjectID })
        .IsUnique();
    }

}

