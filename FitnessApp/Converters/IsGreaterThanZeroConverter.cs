using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FitnessApp.Converters
{
    public class IsGreaterThanZeroConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int count)
                return count > 0;
            if (value is double dcount)
                return dcount > 0;
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}