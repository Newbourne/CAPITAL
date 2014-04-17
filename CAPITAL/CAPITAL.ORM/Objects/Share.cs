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
    [Table("Shares")]
    public class Share : IValidatableObject
    {
        [Key]
        [DataMember]
        public int ShareId { get; set; }

        [Required]
        [DataMember]
        public int UserId { get; set; }

        [Required]
        [DataMember]
        public int AccountId { get; set; }

        [Required]
        [DataMember]
        public int PermissionLevelId { get; set; }

        public User User { get; set; }
        public Account Account { get; set; }
        public PermissionLevel PermissionLevel { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserId == 0 || AccountId == 0 || PermissionLevelId == 0)
            {
                yield return new ValidationResult("Invalid Account Share.");
            }
        }
    }
}
