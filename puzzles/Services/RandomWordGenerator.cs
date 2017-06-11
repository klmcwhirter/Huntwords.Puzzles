using System.Runtime.Serialization;
using puzzles.Models;
using puzzles.Repositories;
using puzzles.Utils;

namespace puzzles.Services
{
    [DataContract]
    public class RandomWordGenerator : IPuzzleKind
    {
        public static readonly string StaticKey = "random";

        [DataMember]
        public string Key { get; } = StaticKey;

        [DataMember]
        public string Name { get; } = "Random";

        [DataMember]
        public string Description { get; set; } = "Puzzle containing a randomly selected list of words";

        static readonly PuzzleKindFeatures RandomPuzzleFeatures = new PuzzleKindFeatures
        {
            HasTopics = false,
            HasTags = false,
            HasSavedPuzzles = false
        };

        [DataMember]
        public PuzzleKindFeatures Features => RandomPuzzleFeatures;

        public int? PuzzleId { get; } = null;

        protected IWordsRepository WordRepository { get; }

        public RandomWordGenerator(IWordsRepository wordRepository)
        {
            WordRepository = wordRepository;
        }

        public PuzzleWord Generate(params object[] options)
        {
            var idx = WordRepository.WordCount.Random();
            var rc = new PuzzleWord {
                Id = -1,
                Word = WordRepository.Get(idx)
            };
            return rc;
        }
    }
}
