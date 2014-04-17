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
    public class Device : IValidatableObject
    {
        [Key]
        [DataMember]
        public int DeviceId { get; set; }

        [Required]
        [DataMember]
        public int UserId { get; set; }

        [Required]
        [DataMember]
        public string UniqueDeviceId { get; set; }

        [Required]
        [DataMember]
        public string DeviceName { get; set; }

        [Required]
        [DataMember]
        public string Manufacturer { get; set; }

        [Required]
        [DataMember]
        public string Model { get; set; }

        [Required]
        [DataMember]
        public DateTime? CreationDate { get; set; }

        public User User { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(UniqueDeviceId))
            {
                yield return new ValidationResult("Invalid Unique ID.");
            }

            if (CreationDate == null)
            {
                yield return new ValidationResult("Invalid Creation Date.");
            }
        }
    }
}
