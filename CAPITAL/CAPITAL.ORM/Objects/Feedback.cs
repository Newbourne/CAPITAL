using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CAPITAL.ORM.Objects
{
    [DataContract]
    [Serializable]
    [Table("Feedback")]
    public class Feedback : IValidatableObject
    {
        [Key]
        [DataMember]
        public int FeedbackId { get; set; }

        [Required]
        [DataMember]
        public int UserId { get; set; }

        [Required]
        [DataMember]
        public string ReportType { get; set; }

        [Required]
        [DataMember]
        public string Message { get; set; }

        [Required]
        [DataMember]
        public DateTime CreationDate { get; set; }

        public User User { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CreationDate == null)
            {
                yield return new ValidationResult("Invalid Creation Date.");
            }
            if (UserId == 0)
            {
                yield return new ValidationResult("Invalid User.");
            }
            if (string.IsNullOrEmpty(ReportType))
            {
                yield return new ValidationResult("Invalid ReportType.");
            }
            if (string.IsNullOrEmpty(Message))
            {
                yield return new ValidationResult("Invalid Message.");
            }
        }
    }
}
