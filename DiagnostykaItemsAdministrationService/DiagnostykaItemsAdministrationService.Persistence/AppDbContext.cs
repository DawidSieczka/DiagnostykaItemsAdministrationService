using Microsoft.EntityFrameworkCore;

namespace DiagnostykaItemsAdministrationService.Persistence;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions options) : base(options) {	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
	}

}