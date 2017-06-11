using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace puzzles.Models
{
    [DataContract]
    public class Puzzle
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Topics { get; set; }

        [DataMember]
        public string Tags { get; set; }

        [DataMember]
        public IEnumerable<PuzzleWord> PuzzleWords { get; set; }

        [NotMapped]
        [DataMember]
        public IPuzzleKind Kind { get; set; }

        [NotMapped]
        public string[] Words => PuzzleWords.Select(pw => pw.Word).ToArray();

        [NotMapped]
        [DataMember]
        public PuzzleBoard Board { get; set; }
    }
}
