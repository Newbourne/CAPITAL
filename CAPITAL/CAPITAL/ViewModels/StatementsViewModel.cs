using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using CAPITAL.CapitalService;
using CAPITAL.Messages;
using System.ServiceModel;
using CAPITAL.Tools;

namespace CAPITAL.ViewModels
{
    public class StatementsViewModel : MyViewModelBase
    {
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

        private IEnumerable<Statement> primaryStatements;
        public IEnumerable<Statement> PrimaryStatements
        {
            get
            {
                return primaryStatements;
            }
            set
            {
                primaryStatements = value;
            }
        }

        private ObservableCollection<StatementSelectionItem> statementFilter;
        public ObservableCollection<StatementSelectionItem> StatementFilter 
        {
            get
            {
                return statementFilter;
            }
            set
            {
                statementFilter = value;
                RaisePropertyChanged("StatementFilter");
            }
        }

        private StatementSelectionItem selectedFilter;
        public StatementSelectionItem SelectedFilter
        {
            get
            {
                return selectedFilter;
            }
            set
            {
                selectedFilter = value;
                RaisePropertyChanged("SelectedFilter");
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

        public StatementsViewModel()
        {
            base.Title = "Statements";
            Messenger.Default.Register<StatementLoadMessage>(this, e => LoadStatements(e));
            StatementFilter = new ObservableCollection<StatementSelectionItem>();
            StatementFilter.Add(new StatementSelectionItem { Name = "Week", Value = 7 });
            StatementFilter.Add(new StatementSelectionItem { Name = "Month", Value = 30 });
            StatementFilter.Add(new StatementSelectionItem { Name = "All", Value = 0 });
            SelectedFilter = StatementFilter[2];
        }

        private void LoadStatements(StatementLoadMessage msg)
        {
            CapitalServiceClient client = new CapitalServiceClient();
            try
            {
                IsBusy = true;

                client.GetStatementsCompleted += (s, e) =>
                    {
                        IsBusy = false;
                        if (e.Error == null)
                        {
                            // Remove Paid Statements
                            Messenger.Default.Send(new StatementLoadRecentMessage { Statements = e.Result.Where(x => x.IsPaid == true) });
                            PrimaryStatements = new ObservableCollection<Statement>(e.Result.Where(x => x.IsPaid == false));
                            StatementFilterRefresh(SelectedFilter);
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
                client.GetStatementsAsync(GetUser());
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

        private RelayCommand<object> statementSelectionCommand;
        public RelayCommand<object> StatementSelectionCommand
        {
            get
            {
                if (statementSelectionCommand == null)
                    statementSelectionCommand = new RelayCommand<object>(x => StatementSelected(x));
                return statementSelectionCommand;
            }
        }

        private RelayCommand<object> statementTileCommand;
        public RelayCommand<object> StatementTileCommand
        {
            get
            {
                if (statementTileCommand == null)
                    statementTileCommand = new RelayCommand<object>(x => CreateStatementTile(x));
                return statementTileCommand;
            }
        }

        private RelayCommand<object> statementFilterSelectionCommand;
        public RelayCommand<object> StatementFilterSelectionCommand
        {
            get
            {
                if (statementFilterSelectionCommand == null)
                    statementFilterSelectionCommand = new RelayCommand<object>(x => StatementFilterRefresh(x));
                return statementFilterSelectionCommand;
            }
        }

        private void StatementSelected(object param)
        {
            if (param != null)
            {
                Statement byValue = new Statement();
                byValue = (param as Statement);

                Messenger.Default.Send<StatementEditMessage>(new StatementEditMessage { Statement = byValue });
                Messenger.Default.Send<NavigationMessage>(new NavigationMessage { Destination = "StatementDetails", Source = this.GetType().ToString(), OnStack = true });
            }
        }

        private void CreateStatementTile(object param)
        {
            try
            {
                Statement svcParam = (param as Statement);
                Messenger.Default.Send(new StatementTileMessage { Statement = svcParam });
            }
            catch
            {

            }
        }

        private void StatementFilterRefresh(object param)
        {
            if (param != null)
            {
                // Look a the filter
                // Set Statements to the collection of the filter
                StatementSelectionItem selection = param as StatementSelectionItem;
                if (PrimaryStatements == null)
                {
                    IsEmpty = true;
                    StatementTotal = 0;
                    return;
                }


                if (selection.Value == 0)
                {
                    Statements = PrimaryStatements as ObservableCollection<Statement>;
                }
                else
                {
                    Statements = new ObservableCollection<Statement>();
                    int difference;
                    foreach (Statement statement in PrimaryStatements)
                    {
                        difference = ((DateTime)statement.DueDate - DateTime.Now).Days;
                        if (difference <= selection.Value)
                        {
                            Statements.Add(statement);
                        }
                    }
                    //Statements = PrimaryStatements.Where(x => (((DateTime)x.DueDate - DateTime.Now).Days <= selection.Value)).AsEnumerable() as ObservableCollection<Statement>;
                }
                if(Statements == null)
                {
                    StatementTotal = 0;
                    IsEmpty = true;
                }
                else
                {
                    if (Statements.Count > 0)
                    {
                        StatementTotal = Statements.Sum(x => x.Balance);
                        IsEmpty = false;
                    }
                    else
                    {
                        StatementTotal = 0;
                        IsEmpty = true;
                    }
                }
            }
        }

        private double statementTotal;
        public double StatementTotal
        {
            get
            {
                return statementTotal;
            }
            set
            {
                statementTotal = value;
                RaisePropertyChanged("StatementTotal");
            }
        }
    }

    public class StatementSelectionItem
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }
}
