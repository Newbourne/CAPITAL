using System;
using System.Windows.Data;
using Microsoft.Phone.Shell;
using System.Globalization;

namespace CAPITAL.Tools.Converters
{
    public class ApplicationBarModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? ApplicationBarMode.Default : ApplicationBarMode.Minimized;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ApplicationBarMode)value) == ApplicationBarMode.Default;
        }
    }
}
