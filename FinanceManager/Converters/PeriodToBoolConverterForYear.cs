using System;
using System.Globalization;
using System.Windows.Data;
using FinanceManager.Core;

namespace FinanceManager.Converters
{
    class PeriodToBoolConverterForYear : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Period)value == Period.Year) return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
