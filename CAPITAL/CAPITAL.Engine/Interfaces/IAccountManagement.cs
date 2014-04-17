using System.Collections.Generic;
using CAPITAL.ORM.Objects;

namespace CAPITAL.Engine.Interfaces
{
    public interface IAccountManagement
    {
        IEnumerable<Account> GetAccounts(User source);

        void CreateAccount(Account account);

        void UpdateAccount(Account account);

        void DeleteAccount(Account account);

        void ShareAccount(Share share);

        IEnumerable<Statement> GetStatements(User user);

        Statement GetStatementById(User user, int id);

        void UpdateStatement(Statement statement);
    }
}
