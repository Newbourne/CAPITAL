using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.IO.IsolatedStorage;
using CAPITAL.Messages;
using CAPITAL.CapitalService;
using CAPITAL.Tools;

namespace CAPITAL.ViewModels
{
    public class HomeViewModel : MyViewModelBase
    {
        public HomeViewModel()
        {

        }

        private bool InitLoad = false;

        private RelayCommand loadedCommand;
        public RelayCommand LoadedCommand
        {
            get
            {
                if (loadedCommand == null)
                    loadedCommand = new RelayCommand(Loaded);
                return loadedCommand;
            }
        }

        private RelayCommand newAccountCommand;
        public RelayCommand NewAccountCommand
        {
            get
            {
                if (newAccountCommand == null)
                    newAccountCommand = new RelayCommand(NewAccount);
                return newAccountCommand;
            }
        }

        private RelayCommand refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                    refreshCommand = new RelayCommand(Refresh);
                return refreshCommand;
            }
        }

        private RelayCommand logoutCommand;
        public RelayCommand LogoutCommand
        {
            get
            {
                if (logoutCommand == null)
                    logoutCommand = new RelayCommand(Logout);
                return logoutCommand;
            }
        }

        private RelayCommand feedbackCommand;
        public RelayCommand FeedbackCommand
        {
            get
            {
                if (feedbackCommand == null)
                    feedbackCommand = new RelayCommand(Feedback);
                return feedbackCommand;
            }
        }

        private void Loaded()
        {
            if (!InitLoad)
            {
                InitLoad = true;
                Messenger.Default.Send(new AccountLoadMessage());
                Messenger.Default.Send(new StatementLoadMessage());
                //Messenger.Default.Send(new StatementLoadRecentMessage());
            }
        }

        private void NewAccount()
        {
            Messenger.Default.Send(new AccountNewMessage());
            Messenger.Default.Send<NavigationMessage>(new NavigationMessage { Destination = "AccountNew", Source = this.GetType().ToString(), OnStack = true });
        }

        private void Refresh()
        {
            try
            {
                Messenger.Default.Send(new AccountLoadMessage());
                Messenger.Default.Send(new StatementLoadMessage());
                Messenger.Default.Send(new StatementLoadRecentMessage());
            }
            catch
            {

            }
        }

        private void Logout()
        {
            try
            {
                Messenger.Default.Send(new LogoutMessage());
            }
            catch
            {

            }
        }

        private void Feedback()
        {
            try
            {
                Messenger.Default.Send(new NavigationMessage { Destination = "Feedback", OnStack = true, Source = this.GetType().ToString() });
            }
            catch
            {

            }
        }
    }
}