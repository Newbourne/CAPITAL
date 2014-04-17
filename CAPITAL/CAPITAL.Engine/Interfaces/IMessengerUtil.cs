using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAPITAL.ORM.Objects;

namespace CAPITAL.Engine.Interfaces
{
    public interface IMessengerUtil
    {
        void Register(Registration registration);

        void SendTile(string name);

        void SendEmail(string fromUser, string subject, string message);

        void SendToast(User user, string title, string subTitle);
    }
}
