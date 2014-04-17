using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAPITAL.ORM.Objects;
using CAPITAL.ORM.Exceptions;
using CAPITAL.Engine.Managers;

namespace CAPITAL.Engine.Test
{
    [TestClass]
    public class UserManagementTest
    {
        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void CreateInvalidModelUserTest()
        {
            UserManagement userManagement = new UserManagement();
            userManagement.CreateUser(new User() { Email = "FAILED EMAIL VALIDATION", Password = "FAILED PASSWORD", CreationDate = DateTime.Now, LastAccessDate = DateTime.Now });
        }

        [TestMethod]
        public void CreateValidUserWithoutDeviceTest()
        {
            UserManagement userManagement = new UserManagement();
            userManagement.CreateUser(new User() { Email = "brian@email.com", Password = "password", CreationDate = DateTime.Now, LastAccessDate = DateTime.Now });
        }

        [TestMethod]
        public void CreateValidUserWithDeviceTest()
        {
            UserManagement userManagement = new UserManagement();
            Device device = new Device()
            {
                DeviceName = "Device Name",
                Manufacturer = "MANU",
                CreationDate = DateTime.Now,
                Model = "MODEL",
                UniqueDeviceId = "UNIQUE ID"
            };
            List<Device> devices = new List<Device>();
            devices.Add(device);

            userManagement.CreateUser(new User()
            {
                Email = "device@email.com",
                Password = "password",
                CreationDate = DateTime.Now,
                LastAccessDate = DateTime.Now,
                Devices = devices
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void CreateDuplicateUserTest()
        {
            UserManagement userManagement = new UserManagement();
            userManagement.CreateUser(new User() { Email = "brian@email.com", Password = "password", CreationDate = DateTime.Now, LastAccessDate = DateTime.Now });
        }

        [TestMethod]
        public void SuccessfulLogin()
        {
            UserManagement userManagement = new UserManagement();
            var user = userManagement.Login(new User { Email = "brian@email.com", Password = "password" });
            if (user == null)
                Assert.Fail("Successful login returned null user");
        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void UnsuccessfulLogin()
        {
            UserManagement userManagement = new UserManagement();
            userManagement.Login(new User { Email = "bland@email.com", Password = "password" });
        }
    }
}
