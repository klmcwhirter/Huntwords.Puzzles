using System.Collections.Generic;
using System.Runtime.Serialization;
using puzzles.Models;

namespace puzzles.Services
{
    [DataContract]
    public class WordWordGenerator : IPuzzleKind
    {
        public const string StaticKey = "word";

        [DataMember]
        public string Key { get; } = StaticKey;

        public const string StaticName = "Word";

        [DataMember]
        public string Name { get; } = StaticName;

        public const string StaticDescription = "Puzzle containing a list of the word WORD";
    
        [DataMember]
        public string Description { get; set; } = StaticDescription;

        static readonly PuzzleKindFeatures WordPuzzleFeatures = new PuzzleKindFeatures
        {
            HasTopics = false,
            HasTags = false,
            HasSavedPuzzles = false
        };

        [DataMember]
        public PuzzleKindFeatures Features => WordPuzzleFeatures;

        public Puzzle Puzzle { get; }

        public WordWordGenerator()
        {
            Puzzle = new Puzzle
            {
                Id = -2,
                Name = StaticName,
                Description = StaticDescription,
                PuzzleWords = new List<PuzzleWord>(),
                Kind = this
            };
        }
        public PuzzleWord Generate(params object[] options)
        {
            var rc = new PuzzleWord
            {
                Id = -1,
                Word = "word"
            };

            Puzzle.PuzzleWords.Add(rc);

            return rc;
        }
    }
}
