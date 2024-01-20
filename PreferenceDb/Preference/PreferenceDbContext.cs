using Microsoft.EntityFrameworkCore;

namespace PreferenceDb.Preference
{
	public class PreferenceDbContext : DbContext
	{
		public DbSet<ProjectPreference> ProjectPreferences { get; set; }

		public PreferenceDbContext(DbContextOptions<PreferenceDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Configure the primary key for ProjectPreference
			modelBuilder.Entity<ProjectPreference>()
				.HasKey(p => p.Id);

			// Configure a unique constraint on the combination of ProjectId and UserId
			modelBuilder.Entity<ProjectPreference>()
				.HasIndex(p => new { p.ProjectId, p.UserId })
				.IsUnique();
		}
	}
}