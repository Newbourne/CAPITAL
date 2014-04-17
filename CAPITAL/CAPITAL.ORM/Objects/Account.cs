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
    public class Account : IValidatableObject
    {
        [Key]
        [DataMember]
        public int AccountId { get; set; }

        [Required]
        [DataMember]
        public int UserId { get; set; }

        [Required]
        [DataMember]
        public string AccountName { get; set; }

        [Required]
        [DataMember]
        public int FrequencyId { get; set; }

        [Required]
        [DataMember]
        public int AccountTypeId { get; set; }

        [Required]
        [DataMember]
        public double Balance { get; set; }

        [Required]
        [DataMember]
        public double DefaultPayment { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }

        [Required]
        [DataMember]
        public DateTime? CreationDate { get; set; }

        [Required]
        [DataMember]
        [NotMapped]
        public bool IsShare { get; set; }

        [Required]
        [DataMember]
        [NotMapped]
        public int PermissionLevel { get; set; }

        [Required]
        [DataMember]
        [NotMapped]
        public bool IsNew { get; set; }

        [DataMember]
        public Frequency Frequency { get; set; }
        [DataMember]
        public AccountType AccountType { get; set; }

        public ICollection<Statement> Statements { get; set; }
        public User User { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CreationDate == null)
            {
                yield return new ValidationResult("Invalid Creation Date.");
            }
            if (StartDate == null)
            {
                yield return new ValidationResult("Invalid Start Date.");
            }
            if (UserId == 0)
            {
                yield return new ValidationResult("Invalid User.");
            }
            if (string.IsNullOrWhiteSpace(AccountName))
            {
                yield return new ValidationResult("Invalid Account Name.");
            }
            if (FrequencyId  < 1)
            {
                yield return new ValidationResult("Invalid Frequency.");
            }
            if (AccountTypeId < 1)
            {
                yield return new ValidationResult("Invalid Account Type.");
            }
        }
    }
}
