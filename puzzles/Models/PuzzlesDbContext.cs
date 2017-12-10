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

        public PuzzlesDbContext(DbContextOptions<PuzzlesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder
                .Entity<Puzzle>()
                .HasMany(e => e.PuzzleWords)
                .WithOne(e => e.Puzzle)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
    }
}
