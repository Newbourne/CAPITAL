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
    public class Registration : IValidatableObject
    {
        [DataMember]
        public int RegistrationId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string URI { get; set; }
        [DataMember]
        public DateTime? CreationDate { get; set; }

        public User User { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ( UserId == 0 )
            {
                yield return new ValidationResult("Invalid User.");
            }
            if (string.IsNullOrEmpty(URI))
            {
                yield return new ValidationResult("Invalid URI.");
            }
            if (CreationDate == null)
            {
                yield return new ValidationResult("Invalid Creation Date.");
            }
        }
    }
}
