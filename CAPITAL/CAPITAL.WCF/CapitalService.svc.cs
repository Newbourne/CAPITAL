using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CAPITAL.ORM.Objects;
using CAPITAL.Engine.Managers;
using CAPITAL.Engine.Utilities;
using CAPITAL.ORM.Exceptions;

namespace CAPITAL.WCF
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single)]
    public class CapitalService : ICapitalService
    {
        #region User Management
        public User CreateUser(User user)
        {
            try
            {
                UserManagement userManagement = new UserManagement();
                return userManagement.CreateUser(user);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public User Login(User user)
        {
            try
            {
                UserManagement userManagement = new UserManagement();
                return userManagement.Login(user);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Account Management
        public IEnumerable<Account> GetAccounts(User source)
        {
            try
            {
                AccountManagement accountManagement = new AccountManagement();
                return accountManagement.GetAccounts(source);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void CreateAccount(Account account)
        {
            try
            {
                AccountManagement accountManagement = new AccountManagement();
                accountManagement.CreateAccount(account);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return;
            }
        }

        public void UpdateAccount(Account account)
        {
            try
            {
                AccountManagement accountManagement = new AccountManagement();
                accountManagement.UpdateAccount(account);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return;
            }
        }

        public void DeleteAccount(Account account)
        {
            try
            {
                AccountManagement accountManagement = new AccountManagement();
                accountManagement.DeleteAccount(account);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return;
            }
        }

        public void ShareAccount(Share share)
        {
            try
            {
                AccountManagement accountManagement = new AccountManagement();
                accountManagement.ShareAccount(share);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return;
            }
        }

        public IEnumerable<Statement> GetStatements(User user)
        {
            try
            {
                AccountManagement accountManagement = new AccountManagement();
                return accountManagement.GetStatements(user);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Statement GetStatementById(User user, int id)
        {
            try
            {
                AccountManagement accountManagement = new AccountManagement();
                return accountManagement.GetStatementById(user, id);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void UpdateStatement(Statement statement)
        {
            try
            {
                AccountManagement accountManagement = new AccountManagement();
                accountManagement.UpdateStatement(statement);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion

        #region Utilities
        public List<Frequency> GetFrequencies()
        {
            try
            {
                AccountUtil utility = new AccountUtil();
                return utility.GetFrequencies();
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<AccountType> GetAccountTypes()
        {
            try
            {
                AccountUtil utility = new AccountUtil();
                return utility.GetAccountTypes();
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SendFeedback(Feedback userFeedback)
        {
            try
            {
                FeedbackUtil utility = new FeedbackUtil();
                utility.SendFeedback(userFeedback);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return;
            }
        }

        public void Register(Registration registration)
        {
            try
            {
                MessengerUtil messenger = new MessengerUtil();
                messenger.Register(registration);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return;
            }
        }

        public void SendTile(string name)
        {
            try
            {
                MessengerUtil messenger = new MessengerUtil();
                messenger.SendTile(name);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return;
            }
        }

        public void SendEmail(string fromUser, string subject, string message)
        {
            try
            {
                MessengerUtil messenger = new MessengerUtil();
                messenger.SendEmail(fromUser, subject, message);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return;
            }
        }

        public void SendToast(User user, string title, string subTitle)
        {
            try
            {
                MessengerUtil messenger = new MessengerUtil();
                messenger.SendToast(user, title, subTitle);
            }
            catch (ModelException ex)
            {
                CapitalError error = new CapitalError(ex);
                throw new FaultException<CapitalError>(error, error.Message);
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion
    }
}
