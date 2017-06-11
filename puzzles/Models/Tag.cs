using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace puzzles.Models
{
    [DataContract]
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
