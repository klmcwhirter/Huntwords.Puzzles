namespace Huntwords.Puzzles.Models
{
    /// <summary>
    /// Represents a word in a puzzle
    /// </summary>
    /// <remarks>Used in the add word API</remarks>
    public class PuzzleWord
    {
        /// <summary>
        /// the word to add
        /// </summary>
        /// <value>string</value>
        public string Word { get; set; }
    }
}
