using System.Windows.Controls;
using CAPITAL.ViewModels;
using Microsoft.Phone.Controls;

namespace CAPITAL.Views
{
    public partial class StatementsView : UserControl
    {
        public StatementsView()
        {
            InitializeComponent();
            StatementListBox.SelectionChanged += new SelectionChangedEventHandler(StatementListBox_SelectionChanged);
            StatementFilterListBox.SelectionChanged += new SelectionChangedEventHandler(StatementFilterListBox_SelectionChanged);
        }

        void StatementFilterListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatementsViewModel dc = (this.DataContext as StatementsViewModel);
            if (dc.StatementFilterSelectionCommand.CanExecute((sender as ListBox).SelectedItem))
                dc.StatementFilterSelectionCommand.Execute((sender as ListBox).SelectedItem);
        }

        void StatementListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatementsViewModel dc = (this.DataContext as StatementsViewModel);
            if (dc.StatementSelectionCommand.CanExecute((sender as ListBox).SelectedItem))
                dc.StatementSelectionCommand.Execute((sender as ListBox).SelectedItem);

            StatementListBox.SelectedIndex = -1;
        }

        //private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    StatementsViewModel dc = (this.DataContext as StatementsViewModel);
        //    if (dc.StatementTileCommand.CanExecute((sender as MenuItem).DataContext))
        //        dc.StatementTileCommand.Execute((sender as MenuItem).DataContext);
        //}
    }
}
