using System.Linq;
using puzzles.Models;
using Microsoft.EntityFrameworkCore;

namespace puzzles.Repositories
{
    public class PuzzlesRepository : DbRepository<int, Puzzle>, IPuzzlesRepository
    {
        protected override DbSet<Puzzle> DbSet => DbContext.Puzzles;

        public PuzzlesRepository(PuzzlesDbContext context) : base(context)
        {
        }

        public override IQueryable<Puzzle> GetAll()
        {
            var rc = DbSet
                        .Include(p => p.PuzzleWords);
            return rc;
        }

        public override Puzzle Get(int id)
        {
            var rc = GetAll()
                        .Where(p => p.Id == id)
                        .FirstOrDefault();
            return rc;
        }

    }
}
