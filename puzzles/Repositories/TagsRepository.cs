using puzzles.Models;
using Microsoft.EntityFrameworkCore;

namespace puzzles.Repositories
{
    /// <summary>
    /// Repository for tags
    /// </summary>
    public class TagsRepository : DbRepository<int, Tag>, ITagsRepository
    {
        /// <summary>
        /// Helper to get at PuzzlesDbContext
        /// </summary>
        /// <returns></returns>
        public PuzzlesDbContext PuzzlesDbContext => (PuzzlesDbContext)DbContext;
        /// <summary>
        /// DbSet being operated on by this repository
        /// </summary>
        protected override DbSet<Tag> DbSet => PuzzlesDbContext.Tags;

        /// <summary>
        /// Construct a TagsRepository
        /// </summary>
        /// <param name="context">PuzzlesDbContext</param>
        public TagsRepository(PuzzlesDbContext context) : base(context)
        {
        }
    }
}
