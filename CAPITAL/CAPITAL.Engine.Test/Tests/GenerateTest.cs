using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAPITAL.Engine.Test
{
    [TestClass]
    public class GenerateTest
    {
        [TestMethod]
        public void GenerateDatabaseTest()
        {
            Database.GenerateDatabase();
        }
    }
}
