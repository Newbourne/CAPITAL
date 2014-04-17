using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using CAPITAL.CapitalService;
using CAPITAL.Messages;

namespace CAPITAL.ViewModels
{
    public class MyViewModelBase : ViewModelBase
    {
        #region Privates
        private bool isBusy;
        private bool isVisible;
        private string applicationName;
        private string title;
        private string busyText = "";
        #endregion

        #region Constructor
        public MyViewModelBase()
        {
    
        }
        #endregion
        
        #region Properties
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    RaisePropertyChanged("IsBusy");
                }
            }
        }
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    RaisePropertyChanged("IsVisible");
                }
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        public string BusyText
        {
            get { return busyText; }
            set
            {
                if (busyText != value)
                {
                    busyText = value;
                    RaisePropertyChanged("BusyText");
                }
            }
        }

        public string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                if (applicationName != value)
                {
                    applicationName = value;
                    RaisePropertyChanged("ApplicationName");
                }
            }
        }
        #endregion

        #region Methods
        public User GetUser()
        {
            User msgUser = new User();

            var msg = new GetUserMessage(this, message =>
            {
                msgUser = message;
            });

            Messenger.Default.Send(msg);

            return msgUser;
        }
        #endregion
    }
}
