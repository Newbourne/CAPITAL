using System;
using GalaSoft.MvvmLight.Messaging;
using CAPITAL.CapitalService;

namespace CAPITAL.Messages
{
    public class LogoutMessage
    {

    }

    public class GetUserMessage : NotificationMessageAction<User>
    {
        public GetUserMessage(object sender, Action<User> callback)
            : base(sender, "GetUserMessage", callback)
        {

        }
    }

    public class SetUserMessage
    {
        public User User { get; set; }
    }
}
