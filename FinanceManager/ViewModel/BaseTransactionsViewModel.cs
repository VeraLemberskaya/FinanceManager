using System;
using System.Collections.Generic;
using FinanceManager.Core;
using FinanceManager.Model;
using FinanceManager.Services;
using System.Collections.ObjectModel;

namespace FinanceManager.ViewModel
{
    internal class BaseTransactionsViewModel : BaseVM
    {
        Service service = Service.GetInstance();
        public RelayCommand UpDateCurrency { get; set; }
        public RelayCommand ShowDayTransactions { get; set; }
        public RelayCommand ShowWeekTransactions { get; set; }
        public RelayCommand ShowMonthTransactions { get; set; }
        public RelayCommand ShowYearTransactions { get; set; }
        public List<Currency> AccountsCurrency
        {
            get
            {
                return service.GetAccountsCurrency();
            }
        }

        public WeeksOfTheYearViewModel Weeks { get; set; }
        public List<MonthName> MonthsOfTheYear { get; set; } = DateTimeService.Months;

        private Currency _selectedCurrency;
        private TransactionsViewModel _currentVM;
        private DateTime _selectedDate;
        private MonthName _selectedMonth;
        private Period _selectedPeriod;

        private OperationType Type;
        public BaseTransactionsViewModel(OperationType type)
        {
            Type = type;

            Weeks = new WeeksOfTheYearViewModel();

            _selectedCurrency = service.DefaultCurrency;
            _selectedPeriod = Period.Week;
            _selectedDate = DateTime.Today;
            _selectedMonth = DateTimeService.GetMonthName(_selectedDate.Month);

            CurrentVM = new TransactionsViewModel(DateTimeService.Week(), SelectedPeriod, Type, SelectedCurrency);
            CurrentVM.SaveObject += SaveChange;
            #region Commands
            UpDateCurrency = new RelayCommand(obj =>
            {
                UpDate();
            });
            ShowDayTransactions = new RelayCommand(obj =>
            {
                SelectedPeriod = Period.Day;
                CurrentVM = new TransactionsViewModel(DateTimeService.Day(SelectedDate), SelectedPeriod, Type , SelectedCurrency);
                CurrentVM.SaveObject += SaveChange;
            });
            ShowWeekTransactions = new RelayCommand(obj =>
            {
                SelectedPeriod = Period.Week;
                Week week = Weeks.SelectedWeek;
                CurrentVM = new TransactionsViewModel( week.DaysOfWeek, SelectedPeriod, Type, SelectedCurrency);
                CurrentVM.SaveObject += SaveChange;
            });
            ShowMonthTransactions = new RelayCommand(obj =>
            {
                SelectedPeriod = Period.Month;
                int month = (int)SelectedMonth + 1;
                DateTime date = new DateTime(DateTime.Today.Year, month, 1);
                CurrentVM = new TransactionsViewModel(DateTimeService.Month(date), SelectedPeriod,Type, SelectedCurrency);
                CurrentVM.SaveObject += SaveChange;
            });
            ShowYearTransactions = new RelayCommand(obj =>
            {
                SelectedPeriod = Period.Year;
                DateTime date = DateTime.Today;
                CurrentVM = new TransactionsViewModel(DateTimeService.Year(), SelectedPeriod, Type, SelectedCurrency);
                CurrentVM.SaveObject += SaveChange;
            });
            #endregion
        }
        #region Properties
        public TransactionsViewModel CurrentVM
        {
            get => _currentVM;
            set
            {
                _currentVM = value;
                OnPropertyChanged();
            }
        }
        public MonthName SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                OnPropertyChanged();
            }
        }
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                _selectedCurrency = value;
                OnPropertyChanged(nameof(TotalBalance));
                OnPropertyChanged();
            }
        }
        public Period SelectedPeriod
        {
            get => _selectedPeriod;
            set
            {
                _selectedPeriod = value;
                OnPropertyChanged();
            }
        }
        public float TotalBalance
        {
            get => service.GetTotalBalance(SelectedCurrency);
        }
        #endregion
        private void SaveChange(object sender, SaveObjectChangesEventArgs e)
        {
            OnPropertyChanged(nameof(TotalBalance));
        }
        public void UpDate()
        {
            switch (_selectedPeriod)
            {
                case Period.Day:
                    {
                        ShowDayTransactions.Execute(null);
                    }
                    break;
                case Period.Week:
                    {
                        ShowWeekTransactions.Execute(null);
                    }
                    break;
                case Period.Month:
                    {
                        ShowMonthTransactions.Execute(null);
                    }
                    break;
                case Period.Year:
                    {
                        ShowYearTransactions.Execute(null);
                    }
                    break;
            }
        }
    }
}
