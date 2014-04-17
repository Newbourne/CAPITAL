using CAPITAL.ViewModels;

namespace CAPITAL.ViewModels
{
    public sealed class ViewModelLocator
    {
        public ViewModelLocator()
        {
            // The hell with IoC. Let's just create all out VM's when application loads.
            MainView = new MainViewModel();
            LoginView = new LoginViewModel();
            NewUserView = new NewUserViewModel();
            HomeView = new HomeViewModel();
            AccountsView = new AccountsViewModel();
            AccountViewer = new AccountDetailsViewModel();
            StatementsView = new StatementsViewModel();
            StatementViewer = new StatementViewerViewModel();
            RecentActivityView = new RecentActivityViewModel();
            FeedbackView = new FeedbackViewModel();
            AboutView = new AboutViewModel();
            AccountNew = new AccountNewViewModel();
        }

        private MainViewModel mainView;
        public MainViewModel MainView
        {
            get
            {
                return mainView;
            }
            private set
            {
                mainView = value;
            }
        }

        private LoginViewModel loginView;
        public LoginViewModel LoginView
        {
            get
            {
                return loginView;
            }
            private set
            {
                loginView = value;
            }
        }

        private NewUserViewModel newUserView;
        public NewUserViewModel NewUserView
        {
            get
            {
                return newUserView;
            }
            private set
            {
                newUserView = value;
            }
        }

        private HomeViewModel homeView;
        public HomeViewModel HomeView
        {
            get
            {
                return homeView;
            }
            private set
            {
                homeView = value;
            }
        }

        private AccountsViewModel accountsView;
        public AccountsViewModel AccountsView
        {
            get
            {
                return accountsView;
            }
            private set
            {
                accountsView = value;
            }
        }

        private AccountNewViewModel accountNew;
        public AccountNewViewModel AccountNew
        {
            get
            {
                return accountNew;
            }
            private set
            {
                accountNew = value;
            }
        }

        private AccountDetailsViewModel accountViewer;
        public AccountDetailsViewModel AccountViewer
        {
            get
            {
                return accountViewer;
            }
            private set
            {
                accountViewer = value;
            }
        }

        private StatementsViewModel statementView;
        public StatementsViewModel StatementsView
        {
            get
            {
                return statementView;
            }
            private set
            {
                statementView = value;
            }
        }

        private StatementViewerViewModel statementViewer;
        public StatementViewerViewModel StatementViewer
        {
            get
            {
                return statementViewer;
            }
            private set
            {
                statementViewer = value;
            }
        }

        private RecentActivityViewModel recentActivityView;
        public RecentActivityViewModel RecentActivityView
        {
            get
            {
                return recentActivityView;
            }
            private set
            {
                recentActivityView = value;
            }
        }

        private FeedbackViewModel feedbackView;
        public FeedbackViewModel FeedbackView
        {
            get
            {
                return feedbackView;
            }
            private set
            {
                feedbackView = value;
            }
        }

        private AboutViewModel aboutView;
        public AboutViewModel AboutView
        {
            get
            {
                return aboutView;
            }
            set
            {
                aboutView = value;
            }
        }
    }
}
