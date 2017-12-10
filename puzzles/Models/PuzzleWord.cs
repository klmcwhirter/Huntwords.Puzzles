using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace puzzles.Models
{
    [Table("PuzzleWords")]
    [DataContract]
    public class PuzzleWord
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Word { get; set; }

        [NotMapped]
        public Puzzle Puzzle { get; set; }
    }
}
