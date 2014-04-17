using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAPITAL.ORM.Objects;

namespace CAPITAL.Engine.Interfaces
{
    public interface IUserManagement
    {
        User CreateUser(User user);

        User Login(User user);
    }
}
