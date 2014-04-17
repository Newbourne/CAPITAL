using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CAPITAL.Engine.Interfaces;
using CAPITAL.ORM.Objects;
using CAPITAL.ORM.Exceptions;

namespace CAPITAL.WCF
{
    [ServiceContract]
    public interface ICapitalService: IUserManagement, IAccountManagement, IFeedbackUtil, IMessengerUtil, IAccountUtil
    {
        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new User CreateUser(User user);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new User Login(User user);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new IEnumerable<Account> GetAccounts(User source);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new void CreateAccount(Account account);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new void UpdateAccount(Account account);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new void DeleteAccount(Account account);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new void ShareAccount(Share share);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new IEnumerable<Statement> GetStatements(User user);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new Statement GetStatementById(User user, int id);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new void UpdateStatement(Statement statement);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new List<Frequency> GetFrequencies();

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new List<AccountType> GetAccountTypes();

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new void SendFeedback(Feedback userFeedback);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new void Register(Registration registration);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new void SendTile(string name);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new void SendEmail(string fromUser, string subject, string message);

        [OperationContract]
        [FaultContract(typeof(CapitalError))]
        new void SendToast(User user, string title, string subTitle);
    }
}
