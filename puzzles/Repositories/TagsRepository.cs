using puzzles.Models;
using Microsoft.EntityFrameworkCore;

namespace puzzles.Repositories
{
    public class TagsRepository : DbRepository<int, Tag>, ITagsRepository
    {
        protected override DbSet<Tag> DbSet => DbContext.Tags;

        public TagsRepository(PuzzlesDbContext context) : base(context)
        {
        }
    }
}
