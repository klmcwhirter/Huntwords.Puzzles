using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using puzzles.Models;
using puzzles.Utils;

namespace puzzles.Services
{
    [DataContract]
    public class AnimalsWordGenerator : IPuzzleKind
    {
        public static readonly string StaticKey = "animals";

        [DataMember]
        public string Key { get; } = StaticKey;

        [DataMember]
        public string Name { get; } = "Animals";

        [DataMember]
        public string Description { get; set; } = "Puzzle containing a randomly selected list of animals.";

        public bool IsIdValid(int? id) => true;

        static readonly PuzzleKindFeatures AnimalsPuzzleFeatures = new PuzzleKindFeatures
        {
            HasTopics = false,
            HasTags = false,
            HasSavedPuzzles = false
        };

        [DataMember]
        public PuzzleKindFeatures Features => AnimalsPuzzleFeatures;

        public int? PuzzleId { get; } = null;

        public string[] Animals { get; set; } =
        {
            "albacoretuna",
            "alligator",
            "bat",
            "bear",
            "beta",
            "bison",
            "bluewhale",
            "boa",
            "boar",
            "cat",
            "cheetah",
            "chicken",
            "chimpanzee",
            "chipmunk",
            "cicada",
            "cougar",
            "cow",
            "coyote",
            "cricket",
            "crow",
            "deer",
            "dog",
            "dolphin",
            "dungbeetle",
            "eagle",
            "earthworm",
            "eel",
            "elephant",
            "falcon",
            "fox",
            "gilamonster",
            "giraffe",
            "goat",
            "goose",
            "gorilla",
            "grasshopper",
            "hawk",
            "horse",
            "hummingbird",
            "joey",
            "kangaroo",
            "kestrel",
            "lion",
            "lizzard",
            "mantaray",
            "meerkat",
            "mockingbird",
            "mole",
            "moth",
            "mouse",
            "mule",
            "orca",
            "ocelot",
            "opossum",
            "owl",
            "pig",
            "pika",
            "platypus",
            "porcupine",
            "prariedog",
            "rat",
            "rhinoceros",
            "salamander",
            "salmon",
            "shark",
            "sheep",
            "shrew",
            "skunk",
            "snake",
            "sow",
            "sparrow",
            "spidermonkey",
            "squirrel",
            "tiger",
            "turkey",
            "wombat",
            "zebra"
        };

        protected int CurrentIdx { get; set; } = 0;
        protected ISet<string> SeenAnimal { get; set; }

        public PuzzleWord Generate(params object[] options)
        {
            if (SeenAnimal == null || SeenAnimal.Count == Animals.Length)
            {
                SeenAnimal = new SortedSet<string>();
            }

            string word;
            do
            {
                var idx = Animals.Length.Random();
                word = Animals[idx];
            } while (SeenAnimal.Contains(word));

            SeenAnimal.Add(word);

            var rc = new PuzzleWord
            {
                Id = -1,
                Word = word
            };
            return rc;
        }
    }
}
