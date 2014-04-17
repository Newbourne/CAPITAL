using System;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Controls;
using CAPITAL.CapitalService;
using CAPITAL.Messages;
using System.ServiceModel;
using CAPITAL.Tools;

namespace CAPITAL.ViewModels
{
    public class StatementViewerViewModel : MyViewModelBase
    {
        private Statement editStatement;
        public Statement EditStatement
        {
            get
            {
                return editStatement;
            }
            set
            {
                editStatement = value;
                RaisePropertyChanged("EditStatement");
            }
        }

        private RelayCommand statementSaveCommand;
        public RelayCommand StatementSaveCommand
        {
            get
            {
                if (statementSaveCommand == null)
                    statementSaveCommand = new RelayCommand(SaveStatement);
                return statementSaveCommand;
            }
        }

        private RelayCommand pinCommand;
        public RelayCommand PinCommand
        {
            get
            {
                if (pinCommand == null)
                    pinCommand = new RelayCommand(PinStatementFromCommand);
                return pinCommand;
            }
        }

        public StatementViewerViewModel()
        {
            base.Title = "Statement";
            Messenger.Default.Register<StatementEditMessage>(this, x => Init(x.Statement));
        }

        private void Init(Statement statement)
        {
            EditStatement = statement;
            EditStatement.PaidDate = DateTime.Now;
            EditStatement.PaidAmount = EditStatement.Balance;
            UpdateStatus();
        }

        public DateTime? DueDate
        {
            get
            {
                return EditStatement.DueDate;
            }
            set
            {
                EditStatement.DueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        public DateTime? PaidDate
        {
            get
            {
                return EditStatement.PaidDate;
            }
            set
            {
                EditStatement.PaidDate = value;
                RaisePropertyChanged("PaidDate");
            }
        }

        public double Balance
        {
            get
            {
                return EditStatement.Balance;
            }
            set
            {
                EditStatement.Balance = value;
                RaisePropertyChanged("Balance");
            }
        }

        public double PaidAmount
        {
            get
            {
                return EditStatement.PaidAmount;
            }
            set
            {
                EditStatement.PaidAmount = value;
                RaisePropertyChanged("PaidAmount");
            }
        }

        public bool IsPaid
        {
            get
            {
                return editStatement.IsPaid;
            }
            set
            {
                editStatement.IsPaid = value;
                UpdateStatus();
                RaisePropertyChanged("IsPaid");
            }
        }

        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                RaisePropertyChanged("Status");
            }
        }

        private void UpdateStatus()
        {
            if (editStatement.IsPaid)
            {
                Status = "Paid";
            }
            else
            {
                Status = "Not Paid";
            }
        }

        private void SaveStatement()
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

            CapitalServiceClient client = new CapitalServiceClient();
            try
            {
                IsBusy = true;
                client.UpdateStatementCompleted += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        // Check for tile and remove
                        string navParam = string.Format("Statement={0}", EditStatement.StatementId);
                        ShellTile TileToFind = ShellTile.ActiveTiles.Where(x => x.NavigationUri.ToString().Contains(navParam)).FirstOrDefault();
                        if (TileToFind != null)
                        {
                            if (EditStatement.IsPaid)
                                TileToFind.Delete();
                            else
                            {
                                // Update old tile
                                PinStatementFromCommand();
                            }
                        }
                        UpdateViews();
                    }
                    else if (e.Error is FaultException<CapitalError>)
                    {
                        ErrorMessages.FaultError(e.Error.Message);
                    }
                    else
                    {
                        ErrorMessages.UnexpectedError();
                    }

                    IsBusy = false;
                };
                client.UpdateStatementAsync(editStatement);
            }
            catch
            {
                client.Abort();
            }
            finally
            {
                client.CloseAsync();
            }
        }

        private void UpdateViews()
        {
            // Refresh Accounts and Statements
            Messenger.Default.Send(new AccountLoadMessage());
            Messenger.Default.Send(new StatementLoadMessage());
            Messenger.Default.Send(new StatementLoadRecentMessage());

            // Navigate to Home/Back
            Messenger.Default.Send(new NavigationMessage { Destination = "Home", OnStack = false, Source = this.GetType().ToString() });
        }

        private void PinStatementFromCommand()
        {
            Messenger.Default.Send(new StatementTileMessage { Statement = editStatement });
        }
    }
}