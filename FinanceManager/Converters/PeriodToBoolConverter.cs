using System;
using System.Globalization;
using System.Windows.Data;
using FinanceManager.Core;

namespace FinanceManager.Converters
{
    class PeriodToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Period)value == Period.Day) return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
