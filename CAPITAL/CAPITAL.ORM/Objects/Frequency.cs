using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAPITAL.ORM.Objects
{
    [DataContract]
    [Serializable]
    [Table("Frequency")]
    public class Frequency
    {
        [Key]
        [DataMember]
        public int FrequencyId { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [Required]
        [DataMember]
        public int Days { get; set; }

        [Required]
        [DataMember]
        public DateTime CreationDate { get; set; }
    }
}
