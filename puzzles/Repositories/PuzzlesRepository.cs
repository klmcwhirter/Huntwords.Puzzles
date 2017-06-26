using System.Linq;
using puzzles.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace puzzles.Repositories
{
    public class PuzzlesRepository : DbRepository<int, Puzzle>, IPuzzlesRepository
    {
        protected override DbSet<Puzzle> DbSet => DbContext.Puzzles;
        public ILogger<PuzzlesRepository> Logger { get; }

        public PuzzlesRepository(PuzzlesDbContext context, ILogger<PuzzlesRepository> logger) : base(context)
        {
            Logger = logger;
        }

        public override IQueryable<Puzzle> GetAll()
        {
            var rc = DbSet
                        .Include(p => p.PuzzleWords);
            return rc;
        }

        public override Puzzle Get(int id)
        {
            Logger.LogInformation($"PuzzleRepository.Get({id})");
            var rc = GetAll()
                        .Where(p => p.Id == id)
                        .FirstOrDefault();
            Logger.LogInformation($"PuzzleRepository.Get({id}): PuzzleWords.Count={rc.PuzzleWords.Count}");
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

        public void Delete(int id)
        {
            Logger.LogInformation($"PuzzleRepository.Delete({id})");
            var entity = Get(id);
            foreach (var word in entity.PuzzleWords.ToArray())
            {
                entity.PuzzleWords.Remove(word);
                DbContext.PuzzleWords.Remove(word);
            }
            Logger.LogInformation($"PuzzleRepository.Delete({id}): PuzzleWords.Count={entity.PuzzleWords.Count}");
            
            DbSet.Remove(entity);
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
