using CAPITAL.CapitalService;

namespace CAPITAL.Messages
{
    public class AccountTileMessage
    {
        public Account Account { get; set; }
    }

    public class StatementTileMessage
    {
        public Statement Statement { get; set; }
    }
}
