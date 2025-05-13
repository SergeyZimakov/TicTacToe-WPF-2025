using System.Globalization;
using System.Windows.Data;
using TicTacToe.Enums;

namespace TicTacToe.Converters
{
    public class SymbolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SymbolTypeEnum symbol) return symbol.GetAsString();
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
