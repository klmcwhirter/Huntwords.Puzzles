#pragma warning disable CS1572, CS1573, CS1591
namespace hwpuzzles.Core.Repositories
{
    public interface IWordsRepository
    {
        int WordCount { get; }
        string Get(int idx);
    }
}
