using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FitnessApp.Converters
{
    public class BoolToTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string text)
            {
                var parts = text.Split('|');
                return boolValue ? parts[0] : parts[1];
            }
            return value?.ToString() ?? string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}