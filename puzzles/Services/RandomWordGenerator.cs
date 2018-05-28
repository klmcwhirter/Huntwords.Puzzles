#pragma warning disable CS1572, CS1573, CS1591
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using puzzles.Models;
using puzzles.Repositories;
using puzzles.Utils;

namespace puzzles.Services
{
    [DataContract]
    public class RandomWordGenerator : IPuzzleKind
    {
        public const string StaticKey = "random";

        [DataMember]
        public string Key { get; } = StaticKey;

        public const string StaticName = "Random";

        [DataMember]
        public string Name { get; } = StaticName;

        public const string StaticDescription = "Puzzle containing a randomly selected list of words";

        [DataMember]
        public string Description { get; set; } = StaticDescription;

        static readonly PuzzleKindFeatures RandomPuzzleFeatures = new PuzzleKindFeatures
        {
            HasTopics = false,
            HasTags = false,
            HasSavedPuzzles = false
        };

        [DataMember]
        public PuzzleKindFeatures Features => RandomPuzzleFeatures;


        public Puzzle Puzzle { get; }

        protected IWordsRepository WordRepository { get; }

        public RandomWordGenerator(IWordsRepository wordRepository)
        {
            WordRepository = wordRepository;
            Puzzle = new Puzzle
            {
                Id = -1,
                Name = StaticName,
                Description = StaticDescription,
                PuzzleWords = new List<PuzzleWord>(),
                Kind = this
            };
        }

        public PuzzleWord Generate(params object[] options)
        {
            var idx = WordRepository.WordCount.Random();
            var rc = new PuzzleWord
            {
                Id = -1,
                Word = WordRepository.Get(idx)
            };

            Puzzle.PuzzleWords.Add(rc);

            return rc;
        }
    }
}
