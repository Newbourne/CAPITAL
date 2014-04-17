using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using System.Linq;
using CAPITAL.Engine.Transaction;
using CAPITAL.ORM;
using CAPITAL.ORM.Exceptions;
using CAPITAL.ORM.Objects;
using CAPITAL.Engine.Utilities;
using CAPITAL.Engine.Interfaces;

namespace CAPITAL.Engine.Managers
{
    public class AccountManagement : Base, IAccountManagement
    {
        public IEnumerable<Account> GetAccounts(User source)
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    // Compare Accounts for that UserID
                    List<Account> accounts = context.Accounts.Where(x => x.UserId == source.UserId).OrderBy(x => x.AccountName).ToList();

                    foreach (Account acc in accounts)
                    {
                        //acc.Balance = acc.Balance - (context.Statements.Where(x => x.AccountId == acc.AccountId && x.IsPaid == true).AsEnumerable().Sum(x => x.PaidAmount));

                        acc.IsShare = false;
                        acc.PermissionLevel = 0;
                    }

                    // LINK SHARED ACCOUNTS
                    var shares = context.Share.Where(x => x.UserId == source.UserId);
                    foreach(Share share in shares)
                    {
                        Account account = context.Accounts.FirstOrDefault(x => x.AccountId == share.AccountId);

                        if (account != null)
                        {
                            account.IsShare = true;
                            account.PermissionLevel = share.PermissionLevelId;

                            accounts.Add(account);
                        }
                    }

                    return accounts;
                }
            }
            catch (Exception ex)
            {
                LogError(source, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public void CreateAccount(Account account)
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    account.CreationDate = DateTime.Now;
                    // Clear these objects to prevent duplicate entry.
                    account.AccountType = null;
                    account.Frequency = null;

                    ValidationContext valContext = new ValidationContext(this, null, null);
                    var errors = account.Validate(valContext);

                    if (errors.Count() == 0)
                    {
                        if (context.Accounts.FirstOrDefault(x => x.AccountName == account.AccountName) != null)
                            throw new ModelException("An account by that name already exists! Please use a different name.");
                        context.Accounts.Add(account);
                        context.SaveChanges();
                    }
                    else
                        throw new ModelException(errors);
                }
            }
            catch (ModelException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LogError(account, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void UpdateAccount(Account account)
        {
            using (CapitalContext context = new CapitalContext())
            {
                try
                {
                    ValidationContext valContext = new ValidationContext(this, null, null);
                    var errors = account.Validate(valContext);
                    if (errors.Count() > 0)
                        throw new ModelException(errors);


                    // Get Account
                    Account acc = context.Accounts.Where(x => x.AccountId == account.AccountId).FirstOrDefault();

                    // Update
                    if (acc != null)
                    {
                        acc.Balance = account.Balance;
                        acc.AccountName = account.AccountName;
                        acc.FrequencyId = account.FrequencyId;
                        acc.StartDate = account.StartDate;
                        acc.DefaultPayment = account.DefaultPayment;

                        // Update latest statement est. Payment amount.

                        Statement statement = context.Statements.Where(x => x.AccountId == acc.AccountId && x.IsPaid == false).FirstOrDefault();

                        if (statement != null)
                            statement.Balance = acc.DefaultPayment;

                        // Save
                        context.SaveChanges();
                    }
                }
                catch (ModelException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    LogError(account, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
            }

        }

        public void DeleteAccount(Account account)
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    var target = context.Accounts.Where(x => x.AccountId == account.AccountId).FirstOrDefault();
                    if (target != null)
                    {
                        context.Accounts.Remove(target);
                        context.SaveChanges();
                    }
                    else
                        throw new ModelException("That account does not exists.");
                }
            }
            catch (ModelException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LogError(account, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void ShareAccount(Share share)
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    // Check for shared user.
                    User user = context.Users.FirstOrDefault(x => x.UserId == share.UserId);
                    if (user == null)
                        throw new ModelException("That shared user does not exists.");

                    // Does this user already have this account?
                    Account account = context.Accounts.FirstOrDefault(x => x.UserId == share.UserId);
                    if (account != null)
                        throw new ModelException("Shared user is the owner of this account.");

                    // Is this already shared with the target?
                    Share existingShare = context.Share.FirstOrDefault(x => x.AccountId == share.AccountId && x.ShareId == user.UserId);
                    if (existingShare != null)
                        throw new ModelException("Account is already shared with this user.");

                    ValidationContext valContext = new ValidationContext(this, null, null);
                    var errors = share.Validate(valContext);
                    if (errors.Count() > 0)
                        throw new ModelException(errors);
                    else
                    {
                        context.Share.Add(share);
                        context.SaveChanges();
                    }
                }
            }
            catch (ModelException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LogError(new object[] {share}, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public IEnumerable<Statement> GetStatements(User user)
        {
            try
            {
                using (var transaction = TransactionUtils.CreateTransactionScope())
                {
                    using (CapitalContext context = new CapitalContext())
                    {
                        // Age Accounts
                        AgeAccounts(user, context);

                        // Get Statements
                        List<Statement> statementCollection = new List<Statement>();
                        var statements = context.Statements.Where(x => x.IsPaid == false && x.Account.UserId == user.UserId).AsEnumerable().OrderBy(x => x.DueDate);

                        foreach (Statement statement in statements)
                        {
                            statement.AccountName = statement.Account.AccountName;
                            statement.Account = null;
                            statementCollection.Add(statement);
                        }

                        var recentStatements = context.Statements.Where(x => x.IsPaid == true && x.Account.UserId == user.UserId && SqlFunctions.DateDiff("DAY", x.PaidDate, DateTime.Now).Value <= 30).Include(x => x.Account).AsEnumerable();
                        foreach (Statement statement in recentStatements)
                        {
                            statement.AccountName = statement.Account.AccountName;
                            statement.Account = null;
                            statementCollection.Add(statement);
                        }

                        transaction.Complete();
                        return statementCollection;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(user, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public Statement GetStatementById(User user, int statementId)
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    Statement statement = context.Statements.Include(x => x.Account).Where(x => x.Account.UserId == user.UserId && x.StatementId == statementId).FirstOrDefault();

                    Statement result = statement;
                    result.AccountName = statement.Account.AccountName;
                    statement.Account = null;
                    return result;
                }
            }
            catch(Exception ex)
            {
                LogError(new object[] { user, statementId }, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }

        public void UpdateStatement(Statement statement)
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    Statement target = context.Statements.Where(x => x.StatementId == statement.StatementId).FirstOrDefault();

                    if (target != null)
                    {
                        target.Balance = statement.Balance;
                        target.PaidAmount = statement.PaidAmount;
                        target.DueDate = statement.DueDate;
                        if (statement.IsPaid)
                        {
                            target.IsPaid = statement.IsPaid;
                            target.PaidDate = DateTime.Now;
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(statement, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void AgeAccounts(User user, CapitalContext context)
        {
            try
            {
                IEnumerable<Account> accounts = context.Accounts.Where(x => x.UserId == user.UserId).Include(x => x.Frequency).AsEnumerable();

                foreach (Account acc in accounts)
                {
                    int? count = context.Statements.Where(x => x.Account.AccountId == acc.AccountId).Count();

                    if (count == 0)
                    {
                        Statement statement = new Statement();
                        statement.AccountId = acc.AccountId;
                        statement.Balance = acc.DefaultPayment;
                        statement.IsPaid = false;
                        statement.CreationDate = DateTime.Now;
                        statement.PaidAmount = 0;
                        statement.PaidDate = null;

                        // int freqDays = context.Frequency.Where(x => x.FrequencyId == context.Accounts.Where(y => y.AccountId == acc.AccountId).FirstOrDefault().FrequencyId).FirstOrDefault().Days;
                        statement.DueDate = acc.StartDate;
                        context.Statements.Add(statement);
                    }
                    else
                    {
                        int id = context.Statements.Where(x => x.Account.UserId == user.UserId).Max(x => x.StatementId);

                        Statement latestStatement = context.Statements.Where(x => x.StatementId == id).FirstOrDefault();

                        TimeSpan difference = DateTime.Now - (DateTime)latestStatement.DueDate;

                        // Make sure the existing unPaid statement doesn't fall within the 7 day period

                        //if (difference.Days >= (acc.Frequency.Days - 7) && difference.Days > 7)
                        if (latestStatement.IsPaid)
                        {
                            // Get Average of previous payments
                            decimal sumPayments = (decimal)context.Statements.Where(x => x.AccountId == acc.AccountId && x.IsPaid == true).AsEnumerable().Sum(x => x.PaidAmount);
                            int totalPayments = (int)context.Statements.Where(x => x.AccountId == acc.AccountId && x.IsPaid == true).Count();


                            Statement statement = new Statement();
                            statement.AccountId = acc.AccountId;
                            //statement.Balance = acc.Payment;
                            if (totalPayments != 0)
                                statement.Balance = (double)Math.Round(sumPayments / totalPayments, 2);
                            else
                                statement.Balance = latestStatement.Balance;
                            statement.IsPaid = false;
                            statement.CreationDate = DateTime.Now;
                            statement.PaidAmount = 0;
                            statement.PaidDate = null;

                            Frequency freq = context.Accounts.Where(x => x.AccountId == acc.AccountId).FirstOrDefault().Frequency;

                            int freqDays = context.Accounts.Where(x => x.AccountId == acc.AccountId).FirstOrDefault().Frequency.Days;
                            statement.DueDate = DateTime.Now.AddDays(freqDays);
                            context.Statements.Add(statement);

                            MessengerUtil messenger = new MessengerUtil();
                            messenger.SendToast(user, string.Format("New {0} Statement!", statement.Account.AccountName), "");
                        }
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogError(user, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                throw new Exception("Error Aging Account", ex);
            }
        }
    }
}
