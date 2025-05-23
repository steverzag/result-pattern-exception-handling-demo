using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;
using ResultPatternExceptionHandlingDemo.API.Data.Models;

namespace ResultPatternExceptionHandlingDemo.API.Data
{
	public class AppDBContext : DbContext
	{
		public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>(builder =>
			{
				builder.HasKey(u => u.Id);
				builder.Property(e => e.Id).ValueGeneratedOnAdd();
			});
		}
	}
}
