using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CAPITAL.ViewModels;

namespace CAPITAL.Views
{
    public partial class HomeView : PhoneApplicationPage
    {
        public HomeView()
        {
            InitializeComponent();
            appBarCreateAccountBtn.Visibility = System.Windows.Visibility.Collapsed;
            HomePanorama.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(HomePanorama_SelectionChanged);
        }

        void HomePanorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Here is where we need to manually change the application bar for the given view

            var item = e.AddedItems[0] as PanoramaItem;

            switch (item.Header.ToString().ToUpper())
            {
                case "STATEMENTS":
                    //bindingAppBar.Mode = ApplicationBarMode.Minimized;
                    appBarCreateAccountBtn.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case "ACCOUNTS":
                    //bindingAppBar.Mode = ApplicationBarMode.Default;
                    appBarCreateAccountBtn.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    //bindingAppBar.Mode = ApplicationBarMode.Minimized;
                    appBarCreateAccountBtn.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }
    }
}