using System.Collections.Generic;
using System.Linq;
using hwpuzzles.Core.Models;

namespace hwpuzzles.Core.Repositories
{
    /// <summary>
    /// Storage agnostic repository for puzzles
    /// </summary>
    public abstract class PuzzlesRepository : IPuzzlesRepository
    {
        /// <summary>
        /// Gets all the puzzles
        /// </summary>
        public abstract ICollection<Puzzle> GetAll();
        /// <summary>
        /// Gets a puzzle
        /// </summary>
        /// <param name="name">Puzzle name</param>
        /// <returns>Puzzle</returns>
        public abstract Puzzle Get(string name);
        /// <summary>
        /// Add a puzzle
        /// </summary>
        /// <param name="entity">Puzzle</param>
        /// <returns>Puzzle</returns>
        public abstract Puzzle Add(Puzzle entity);

        /// <summary>
        /// Add a word to a puzle
        /// </summary>
        /// <param name="name">Puzzle name</param>
        /// <param name="word"></param>
        /// <returns>Puzzle</returns>
        public abstract Puzzle AddWord(string name, string word);

        /// <summary>
        /// Delete a puzzle
        /// </summary>
        /// <param name="name">Puzzle name</param>
        public abstract void Delete(string name);

        /// <summary>
        /// Delete a word from a puzzle
        /// </summary>
        /// <param name="name">Puzzle name</param>
        /// <param name="word"></param>
        /// <returns>Puzzle</returns>
        public abstract Puzzle DeleteWord(string name, string word);

        /// <summary>
        /// Update a puzzle
        /// </summary>
        /// <param name="name">Puzzle name</param>
        /// <param name="entity">Puzzle</param>
        /// <returns>Puzzle</returns>
        public abstract Puzzle Update(string name, Puzzle entity);

        /// <summary>
        /// Delegate to Repository.UpdateWords(entity)
        /// </summary>
        /// <param name="entity"></param>
        public abstract void UpdateWords(Puzzle entity);

        /// <summary>
        ///  Delegates to Repository to save any changes
        /// </summary>
        public abstract void SaveChanges();
    }
}