using System;
using System.Collections.Generic;
using CAPITAL.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CAPITAL.Views
{
    public partial class AccountDetailsView : PhoneApplicationPage
    {
        public AccountDetailsView()
        {
            InitializeComponent();

            if ((DataContext as AccountDetailsViewModel).IsNew)
            {
                createButton.Visibility = System.Windows.Visibility.Visible;
                deleteButton.Visibility = System.Windows.Visibility.Collapsed;
                saveButton.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                createButton.Visibility = System.Windows.Visibility.Collapsed;
                deleteButton.Visibility = System.Windows.Visibility.Visible;
                saveButton.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}