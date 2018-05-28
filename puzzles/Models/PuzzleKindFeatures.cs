#pragma warning disable CS1572, CS1573, CS1591
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
