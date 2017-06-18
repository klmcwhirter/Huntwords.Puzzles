using puzzles.Models;

namespace puzzles.Repositories
{
    public interface IPuzzlesRepository : IRepository<int, Puzzle>
    {
        Puzzle AddWord(int id, string word);
        Puzzle DeleteWord(int id, int wordId);
    }
}
