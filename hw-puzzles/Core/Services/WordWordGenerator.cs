#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using System.Runtime.Serialization;
using hwpuzzles.Core.Models;

namespace hwpuzzles.Core.Services
{
    [DataContract]
    public class WordWordGenerator
    {
        public Puzzle Puzzle { get; }

        public WordWordGenerator()
        {
            Puzzle = new Puzzle
            {
                Name = "Word",
                Description = "Puzzle containing a list of the word WORD",
                PuzzleWords = new List<string>()
            };
        }
        public string Generate(params object[] options)
        {
            var rc = "word";

            Puzzle.PuzzleWords.Add(rc);

            return rc;
        }
    }
}
