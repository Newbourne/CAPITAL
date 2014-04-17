using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CAPITAL.ORM.Exceptions;

namespace CAPITAL.ORM.Objects
{
    [DataContract]
    [Serializable]
    public class CapitalError
    {
        [DataMember]
        public string Message { get; set; }

        public CapitalError(ModelException ex)
        {
            Message = ex.Message;
        }
    }
}
