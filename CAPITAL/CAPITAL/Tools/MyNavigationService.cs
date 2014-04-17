using System;
using System.Windows.Navigation;
using CAPITAL.Messages;
using Microsoft.Phone.Controls;

namespace CAPITAL.Tools
{
    public sealed class MyNavigationService 
    {
        private NavigationMessage navMessage;

        public MyNavigationService(NavigationService navServiceParam)
        {
            NavService = navServiceParam;
            MainFrame.Navigated += new NavigatedEventHandler(MainFrame_Navigated);
        }

        void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (navMessage != null)
            {
                if (!navMessage.OnStack)
                    RemoveBackEntry();
            }
            navMessage = null;
        }

        public PhoneApplicationFrame MainFrame
        {
            get
            {
                return ((PhoneApplicationFrame)App.Current.RootVisual);
            }
        }

        private NavigationService navService;
        public NavigationService NavService
        {
            get
            {
                return navService;
            }
            set
            {
                navService = value;
            }
        }

        public void Navigate(NavigationMessage navParamMessage)
        {
            navMessage = navParamMessage;

            string destination = navMessage.Destination;
            string source = navMessage.Source;
            NavService.Navigate(new Uri(string.Format("/Views/{0}View.xaml", destination), UriKind.Relative));
        }

        public void GoBack()
        {
            if (NavService.CanGoBack)
            {
                NavService.GoBack();
            }
            else
            {
                Navigate(new NavigationMessage { OnStack = false, Destination = "Main", Source = "Redirect" });
            }
        }

        public void RemoveBackEntry()
        {
            if (NavService.CanGoBack)
            {
                NavService.RemoveBackEntry();
            }
        }
    }
}
