#pragma warning disable CS1572, CS1573, CS1591
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace puzzles.Models
{
public class PuzzlesDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PuzzlesDbContext>
{
    public PuzzlesDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var builder = new DbContextOptionsBuilder<PuzzlesDbContext>();
        var connectionString = configuration.GetConnectionString("puzzles");
        builder.UseSqlite(connectionString);
        return new PuzzlesDbContext(builder.Options);
    }
}
}