using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FitnessApp.Converters
{
    public class BoolToPaidConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "✓ Paid" : "Pay Subscription ($20)";
            }
            return "Pay Subscription ($20)";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}