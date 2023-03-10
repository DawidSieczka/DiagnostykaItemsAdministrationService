using DiagnostykaItemsAdministrationService.Domain.Entities;
using DiagnostykaItemsAdministrationService.Domain.Rules;
using Microsoft.EntityFrameworkCore;

namespace DiagnostykaItemsAdministrationService.Persistence;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions options) : base(options) {	}

	public DbSet<Color> Colors { get; set; }
	public DbSet<Item> Items { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Item>(eb =>
		{
			eb.HasOne(i => i.Color)
				.WithMany(c => c.Items)
				.HasForeignKey(i => i.ColorId)
				.OnDelete(DeleteBehavior.SetNull);

			eb.Property(i => i.Name).HasMaxLength(ItemRules.NameMaxLength);
			eb.Property(i => i.Code).HasMaxLength(12).IsFixedLength();
		});

		modelBuilder.Entity<Color>(eb =>
		{
			eb.Property(i => i.Name).HasMaxLength(ColorRules.NameMaxLength).IsRequired();
		});

		base.OnModelCreating(modelBuilder);
	}
}