using System;
using System.Collections.Generic;
using FinanceManager.Services;
using FinanceManager.Core;
using FinanceManager.Model;

namespace FinanceManager.ViewModel
{
    class WeeksOfTheYearViewModel : BaseVM
    {
        Service service = Service.GetInstance();
        public List<MonthName> Months { get; set; } = DateTimeService.Months;
        public List<Week> Weeks { get; set; }

        private int year;
        private MonthName _selectedMonth;
        private Week _selectedWeek;

        public WeeksOfTheYearViewModel()
        {
            year = DateTime.Today.Year;
            
            Weeks = DateTimeService.GetWeeksOfTheYear(year);

            _selectedMonth = DateTimeService.GetMonthName(DateTime.Today.Month);
            _selectedWeek = new Week(DateTimeService.Week());
        }
        public WeeksOfTheYearViewModel(DateTime date)
        {
            this.year = date.Year;
            Weeks = DateTimeService.GetWeeksOfTheYear(year);

            _selectedMonth = DateTimeService.GetMonthName(date.Month);
            _selectedWeek = new Week(DateTimeService.Week(ref date));
        }

        #region Properties
        public string Year
        {
            get => year.ToString();
        }
        public MonthName SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                SelectedWeek= Weeks.Find(o => o.Month.Equals(SelectedMonth));
                OnPropertyChanged();
            }
        }
        public Week SelectedWeek
        {
            get => _selectedWeek;
            set
            {
                _selectedWeek = value;
                OnPropertyChanged();
            }
        }
        #endregion

    }
}
