using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows;
using CAPITAL.CapitalService;
using CAPITAL.Messages;
using CAPITAL.Tools;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CAPITAL.ViewModels
{
    public class RecentActivityViewModel : MyViewModelBase
    {
        public RecentActivityViewModel()
        {
            base.Title = "Recent Activity";
            Messenger.Default.Register<StatementLoadRecentMessage>(this, msg => LoadRecentActivity(msg));
        }

        private ObservableCollection<Statement> statements;
        public ObservableCollection<Statement> Statements
        {
            get
            {
                return statements;
            }
            set
            {
                statements = value;
                RaisePropertyChanged("Statements");
            }
        }

        private bool isEmpty;
        public bool IsEmpty
        {
            get
            {
                return isEmpty;
            }
            set
            {
                isEmpty = value;
                RaisePropertyChanged("IsEmpty");
            }
        }

        private void LoadRecentActivity(StatementLoadRecentMessage msg)
        {
            if (msg.Statements != null)
            {
                Statements = new ObservableCollection<Statement>(msg.Statements);
            }
            else
            {
                Statements = null;
            }
        }
    }
}
