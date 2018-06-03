#pragma warning disable CS1572, CS1573, CS1591
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace hwpuzzles.Core.Models
{
    [DataContract]
    public class Puzzle
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Topics { get; set; }

        [DataMember]
        public string Tags { get; set; }

        [DataMember]
        public ICollection<string> PuzzleWords { get; set; }

        [NotMapped]
        public string[] Words => PuzzleWords.Select(pw => pw).ToArray();
    }
}
