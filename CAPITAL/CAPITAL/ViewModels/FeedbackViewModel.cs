using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using CAPITAL.CapitalService;
using CAPITAL.Messages;

namespace CAPITAL.ViewModels
{
    public class FeedbackViewModel : MyViewModelBase
    {
        public FeedbackViewModel()
        {
            FeedbackItems = new List<string>();
            FeedbackItems.Add("Report Bug");
            FeedbackItems.Add("Suggestion");
            FeedbackItems.Add("Other");
        }

        public List<string> FeedbackItems { get; set; }

        private int selectedFeedbackIndex = 0;
        public int SelectedFeedbackIndex
        {
            get
            {
                return selectedFeedbackIndex;
            }
            set
            {
                selectedFeedbackIndex = value;
                RaisePropertyChanged("SelectedFeedbackIndex");
            }
        }

        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                RaisePropertyChanged("Message");
            }
        }

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

        private RelayCommand sendFeedbackCommand;
        public RelayCommand SendFeedbackCommand
        {
            get
            {
                if (sendFeedbackCommand == null)
                    sendFeedbackCommand = new RelayCommand(SendFeedback);
                return sendFeedbackCommand;
            }
        }

        private void Loaded()
        {
            SelectedFeedbackIndex = 0;
            Message = string.Empty;
        }

        private void SendFeedback()
        {
            try
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


                IsBusy = true;
                CapitalServiceClient client = new CapitalServiceClient();
                client.SendFeedbackCompleted += (s, e) => 
                {
                    IsBusy = false;
                    if (e.Error != null)
                        MessageBox.Show("Error sending feedback.", "Error", MessageBoxButton.OK);
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Thank You!");

                        Messenger.Default.Send(new NavigateBackMessage());
                    }
                };

                client.SendFeedbackAsync(new Feedback { 
                        Message = this.Message, 
                        ReportType = FeedbackItems[SelectedFeedbackIndex], 
                        UserId = GetUser().UserId });
            }
            catch
            {

            }
        }
    }
}
