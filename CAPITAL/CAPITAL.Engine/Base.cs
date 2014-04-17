using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAPITAL.ORM.Objects;
using System.Xml.Serialization;
using System.IO;
using CAPITAL.ORM;

namespace CAPITAL.Engine
{
    public abstract class Base
    {
        public void LogError(object dataObj, Exception exception, string functionName)
        {
            try
            {
                XmlSerializer xml = new XmlSerializer(dataObj.GetType());
                StringWriter stream = new StringWriter();
                xml.Serialize(stream, dataObj);


                CapitalLogError error = new CapitalLogError();
                error.DataObject = stream.ToString();
                error.EventDate = DateTime.Now;
                error.ExceptionMessage = exception.Message;
                error.FunctionName = functionName;

                using (CapitalContext context = new CapitalContext())
                {
                    context.Errors.Add(error);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }

        public void LogError(object[] dataObj, Exception exception, string functionName)
        {
            try
            {
                XmlSerializer xml = new XmlSerializer(dataObj.GetType());
                StringWriter stream = new StringWriter();
                xml.Serialize(stream, dataObj);


                CapitalLogError error = new CapitalLogError();
                error.DataObject = stream.ToString();
                error.EventDate = DateTime.Now;
                error.ExceptionMessage = exception.Message;
                error.FunctionName = functionName;

                using (CapitalContext context = new CapitalContext())
                {
                    context.Errors.Add(error);
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
