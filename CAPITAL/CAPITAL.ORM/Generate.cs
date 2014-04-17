using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace CAPITAL.ORM
{
    public static class Generate
    {
        public static void Start()
        {
            Database.SetInitializer(new CapitalContextInitializer());
        }
    }
}
