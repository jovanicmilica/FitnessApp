using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace FitnessApp.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string colors)
            {
                var parts = colors.Split('|');
                return boolValue ? new SolidColorBrush(Color.Parse(parts[0])) : new SolidColorBrush(Color.Parse(parts[1]));
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}