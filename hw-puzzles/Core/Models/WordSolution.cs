#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace hwpuzzles.Core.Models
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
