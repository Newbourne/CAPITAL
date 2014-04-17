using CAPITAL.CapitalService;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CAPITAL.Messages
{
    public sealed class AccountLoadMessage { }

    public sealed class AccountEditMessage
    {
        public Account Account{ get; set; }
    }

    public sealed class AccountNewMessage
    {

    }

    public class StatementLoadMessage { }
    public class StatementEditMessage
    {
        public Statement Statement { get; set; }
    }

    public class StatementLoadRecentMessage {
        public IEnumerable<Statement> Statements { get; set; }
    }
}
