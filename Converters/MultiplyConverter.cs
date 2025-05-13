using System.Globalization;
using System.Windows.Data;

namespace TicTacToe.Converters
{
    public class MultiplyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double original && double.TryParse(parameter?.ToString(), out var factor))
            {
                return original * factor;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
