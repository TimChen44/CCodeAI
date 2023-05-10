using CCodeAI.Models;
using System.Globalization;
using System.Windows.Data;

namespace CCodeAI.ToolWindows;

public class WhoToStringConverter : IValueConverter
{
    public object Convert(
        object value, 
        Type targetType,
        object parameter, 
        CultureInfo culture)
    {
        if (value is EWho who)
        {
            return who switch
            {
                EWho.User => Environment.UserName,
                EWho.PlugIn or EWho.Assistant or EWho.Welcome => Resources.Resources.Assistant,
                _ => value.ToString(),
            };
        }

        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}