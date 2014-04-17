using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAPITAL.Engine.Interfaces;
using CAPITAL.ORM;
using CAPITAL.ORM.Objects;

namespace CAPITAL.Engine.Utilities
{
    public class AccountUtil : Base,  IAccountUtil
    {
        public List<Frequency> GetFrequencies()
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    return context.Frequency.ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        public List<AccountType> GetAccountTypes()
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    return context.AccountTypes.ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
