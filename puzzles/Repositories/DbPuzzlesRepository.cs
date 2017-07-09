using System.Linq;
using puzzles.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace puzzles.Repositories
{
    public class DbPuzzlesRepository : DbRepository<int, Puzzle>, IPuzzlesRepository
    {
        public const string StaticKey = "db";

        protected override DbSet<Puzzle> DbSet => DbContext.Puzzles;
        public ILogger<DbPuzzlesRepository> Logger { get; }

        public DbPuzzlesRepository(PuzzlesDbContext context, ILogger<DbPuzzlesRepository> logger) : base(context)
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
            Logger.LogInformation($"DbPuzzleRepository.Get({id})");
            var rc = GetAll()
                        .Where(p => p.Id == id)
                        .FirstOrDefault();
            Logger.LogInformation($"DbPuzzleRepository.Get({id}): PuzzleWords.Count={rc.PuzzleWords.Count}");
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

        public override void Delete(int id)
        {
            Logger.LogInformation($"DbPuzzleRepository.Delete({id})");
            var entity = Get(id);
            foreach (var word in entity.PuzzleWords.ToArray())
            {
                entity.PuzzleWords.Remove(word);
                DbContext.PuzzleWords.Remove(word);
            }
            Logger.LogInformation($"DbPuzzleRepository.Delete({id}): PuzzleWords.Count={entity.PuzzleWords.Count}");
            
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
