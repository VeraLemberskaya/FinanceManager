using System;
using System.Globalization;
using System.Windows.Data;
using FinanceManager.Core;

namespace FinanceManager.Converters
{
    class OperationTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((OperationType)value == OperationType.Expenses) return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == false) return OperationType.Income;
            return OperationType.Expenses;
        }
    }
}
