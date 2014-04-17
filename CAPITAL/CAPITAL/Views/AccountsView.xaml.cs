using System.Windows.Controls;
using CAPITAL.ViewModels;

namespace CAPITAL.Views
{
    public partial class AccountsView : UserControl
    {
        public AccountsView()
        {
            InitializeComponent();
            AccountListBox.SelectionChanged += new SelectionChangedEventHandler(AccountListBox_SelectionChanged);
        }

        void AccountListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccountsViewModel dc = (this.DataContext as AccountsViewModel);
            if (dc.AccountSelectionCommand.CanExecute((sender as ListBox).SelectedItem))
                dc.AccountSelectionCommand.Execute((sender as ListBox).SelectedItem);

            AccountListBox.SelectedIndex = -1;
        }
    }
}
