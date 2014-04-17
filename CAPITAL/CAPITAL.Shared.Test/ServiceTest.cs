using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAPITAL.Shared.Test.CapitalService;
using System.ServiceModel;

namespace CAPITAL.Shared.Test
{
    [TestClass]
    public class ServiceTest
    {
        [TestMethod]
        public void LoginSuccess()
        {
            CapitalServiceClient client = new CapitalServiceClient();
            try
            {
                User user = client.Login(new User() { Email = "brian@email.com", Password = "password" });
            }
            catch (FaultException<CapitalError> ex)
            {
                client.Abort();
                throw ex;
            }
            catch (Exception ex)
            {
                client.Abort();
                throw ex;
            }
            finally
            {
                client.Close();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<CapitalError>))]
        public void LoginFail()
        {
            CapitalServiceClient client = new CapitalServiceClient();
            try
            {
                User user = client.Login(new User() { Email = "brian@asdafas.com", Password = "password" });
            }
            catch (FaultException<CapitalError> ex)
            {
                client.Abort();
                throw ex;
            }
            catch (Exception ex)
            {
                client.Abort();
                throw ex;
            }
            finally
            {
                client.Close();
            }
        }
    }
}
