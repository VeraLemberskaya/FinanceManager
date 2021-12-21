using System;
using System.Collections.Generic;
using System.Linq;
using FinanceManager.Services;

namespace FinanceManager.Model
{
    public class Week
    {
        public List<DateTime> DaysOfWeek { get; private set; }
        public Week(List<DateTime> dateList)
        {
            DaysOfWeek = dateList;
        }
        #region Properties
        public DateTime FirstDay
        {
            get => DaysOfWeek.First();
        }
        public DateTime LastDay
        {
            get => DaysOfWeek.Last();
        }
        public MonthName Month
        {
            get
            {
                return DateTimeService.GetMonthName(LastDay.Month);
            }
        }
        #endregion
        public override bool Equals(object obj)
        {
            if (obj is Week)
                return this.FirstDay.Equals((obj as Week).FirstDay);
            return false;
        }
        public override string ToString()
        {
            return FirstDay.ToString("dd.MM") + "-" + LastDay.ToString("dd.MM");
        }
    }
}
