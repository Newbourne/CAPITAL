using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CAPITAL.ORM.Objects
{
    public class CapitalLogError
    {
        [Key]
        public int ErrorId { get; set; }

        public string FunctionName { get; set; }

        public string ExceptionMessage { get; set; }

        public string DataObject { get; set; }

        public DateTime? EventDate { get; set; }
    }
}
