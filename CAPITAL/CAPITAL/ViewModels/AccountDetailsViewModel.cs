using System;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using CAPITAL.Messages;
using CAPITAL.CapitalService;
using CAPITAL.Tools;
using System.ServiceModel;
using System.Collections.ObjectModel;

namespace CAPITAL.ViewModels
{
    public class AccountDetailsViewModel : MyViewModelBase
    {
        public AccountDetailsViewModel()
        {
            base.Title = "Account";
            Messenger.Default.Register<AccountEditMessage>(this, x => Init(x.Account));
        }

        private Account editAccount;
        public Account EditAccount
        {
            get
            {
                return editAccount;
            }
            set
            {
                editAccount = value;
                RaisePropertyChanged("EditAccount");
            }
        }

        private ObservableCollection<Frequency> freqCollection;
        public ObservableCollection<Frequency> FreqCollection
        {
            get
            {
                return freqCollection;
            }
            set
            {
                freqCollection = value;
                RaisePropertyChanged("FreqCollection");
            }
        }

        public bool IsNew
        {
            get
            {
                return editAccount.IsNew;
            }
            set
            {
                editAccount.IsNew = value;
                RaisePropertyChanged("IsNew");
            }
        }

        public string AccountName
        {
            get
            {
                return EditAccount.AccountName;
            }
            set
            {
                if (string.IsNullOrEmpty(editAccount.AccountName))
                {
                    editAccount.AccountName = "";
                }
                else
                    editAccount.AccountName = value;
                RaisePropertyChanged("AccountName");
            }
        }

        private Frequency frequency;
        public Frequency Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                frequency = value;

                if (frequency != null)
                {
                    EditAccount.FrequencyId = frequency.FrequencyId;
                    RaisePropertyChanged("FreqCollection");
                    RaisePropertyChanged("Frequency");
                }
            }
        }

        public double Balance
        {
            get
            {
                return EditAccount.Balance;
            }
            set
            {
                EditAccount.Balance = (double)value;
                RaisePropertyChanged("Balance");
            }
        }

        public double DefaultPayment
        {
            get
            {
                return EditAccount.DefaultPayment;
            }
            set
            {
                EditAccount.DefaultPayment = (double)value;
                RaisePropertyChanged("DefaultPayment");
            }
        }

        public DateTime? StartDate
        {
            get
            {
                return EditAccount.StartDate;
            }
            set
            {
                EditAccount.StartDate = value;
                RaisePropertyChanged("StartDate");
            }
        }

        private void Init(Account account)
        {
            EditAccount = account;
            IsBusy = true;
            CapitalServiceClient client = new CapitalServiceClient();
            try
            {
                client.GetFrequenciesCompleted += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        FreqCollection = e.Result;
                    }
                    else
                        ErrorMessages.UnexpectedError();


                    Frequency = FreqCollection.Where(x => x.FrequencyId == editAccount.FrequencyId).FirstOrDefault();

                    if (Frequency == null)
                    {
                        Frequency = FreqCollection[0];
                    }

                    IsBusy = false;
                };
                client.GetFrequenciesAsync();
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

        private RelayCommand createAccountCommand;  
        public RelayCommand CreateAccountCommand
        {
            get
            {
                if (createAccountCommand == null)
                    createAccountCommand = new RelayCommand(CreateAccount);
                return createAccountCommand;
            }
        }

        private void CreateAccount()
        {
            object focusObj = FocusManager.GetFocusedElement();
            if (focusObj != null && focusObj is TextBox)
            {
                var binding = (focusObj as TextBox).GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
                (focusObj as TextBox).IsEnabled = false;
                (focusObj as TextBox).IsEnabled = true;
                return;
            } 

            if (!Validate())
                return;

            CapitalServiceClient client = new CapitalServiceClient();

            try
            {
                IsBusy = true;

                EditAccount.UserId = GetUser().UserId;

                client.CreateAccountCompleted += (s, e) =>
                    {
                        if (e.Error == null)
                        {
                            UpdateViews();
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
                client.CreateAccountAsync(EditAccount);
            }
            catch (Exception)
            {
                client.Abort();
            }
            finally
            {
                client.CloseAsync();
            }
        }

        private RelayCommand deleteAccountCommand;
        public RelayCommand DeleteAccountCommand
        {
            get
            {
                if (deleteAccountCommand == null)
                    deleteAccountCommand = new RelayCommand(DeleteAccount);
                return deleteAccountCommand;
            }
        }

        private void DeleteAccount()
        {
            MessageBoxResult result = MessageBox.Show("Are You Sure?", "Account Deletion", MessageBoxButton.OKCancel);

            if (result != MessageBoxResult.OK)
                return;

            CapitalServiceClient client = new CapitalServiceClient();

            try
            {
                IsBusy = true;
                client.DeleteAccountCompleted += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        EditAccount = null;
                        UpdateViews();
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
                client.DeleteAccountAsync(EditAccount);
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

        private RelayCommand saveAccountCommand;
        public RelayCommand SaveAccountCommand
        {
            get
            {
                if (saveAccountCommand == null)
                    saveAccountCommand = new RelayCommand(SaveAccount);
                return saveAccountCommand;
            }
        }

        private void SaveAccount()
        {
            object focusObj = FocusManager.GetFocusedElement();
            if (focusObj != null && focusObj is TextBox)
            {
                var binding = (focusObj as TextBox).GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
                (focusObj as TextBox).IsEnabled = false;
                (focusObj as TextBox).IsEnabled = true;
                return;
            }

            if (!Validate())
                return;

            CapitalServiceClient client = new CapitalServiceClient();

            try
            {
                IsBusy = true;

                client.UpdateAccountCompleted += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        UpdateViews();
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
                client.UpdateAccountAsync(EditAccount);
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

        private void UpdateViews()
        {
            // Refresh Accounts and Statements
            Messenger.Default.Send(new AccountLoadMessage());
            Messenger.Default.Send(new StatementLoadMessage());
            Messenger.Default.Send(new StatementLoadRecentMessage());

            // Navigate to Home/Back
            if (EditAccount != null)
                if (EditAccount.IsNew)
                    Messenger.Default.Send(new RemoveBackEntry());

            Messenger.Default.Send(new NavigateBackMessage());
        }

        private bool Validate()
        {
            string error = "";
            bool valid = true;

            if (string.IsNullOrEmpty(editAccount.AccountName) || string.IsNullOrWhiteSpace(editAccount.AccountName))
            {
                valid = false;
                error += "Account Name Required.\n";
            }

            if (!valid)
            {
                ErrorMessages.ValidationError(error);
            }

            return valid;
        }
    }
}
