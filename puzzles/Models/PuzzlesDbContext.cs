using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace puzzles.Models
{
    public partial class PuzzlesDbContext : DbContext
    {
        public DbSet<Puzzle> Puzzles { get; set; }
        public DbSet<PuzzleWord> PuzzleWords { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Topic> Topics { get; set; }

        // Default value is required for dotnet ef database update
        private string ConnectionString => GetDefaultConnectionString();

        private string GetDefaultConnectionString()
        {
            // Do not use GetEntryAssembly() - will get wrong results with dotnet ef database update
            var assembly = Assembly.GetCallingAssembly().Location;
            var location = Path.GetDirectoryName(assembly);
            var rc = $"Filename={location}/puzzles.sqlite";
            return rc;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
