using DiagnostykaItemsAdministrationService.Domain.Entities;
using System.Xml.Linq;

namespace DiagnostykaItemsAdministrationService.Persistence.Extensions;

public static class DataSeeder
{
    public static void Seed(this AppDbContext dbContext)
    {
        SeedColors(dbContext);
    }

    private static void SeedColors(AppDbContext dbContext)
    {
        if (dbContext?.Colors.Any() is false)
        {

            dbContext.AddRange(new List<Color>()
            {
                new Color 
                {
                    Name = "Red"
                },
                new Color {
                    Name =  "Black"
                },
                new Color {
                    Name =  "White"
                }
            });
        }

        dbContext?.SaveChanges();
    }
}