using System.Runtime.Serialization;
using puzzles.Models;

namespace puzzles.Services
{
    [DataContract]
    public class WordWordGenerator : IPuzzleKind
    {
        public static readonly string StaticKey = "word";

        [DataMember]
        public string Key { get; } = StaticKey;

        [DataMember]
        public string Name { get; } = "Word";

        [DataMember]
        public string Description { get; set; } = "Puzzle containing a list of the word WORD";

        static readonly PuzzleKindFeatures WordPuzzleFeatures = new PuzzleKindFeatures
        {
            HasTopics = false,
            HasTags = false,
            HasSavedPuzzles = false
        };

        [DataMember]
        public PuzzleKindFeatures Features => WordPuzzleFeatures;

        public int? PuzzleId { get; } = null;

        public PuzzleWord Generate(params object[] options)
        {
            var rc = new PuzzleWord
            {
                Id = -1,
                Word = "word"
            };
            return rc;
        }
    }
}
