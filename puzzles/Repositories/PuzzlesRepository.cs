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

        public Puzzle AddWord(int id, string word)
        {
            var puzzle = Get(id);
            if (!puzzle.PuzzleWords.Any(w => w.Word == word))
            {
                puzzle.PuzzleWords.Add(new PuzzleWord { Word = word });
            }
            return puzzle;
        }

        public Puzzle DeleteWord(int id, int wordId)
        {
            var puzzle = Get(id);
            foreach(var puzzleWord in puzzle.PuzzleWords.Where(w => w.Id == wordId).ToArray())
            {
                puzzle.PuzzleWords.Remove(puzzleWord);
            }
            return puzzle;
        }
    }
}
