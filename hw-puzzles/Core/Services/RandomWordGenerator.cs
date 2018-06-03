#pragma warning disable CS1572, CS1573, CS1591
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using hwpuzzles.Core.Models;
using hwpuzzles.Core.Repositories;
using hwpuzzles.Core.Utils;

namespace hwpuzzles.Core.Services
{
    [DataContract]
    public class RandomWordGenerator
    {
        public Puzzle Puzzle { get; }

        protected IWordsRepository WordRepository { get; }

        public RandomWordGenerator(IWordsRepository wordRepository)
        {
            WordRepository = wordRepository;
            Puzzle = new Puzzle
            {
                Name = "random",
                Description = "Puzzle containing a randomly selected list of words",
                PuzzleWords = new List<string>()
            };
        }

        public string Generate(params object[] options)
        {
            var idx = WordRepository.WordCount.Random();
            var rc = WordRepository.Get(idx);

            Puzzle.PuzzleWords.Add(rc);

            return rc;
        }
    }
}
