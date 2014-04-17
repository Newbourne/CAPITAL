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
    public class Statement
    {
        [Key]
        [DataMember]
        public int StatementId { get; set; }

        [Required]
        [DataMember]
        public int AccountId { get; set; }

        [Required]
        [DataMember]
        public double Balance { get; set; }

        [Required]
        [DataMember]
        public double PaidAmount { get; set; }

        [Required]
        [DataMember]
        public bool IsPaid { get; set; }

        [DataMember]
        public DateTime? DueDate { get; set; }

        [DataMember]
        public DateTime? PaidDate { get; set; }

        [Required]
        [DataMember]
        public DateTime CreationDate { get; set; }

        [DataMember]
        [NotMapped]
        public string AccountName { get; set; }

        public Account Account { get; set; }
    }
}
