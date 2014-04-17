using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CAPITAL.ORM.Exceptions
{
    public class ModelException : ApplicationException
    {
        public ModelException(string message)
            : base(message)
        {
        }

        public ModelException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ModelException(DbEntityValidationException ex)
            :base(GetError(ex))

        {
        }

        public ModelException(DbUnexpectedValidationException ex)
            : base(ex.Message, ex.InnerException)
        {
        }

        public ModelException(IEnumerable<ValidationResult> ex)
            :base(GetError(ex))
        {
        }

        private static string GetError(DbEntityValidationException ex)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var validationErrors in ex.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    builder.AppendLine(validationError.ErrorMessage);
                }
            }
            return builder.ToString();
        }

        private static string GetError(IEnumerable<ValidationResult> ex)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var validationError in ex)
            {
                builder.AppendLine(validationError.ErrorMessage);
            }
            return builder.ToString();
        }
    }
}
