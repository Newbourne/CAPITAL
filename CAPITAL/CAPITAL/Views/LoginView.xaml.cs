using System;
using CAPITAL.ViewModels;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CAPITAL.Views
{
    public partial class LoginView : PhoneApplicationPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void LoginBarButton_Click(object sender, EventArgs e)
        {
            object focusObj = FocusManager.GetFocusedElement();
            if (focusObj != null && focusObj is TextBox)
            {
                var binding = (focusObj as TextBox).GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
            } 

            LoginViewModel dc = (this.DataContext as LoginViewModel);
            if(dc.LoginCommand.CanExecute(sender))
                dc.LoginCommand.Execute(sender);
        }

        private void NewUserBarButton_Click(object sender, EventArgs e)
        {
            object focusObj = FocusManager.GetFocusedElement();
            if (focusObj != null && focusObj is TextBox)
            {
                var binding = (focusObj as TextBox).GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
            } 

            LoginViewModel dc = (this.DataContext as LoginViewModel);
            if (dc.NewUserCommand.CanExecute(sender))
                dc.NewUserCommand.Execute(sender);
        }
    }
}