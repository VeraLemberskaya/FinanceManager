using System;
using System.Collections.Generic;
using FinanceManager.Model;

namespace FinanceManager.Services
{
    public enum MonthName
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December,
    }
    class DateTimeService
    {
        #region GetDateTime

        public static List<MonthName> Months = new List<MonthName>()
        {MonthName.December, MonthName.January,MonthName.February,MonthName.March,MonthName.April,MonthName.May,MonthName.June,
        MonthName.July, MonthName.August,MonthName.September,MonthName.October,MonthName.November};
        public static List<DateTime> Today()
        {
            return Day(DateTime.Today);
        }
        public static List<DateTime> Day(DateTime Date)
        {
            List<DateTime> date = new();
            date.Add(Date);
            return date;
        }
        public static List<DateTime> Week()
        {
            DateTime date = DateTime.Today;
            return Week(ref date);
        }
        public static List<DateTime> Week(ref DateTime Date)
        {
            List<DateTime> date = new();
            DateTime day = Date;
            DayOfWeek d = day.DayOfWeek;
            DateTime startDate = day.DayOfWeek switch
            {
                DayOfWeek.Monday => day,
                DayOfWeek.Tuesday => day.AddDays(-1),
                DayOfWeek.Wednesday => day.AddDays(-2),
                DayOfWeek.Thursday => day.AddDays(-3),
                DayOfWeek.Friday => day.AddDays(-4),
                DayOfWeek.Saturday => day.AddDays(-5),
                DayOfWeek.Sunday => day.AddDays(-6)
            };
            for (int i = 0; i < 7; i++)
            {
                date.Add(startDate.AddDays(i));
            }
            Date = startDate.AddDays(7);
            return date;
        }
        public static List<DateTime> Month()
        {
            return Month(DateTime.Today);
        }
        public static List<DateTime> Month(DateTime Date)
        {
            List<DateTime> date = new();
            DateTime day = Date;
            DateTime start = day.AddDays(-day.Day + 1);
            for (int i = 0; i < DateTime.DaysInMonth(day.Year, day.Month); i++)
            {
                date.Add(start.AddDays(i));
            }
            return date;
        }

        public static List<DateTime> Year()
        {
            return Year(DateTime.Today);
        }

        public static List<DateTime> Year(DateTime Date)
        {
            List<DateTime> date = new();
            DateTime FirstDay = new DateTime(Date.Year, 1, 1);
            int days;
            if (DateTime.IsLeapYear(Date.Year)) days = 366;
            else days = 365;
            for (int i = 0; i < days; i++)
            {
                date.Add(FirstDay.AddDays(i));
            }
            return date;
        }

        public static List<Week> GetWeeksOfTheYear(int Year)
        {
            List<Week> Weeks = new List<Week>();
            DateTime DayOfTheYear = new DateTime(Year, 1, 1);
            do
            {
                Week week = new Week(Week(ref DayOfTheYear));
                Weeks.Add(week);
            } while (DayOfTheYear.Year == Year);
            return Weeks;
        }

        public static MonthName GetMonthName(int Month)
        {
            return Month switch
            {
                1 => MonthName.January,
                2 => MonthName.February,
                3 => MonthName.March,
                4 => MonthName.April,
                5 => MonthName.May,
                6 => MonthName.June,
                7 => MonthName.July,
                8 => MonthName.August,
                9 => MonthName.September,
                10 => MonthName.October,
                11 => MonthName.November,
                12 => MonthName.December
            };
        }

        #endregion
    }
}
