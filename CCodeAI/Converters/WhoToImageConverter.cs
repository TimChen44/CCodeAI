using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CCodeAI.ToolWindows
{
    public class WhoToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            var who = value?.ToString();
            if (who == "User")
            {
                bitmap.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("CCodeAI.ToolWindows.user.png");
            }
            else if (who == "Assistant")
            {
                bitmap.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("CCodeAI.ToolWindows.bot.png");
            }
            else
            {
                bitmap.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream("CCodeAI.ToolWindows.bot.png");
            }
            bitmap.EndInit();
            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}