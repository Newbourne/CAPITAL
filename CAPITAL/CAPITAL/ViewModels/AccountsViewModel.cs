using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using CAPITAL.CapitalService;
using CAPITAL.Messages;
using CAPITAL.Tools;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CAPITAL.ViewModels
{
    public class AccountsViewModel : MyViewModelBase 
    {
        public ObservableCollection<Account> accounts;
        public ObservableCollection<Account> Accounts
        {
            get
            {
                return accounts;
            }
            set
            {
                accounts = value;
                RaisePropertyChanged("Accounts");
            }
        }

        private bool isEmpty;
        public bool IsEmpty
        {
            get
            {
                return isEmpty;
            }
            set
            {
                isEmpty = value;
                RaisePropertyChanged("IsEmpty");
            }
        }

        public AccountsViewModel()
        {
            Messenger.Default.Register<AccountLoadMessage>(this, s => LoadAccounts(s));
            base.Title = "Accounts";
        }

        private RelayCommand<object> accountSelectionCommand;
        public RelayCommand<object> AccountSelectionCommand
        {
            get
            {
                if (accountSelectionCommand == null)
                    accountSelectionCommand = new RelayCommand<object>(x => AccountSelected(x));
                return accountSelectionCommand;
            }
        }

        private void AccountSelected(object param)
        {
            if (param != null)
            {
                ((Account)param).IsNew = false;
                Messenger.Default.Send<AccountEditMessage>(new AccountEditMessage { Account = (Account)param });
                Messenger.Default.Send<NavigationMessage>(new NavigationMessage { Destination = "AccountDetails", Source = this.GetType().ToString(), OnStack = true });
            }
        }

        private void LoadAccounts(AccountLoadMessage message)
        {
            CapitalServiceClient client = new CapitalServiceClient();
            try
            {
                IsBusy = true;
                client.GetAccountsCompleted += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        if (e.Result != null)
                        {
                            Accounts = e.Result;
                            if (Accounts.Count < 1)
                                IsEmpty = true;
                            else
                                IsEmpty = false;
                        }
                        else
                        {
                            IsEmpty = true;
                        }
                    }
                    else if (e.Error is FaultException<CapitalError>)
                    {
                        ErrorMessages.FaultError(e.Error.Message);
                    }
                    else
                    {
                        ErrorMessages.UnexpectedError();
                    }
                    IsBusy = false;
                };
                client.GetAccountsAsync(GetUser());
            }
            catch
            {
                client.Abort();
            }
            finally
            {
                client.CloseAsync();
            }
        }
    }
}
