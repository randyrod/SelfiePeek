using System;
using Windows.UI.Xaml.Data;

namespace SelfiePeek.Utilities.Converters
{
    /// <summary>
    /// Value Converter from Bool to UI Visibility for Visibility Binding
    /// </summary>
    class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool val = (bool)value;
            if (val)
            {
                return Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                return Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
