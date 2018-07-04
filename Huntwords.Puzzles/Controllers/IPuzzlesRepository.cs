#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using Huntwords.Common.Models;

namespace Huntwords.Puzzles.Controllers
{
    /// <summary>
    /// Contract for a puzzle repository
    /// </summary>
    public interface IPuzzlesRepository
    {
        /// <summary>
        /// Gets all the puzzles
        /// </summary>
        ICollection<Puzzle> GetAll();
        /// <summary>
        /// Gets a puzzle
        /// </summary>
        /// <param name="name">Puzzle name</param>
        /// <returns>Puzzle</returns>
        Puzzle Get(string name);
        /// <summary>
        /// Adds a puzzle
        /// </summary>
        /// <param name="puzzle">Puzzle</param>
        Puzzle Add(Puzzle puzzle);
        /// <summary>
        /// Add a word to a puzzle
        /// </summary>
        /// <param name="name">Puzzle name</param>
        /// <param name="puzzle">Puzzle</param>
        /// <returns>Puzzle</returns>
        Puzzle Update(string name, Puzzle puzzle);
        /// <summary>
        /// Deletes a puzzle
        /// </summary>
        /// <param name="name">Puzzle name</param>
        void Delete(string name);        
        /// <summary>
        /// Add a word to a puzzle
        /// </summary>
        /// <param name="name">Puzzle name</param>
        /// <param name="word"></param>
        /// <returns>Puzzle</returns>
        Puzzle AddWord(string name, string word);
        /// <summary>
        /// Delete a word from a puzzle
        /// </summary>
        /// <param name="name">Puzzle name</param>
        /// <param name="word"></param>
        /// <returns>Puzzle</returns>
        Puzzle DeleteWord(string name, string word);
    }
}
