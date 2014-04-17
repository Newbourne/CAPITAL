using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using CAPITAL.Messages;
using GalaSoft.MvvmLight.Command;
using CAPITAL.CapitalService;
using CAPITAL.Tools;
using System.ServiceModel;
using System.Collections.ObjectModel;

namespace CAPITAL.ViewModels
{
    public class AccountNewViewModel : MyViewModelBase
    {
        private ObservableCollection<AccountType> accountTypes;
        public ObservableCollection<AccountType> AccountTypes
        {
            get
            {
                return accountTypes;
            }
            private set
            {
                accountTypes = value;
                RaisePropertyChanged("AccountTypes");
            }
        }

        public AccountNewViewModel()
        {
            base.Title = "Select Account Type";
            Messenger.Default.Register<AccountNewMessage>(this, x => Init());
        }

        private void Init()
        {
            // Load Account Types
            CapitalServiceClient service = service = new CapitalServiceClient();
            try
            {
                IsBusy = true;

                service.GetAccountTypesCompleted += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        if (e.Result != null)
                        {
                            AccountTypes = e.Result;
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

                service.GetAccountTypesAsync();
            }
            catch (Exception)
            {
                service.Abort();
            }
            finally
            {
                service.CloseAsync();
            }
        }

        private RelayCommand<object> accountTypeSelectionCommand;
        public RelayCommand<object> AccountTypeSelectionCommand
        {
            get
            {
                if (accountTypeSelectionCommand == null)
                    accountTypeSelectionCommand = new RelayCommand<object>(x => AccountTypeSelected(x));
                return accountTypeSelectionCommand;
            }
        }

        private void AccountTypeSelected(object param)
        {
            if (param != null)
            {
                Account account = new Account();
                account.IsNew = true;
                account.StartDate = DateTime.Now;
                account.AccountType = ((AccountType)param);
                account.AccountTypeId = ((AccountType)param).AccountTypeId;

                Messenger.Default.Send<AccountEditMessage>(new AccountEditMessage { Account = account });
                Messenger.Default.Send<NavigationMessage>(new NavigationMessage { Destination = "AccountDetails", Source = this.GetType().ToString(), OnStack = true });
            }
        }
    }
}
