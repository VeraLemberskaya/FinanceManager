using System;
using System.Globalization;
using System.Windows.Data;
using FinanceManager.Core;
using MaterialDesignThemes.Wpf;

namespace FinanceManager.Converters
{
    internal class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == false) return "HotPink";
            return "LightBlue";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
