using System.Linq;
using puzzles.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace puzzles.Repositories
{
    /// <summary>
    /// Repository for puzzles that extends DbRepository
    /// </summary>
    public class DbPuzzlesRepository : DbRepository<int, Puzzle>, IPuzzlesRepository
    {
        /// <summary>
        /// Helper to get at PuzzlesDbContext
        /// </summary>
        /// <returns></returns>
        public PuzzlesDbContext PuzzlesDbContext => (PuzzlesDbContext)DbContext;

        /// <summary>
        /// DbSet operated on by this repository
        /// </summary>
        protected override DbSet<Puzzle> DbSet => PuzzlesDbContext.Puzzles;

        /// <summary>
        /// Logger property
        /// </summary>
        /// <returns>ILogger</returns>
        protected ILogger<DbPuzzlesRepository> Logger { get; }

        /// <summary>
        /// Construct a DbPuzzlesRepository
        /// </summary>
        /// <param name="context">PuzzlesDbContext</param>
        /// <param name="logger">ILogger</param>
        /// <returns></returns>
        public DbPuzzlesRepository(PuzzlesDbContext context, ILogger<DbPuzzlesRepository> logger) : base(context)
        {
            Logger = logger;
        }

        /// <summary>
        /// Get all puzzles
        /// </summary>
        /// <returns>IQueryable&lt;Puzzle&gt;</returns>
        public override IQueryable<Puzzle> GetAll()
        {
            var rc = DbSet
                        .Include(p => p.PuzzleWords);
            return rc;
        }

        /// <summary>
        /// Get a puzzle
        /// </summary>
        /// <param name="id">Puzzle id</param>
        /// <returns>Puzzle</returns>
        public override Puzzle Get(int id)
        {
            Logger.LogInformation($"DbPuzzleRepository.Get({id})");
            var rc = GetAll()
                        .Where(p => p.Id == id)
                        .FirstOrDefault();
            Logger.LogInformation($"DbPuzzleRepository.Get({id}): PuzzleWords.Count={rc.PuzzleWords.Count}");
            return rc;
        }

        /// <summary>
        /// Add a word to a puzzle
        /// </summary>
        /// <param name="id">Puzzle id</param>
        /// <param name="word"></param>
        /// <returns></returns>
        public Puzzle AddWord(int id, string word)
        {
            var puzzle = Get(id);
            if (!puzzle.PuzzleWords.Any(w => w.Word == word))
            {
                puzzle.PuzzleWords.Add(new PuzzleWord { Word = word });
            }
            return puzzle;
        }

        /// <summary>
        /// Delete a puzzle
        /// </summary>
        /// <param name="id">Puzzle id</param>
        public override void Delete(int id)
        {
            Logger.LogInformation($"DbPuzzleRepository.Delete({id})");
            var entity = Get(id);
            foreach (var word in entity.PuzzleWords.ToArray())
            {
                entity.PuzzleWords.Remove(word);
            }
            Logger.LogInformation($"DbPuzzleRepository.Delete({id}): PuzzleWords.Count={entity.PuzzleWords.Count}");

            DbSet.Remove(entity);
        }

        /// <summary>
        /// Delete a word
        /// </summary>
        /// <param name="id"></param>
        /// <param name="wordId"></param>
        /// <returns></returns>
        public Puzzle DeleteWord(int id, int wordId)
        {
            var puzzle = Get(id);
            foreach (var puzzleWord in puzzle.PuzzleWords.Where(w => w.Id == wordId).ToArray())
            {
                puzzle.PuzzleWords.Remove(puzzleWord);
            }
            return puzzle;
        }

        /// <summary>
        /// Update the words from the entity passed in
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        public void UpdateWords(Puzzle entity)
        {
            entity.PuzzleWords.ToList().ForEach(word =>
            {
                var puzzleWord = PuzzlesDbContext.PuzzleWords.AsNoTracking().Where(w => w.Id == word.Id).FirstOrDefault();
                if (puzzleWord.Word != word.Word)
                {
                    var entry = PuzzlesDbContext.Attach(word);
                    entry.State = EntityState.Modified;
                }
            });
        }
    }
}
