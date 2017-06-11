using puzzles.Models;
using Microsoft.EntityFrameworkCore;

namespace puzzles.Repositories
{
    public class TopicsRepository : DbRepository<int, Topic>, ITopicsRepository
    {
        protected override DbSet<Topic> DbSet => DbContext.Topics;

        public TopicsRepository(PuzzlesDbContext context) : base(context)
        {
        }
    }
}
