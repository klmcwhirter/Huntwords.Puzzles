using puzzles.Models;
using Microsoft.EntityFrameworkCore;

namespace puzzles.Repositories
{
    /// <summary>
    /// Repository for topics
    /// </summary>
    public class TopicsRepository : DbRepository<int, Topic>, ITopicsRepository
    {
        /// <summary>
        /// Helper to get at PuzzlesDbContext
        /// </summary>
        /// <returns></returns>
        public PuzzlesDbContext PuzzlesDbContext => (PuzzlesDbContext)DbContext;

        /// <summary>
        /// DbSet being operated on by this repository
        /// </summary>
        protected override DbSet<Topic> DbSet => PuzzlesDbContext.Topics;

        /// <summary>
        /// Construct a TopicsRepository
        /// </summary>
        /// <param name="context">PuzzlesDbContext</param>
        public TopicsRepository(PuzzlesDbContext context) : base(context)
        {
        }
    }
}
