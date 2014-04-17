using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAPITAL.ORM.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAPITAL.ORM.Exceptions;
using CAPITAL.Engine.Managers;

namespace CAPITAL.Engine.Test.Tests
{
    [TestClass]
    public class AccountManagementTest
    {
        [TestMethod]
        public void CreateValidAccount()
        {
            UserManagement userManagement = new UserManagement();
            User user = userManagement.Login(new User { Email = "brian@email.com", Password = "password" });

            if (user == null)
                Assert.Fail("No user for new account.");

            Account newAccount = new Account();
            newAccount.AccountName = "TEST 1";
            newAccount.CreationDate = DateTime.Now;
            newAccount.StartDate = DateTime.Now;
            newAccount.DefaultPayment = 200.00;
            newAccount.FrequencyId = 2;
            newAccount.UserId = user.UserId;
            newAccount.AccountTypeId = 1;

            AccountManagement accountManagement = new AccountManagement();
            accountManagement.CreateAccount(newAccount);
        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void CreateInvalidAccount()
        {
            UserManagement userManagement = new UserManagement();
            User user = userManagement.Login(new User { Email = "brian@email.com", Password = "password" });

            if (user == null)
                Assert.Fail("No user for new account.");

            Account newAccount = new Account();

            AccountManagement accountManagement = new AccountManagement();
            accountManagement.CreateAccount(newAccount);
        }

        [TestMethod]
        public void GetAccountsForValidUser()
        {
            AccountManagement accManage = new AccountManagement();
            UserManagement userManage = new UserManagement();
            User user = userManage.Login(new User{ Email = "brian@email.com", Password = "password"});
            var accounts = accManage.GetAccounts(user);
            if (accounts.Count() < 1)
                Assert.Fail("No accounts returned for valid user.");
        }

        [TestMethod]
        public void GetAccountsForInvalidUser()
        {
            AccountManagement accManage = new AccountManagement();
            var accounts = accManage.GetAccounts(new User());
            if (accounts.Count() > 0)
                Assert.Fail("Accounts returned for invalid user.");
        }

        [TestMethod]
        public void UpdateValidAccount()
        {
            UserManagement userManage = new UserManagement();
            User user = userManage.Login(new User { Email = "brian@email.com", Password = "password" });

            AccountManagement accManage = new AccountManagement();
            Account[] accounts = accManage.GetAccounts(user).ToArray();

            if (accounts == null || accounts.Count() < 1)
                Assert.Fail("No account to update.");

            Account target = accounts[0];

            target.AccountName = "New Account Name";
            target.Balance = 1000000;
            target.DefaultPayment = 50;

            accManage.UpdateAccount(target);
        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void UpdateInvalidAccount()
        {
            UserManagement userManage = new UserManagement();
            User user = userManage.Login(new User { Email = "brian@email.com", Password = "password" });

            AccountManagement accManage = new AccountManagement();
            Account[] accounts = accManage.GetAccounts(user).ToArray();

            if (accounts == null || accounts.Count() < 1)
                Assert.Fail("No account to update.");

            Account target = accounts[0];

            target.AccountName = "";

            accManage.UpdateAccount(target);
        }

        [TestMethod]
        public void DeleteValidAccount()
        {
            UserManagement userManage = new UserManagement();
            User user = userManage.Login(new User { Email = "brian@email.com", Password = "password" });

            AccountManagement accManage = new AccountManagement();
            Account[] accounts = accManage.GetAccounts(user).ToArray();

            if (accounts == null || accounts.Count() < 1)
                Assert.Fail("No account to delete.");

            accManage.DeleteAccount(accounts[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void DeleteInvalidAccount()
        {
            AccountManagement accManage = new AccountManagement();
            accManage.DeleteAccount(new Account { AccountId = 0 });
        }

        [TestMethod]
        public void ShareValidAccount()
        {
            UserManagement userManage = new UserManagement();
            AccountManagement accManage = new AccountManagement();

            // Owner            
            User Owner = userManage.CreateUser(new User
            {
                Email = "owner@email.com",
                Password = "password",
                CreationDate = DateTime.Now,
                LastAccessDate = DateTime.Now
            });

            // Owner Account
            accManage.CreateAccount(new Account
            {
                AccountName = "ShareAccountTest",
                CreationDate = DateTime.Now,
                StartDate = DateTime.Now,
                FrequencyId = 3,
                AccountTypeId = 1,
                UserId = Owner.UserId
            });

            Account[] accounts = accManage.GetAccounts(Owner).ToArray();

            // Share user
            User shareUser = userManage.CreateUser(new User
            {
                Email = "share@email.com",
                Password = "password",
                CreationDate = DateTime.Now,
                LastAccessDate = DateTime.Now
            });

            // Share
            int accountID = accounts[0].AccountId;
            Share share = new Share();
            share.UserId = shareUser.UserId;
            share.AccountId = accounts[0].AccountId;
            share.PermissionLevelId = 2;

            accManage.ShareAccount(share);

            // Confirm
            accounts = accManage.GetAccounts(shareUser).ToArray();

            Account account = accounts.FirstOrDefault(x => x.AccountId == accountID);

            if (account == null)
                Assert.Fail("Shared account was not in Shared User collection.");

            if (!(account.IsShare))
                Assert.Fail("Shared account is not flagged as shared.");

            if (account.PermissionLevel < 1)
                Assert.Fail("Shared account permission level is invalid.");
        }

        [TestMethod]
        public void GetStatementsCreateFirstStatement()
        {
            UserManagement userManage = new UserManagement();
            User user = userManage.Login(new User { Email = "owner@email.com", Password = "password" });

            AccountManagement accManage = new AccountManagement();

            Statement[] statements = accManage.GetStatements(user).ToArray();

            if (statements.Count() == 0)
                Assert.Fail("No Statement was created.");
        }

        [TestMethod]
        public void UpdateStatement()
        {
            UserManagement userManage = new UserManagement();
            User user = userManage.Login(new User { Email = "owner@email.com", Password = "password" });

            AccountManagement accManage = new AccountManagement();

            Statement[] statements = accManage.GetStatements(user).ToArray();

            Statement statement = statements[0];
            statement.IsPaid = true;

            accManage.UpdateStatement(statement);
        }

        [TestMethod]
        public void GetStatementsCreateSecondStatement()
        {
            UserManagement userManage = new UserManagement();
            User user = userManage.Login(new User { Email = "owner@email.com", Password = "password" });

            AccountManagement accManage = new AccountManagement();
            Statement[] statements = accManage.GetStatements(user).ToArray();

            if (statements.Count() == 0)
                Assert.Fail("No Statement was created.");
        }
    }
}