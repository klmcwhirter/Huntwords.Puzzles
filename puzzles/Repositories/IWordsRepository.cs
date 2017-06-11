namespace puzzles.Repositories
{
    public interface IWordsRepository : IRepository<int, string>
    {
        int WordCount { get; }
    }
}
