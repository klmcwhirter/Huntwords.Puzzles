#pragma warning disable CS1572, CS1573, CS1591
namespace puzzles.Repositories
{
    public interface IWordsRepository : IRepository<int, string>
    {
        int WordCount { get; }
    }
}
