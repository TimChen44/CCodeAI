using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace CCodeAI.Converters;

public class StringIsNullOrEmptyToVisibilityConveter : IValueConverter
{
    public object Convert(
        object value, 
        Type targetType, 
        object parameter, 
        CultureInfo culture)
    {
        return  string.IsNullOrEmpty(value?.ToString()) ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(
        object value, 
        Type targetType, 
        object parameter, 
        CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}