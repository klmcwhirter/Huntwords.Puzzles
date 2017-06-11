using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace puzzles.Models
{
    [DataContract]
    public class WordSolution
    {
        [DataMember]
        public string Word { get; set; }

        [DataMember]
        public bool Placed { get; set; }

        [DataMember]
        public PuzzlePoint Origin { get; set; }

        [DataMember]
        public string Direction => WordSlope.ToString();

        [NotMapped]
        public WordSlope WordSlope { get; set; }

        [DataMember]
        [NotMapped]
        public List<PuzzlePoint> Points { get; set; }
    }
}
