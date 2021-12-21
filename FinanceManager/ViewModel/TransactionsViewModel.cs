using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FinanceManager.Core;
using LiveCharts;
using FinanceManager.Services;
using FinanceManager.Model;

namespace FinanceManager.ViewModel
{
     class TransactionsViewModel:BaseVM,ISaveObjectChanges
    {
        Service service = Service.GetInstance();

        public delegate void SaveObjectChangesHandler(object sender, SaveObjectChangesEventArgs e);
        public event ISaveObjectChanges.SaveObjectChangesHandler SaveObject;

        public RelayCommand EditTransaction { get; set; }
        public RelayCommand AddTransaction { get; set; }
        public OperationType Type { get; set; }
        public Period Period { get; set; }
        public string ErrorText { get; set; }
        public string DateText { get; set; }
        public List<DateTime> PeriodDate { get; set; }
        public ObservableCollection<TransactionViewModel> Transactions
        {
            get
            {
                switch (Period)
                {
                    case Period.Day:
                        {
                            return new ObservableCollection<TransactionViewModel>(ServiceConverter.ConvertToTransactionVM(Service.DayTransactions(service.GetTransactions(Type),_currency, PeriodDate.First())));
                        }
                        break;
                    case Period.Week:
                        {
                            return new ObservableCollection<TransactionViewModel>(ServiceConverter.ConvertToTransactionVM(Service.WeekTransactions(service.GetTransactions(Type), _currency, PeriodDate)));
                        }
                        break;
                    case Period.Month:
                        {
                            return new ObservableCollection<TransactionViewModel>(ServiceConverter.ConvertToTransactionVM(Service.MonthTransactions(service.GetTransactions(Type), _currency, PeriodDate.First().Month)));
                        }
                        break;
                    case Period.Year:
                        {
                            return new ObservableCollection<TransactionViewModel>(ServiceConverter.ConvertToTransactionVM(Service.YearTransactions(service.GetTransactions(Type), _currency, DateTime.Today.Year)));
                        }
                        break;
                }
                return null;
            }
        }

        private Currency _currency;
        private TransactionViewModel _selectedTransaction;
        private object _currentVM;

        public TransactionsViewModel(List<DateTime> periodDate, Period period,OperationType Type, Currency currency)
        { 
            this.PeriodDate = periodDate;
            this.Period = period;
            this.Type= Type;
            this._currency = currency;

            SetText();

            #region Commands
            EditTransaction = new RelayCommand(obj =>
            {
                CurrentVM = null;
                if (SelectedTransaction!= null)
                {
                    CurrentVM = new TransactionViewModel(new Transaction(SelectedTransaction.Date, service.GetAccount(SelectedTransaction.Account), SelectedTransaction.Category, SelectedTransaction.Comment, SelectedTransaction.Money, SelectedTransaction.Type));
                    (CurrentVM as TransactionViewModel).SaveObject += Edittransaction;
                    (CurrentVM as TransactionViewModel).DeleteObject += DeleteTransaction;
                }
            });

            AddTransaction = new RelayCommand(obj =>
              {
                  CurrentVM = new TransactionViewModel(Type,currency);
                  (CurrentVM as TransactionViewModel).SaveObject += Addtransaction;
              });
            #endregion
        }
        #region Properties
        public SeriesCollection TransactionsSeries
        {
           get=> ChartsService.CreateSeriesCollection(Service.GetTransactionsCategories(Transactions), _currency);
        }
        public object CurrentVM
        {
            get => _currentVM;
            set
            {
                _currentVM = value;
                OnPropertyChanged();
            }
        }
        public TransactionViewModel SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                _selectedTransaction = value;
                OnPropertyChanged();
            }
        }

        public bool IsEmpty
        {
            get
            {
                if (Transactions.Count == 0) return true;
                else return false;
            }
        }

        public bool NoAccounts
        {
            get
            {
                if (service.Accounts.Count == 0) return false;
                return true;
            }
        }
        #endregion
        #region Methods
        private void Addtransaction(object sender, SaveObjectChangesEventArgs e)
        {
            TransactionViewModel newTransaction = e.Object as TransactionViewModel;

            service.AddTransaction(newTransaction.Transaction);
            service.AddTransactionChangeBalance(newTransaction.Transaction);

            OnPropertyChanged(nameof(Transactions));
            OnPropertyChanged(nameof(TransactionsSeries));
            SaveObjectExecute(this);
            OnPropertyChanged(nameof(IsEmpty));
            CurrentVM = null;
        }
        private void Edittransaction(object sender, SaveObjectChangesEventArgs e)
        {
                TransactionViewModel newTransaction = e.Object as TransactionViewModel;

                 service.EditTransactionChangeBalance(SelectedTransaction.Transaction, newTransaction.Transaction);
                 service.EditTransaction(SelectedTransaction.Transaction, newTransaction.Transaction);

                
                OnPropertyChanged(nameof(Transactions));
                OnPropertyChanged(nameof(TransactionsSeries));
                SaveObjectExecute(this);
                CurrentVM = null;
            
        }
        private void DeleteTransaction(object sender, SaveObjectChangesEventArgs e)
        {
            service.DeleteTransaction(SelectedTransaction.Transaction);
            OnPropertyChanged(nameof(Transactions));
            OnPropertyChanged(nameof(TransactionsSeries));
            OnPropertyChanged(nameof(IsEmpty));
            SaveObjectExecute(this);
            CurrentVM = null;
        }
        private void SetText()
        {
            if (service.Accounts.Count == 0) ErrorText = "To perform a transaction \nyou need to have \nat least one account.";
            else
            {
                if (Type == OperationType.Income)
                    ErrorText = Period switch
                    {
                        Period.Day => "You haven't got\nany income this day.",
                        Period.Week => "You haven't got\nany income this week.",
                        Period.Month => "You haven't got\nany income this month.",
                        Period.Year => "You haven't got\nany income this year."
                    };
                else
                    ErrorText = Period switch
                    {
                        Period.Day => "You haven't got\nany expenses this day.",
                        Period.Week => "You haven't got\nany expenses this week.",
                        Period.Month => "You haven't got\nany expenses this month.",
                        Period.Year => "You haven't got\nany expenses this year."
                    };
            }

            DateText = Period switch
            {
                Period.Day => $"{PeriodDate.First().ToString("dd.MM.yyyy")}",
                Period.Week => $"{PeriodDate.First().ToString("dd.MM")}-{PeriodDate.Last().ToString("dd.MM")}",
                Period.Month => $"{DateTimeService.GetMonthName(PeriodDate.First().Month)} {PeriodDate.First().Year}",
                Period.Year => $"{PeriodDate.First().Year}"
            };
        }
        #endregion
        public void SaveObjectExecute(object obj)
        {
            SaveObject?.Invoke(this, new SaveObjectChangesEventArgs(obj as TransactionsViewModel));
        }
    }
}
