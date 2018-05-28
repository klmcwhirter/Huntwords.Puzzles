#pragma warning disable CS1572, CS1573, CS1591
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace puzzles.Models
{
    [DataContract]
    public class Tag
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
