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
    public class User : IValidatableObject
    {
        [Key]
        [DataMember]
        public int UserId { get; set; }

        [Required]
        [DataMember]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", 
            ErrorMessage="Invalid Email Address.")]
        public string Email { get; set; }

        [Required]
        [DataMember]
        public string Password { get; set; }

        [Required]
        [DataMember]
        public DateTime? LastAccessDate { get; set; }
        
        [Required]
        [DataMember]
        public DateTime? CreationDate { get; set; }

        [DataMember]
        public ICollection<Device> Devices { get; set; }

        public ICollection<Account> Accounts { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CreationDate == null)
            {
                yield return new ValidationResult("Invalid Creation Date.");
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                yield return new ValidationResult("Invalid Email Address.");
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                yield return new ValidationResult("Invalid Password.");
            }
            if (LastAccessDate == null)
            {
                yield return new ValidationResult("Invalid Last Access Date.");
            }
            if (Devices != null && Devices.Count() > 0)
            {
                foreach (Device device in Devices)
                {
                    ValidationContext valContext = new ValidationContext(this, null, null);
                    var errors = device.Validate(valContext);
                    if (errors.Count() > 0)
                        yield return new ValidationResult(device.DeviceName + " is invalid.");
                }
            }
        }
    }
}
