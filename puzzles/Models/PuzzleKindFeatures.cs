using System.Runtime.Serialization;

namespace puzzles.Models
{
    [DataContract]
    public class PuzzleKindFeatures
    {
        [DataMember]
        public bool HasTopics { get; set; }

        [DataMember]
        public bool HasTags { get; set; }

        [DataMember]
        public bool HasSavedPuzzles { get; set; }
    }
}
