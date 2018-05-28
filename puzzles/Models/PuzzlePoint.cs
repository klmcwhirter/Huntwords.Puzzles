#pragma warning disable CS1572, CS1573, CS1591
using System.Runtime.Serialization;

namespace puzzles.Models
{
    [DataContract]
    // This struct exists because System.Drawing.Point contains properties not needed
    // These make the JSON reposonse for the REST API large than it should be.
    public struct PuzzlePoint
    {
        public PuzzlePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        [DataMember]
        public int X { get; set; }

        [DataMember]
        public int Y { get; set; }
    }
}
