using puzzles.Models;

namespace puzzles.Repositories
{
    /// <summary>
    /// Contract for a puzzle repository
    /// </summary>
    public interface IPuzzlesRepository : IRepository<int, Puzzle>
    {
        /// <summary>
        /// Add a word to a puzzle
        /// </summary>
        /// <param name="id">Puzzle id</param>
        /// <param name="word"></param>
        /// <returns>Puzzle</returns>
        Puzzle AddWord(int id, string word);
        /// <summary>
        /// Delete a word from a puzzle
        /// </summary>
        /// <param name="id">Puzzle id</param>
        /// <param name="wordId"></param>
        /// <returns>Puzzle</returns>
        Puzzle DeleteWord(int id, int wordId);
        /// <summary>
        /// Updates the words in a puzzle
        /// </summary>
        /// <param name="entity">Puzzle</param>
        void UpdateWords(Puzzle entity);
    }
}
