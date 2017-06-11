using System.Runtime.Serialization;

namespace puzzles.Models
{
    [DataContract]
    public class PuzzleBoard
    {
        [DataMember]
        public int Height { get; set; }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public string[,] Letters { get; set; }

        [DataMember]
        public WordSolution[] Solutions { get; set; }
    }
}
