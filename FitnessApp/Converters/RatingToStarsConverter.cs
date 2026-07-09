using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FitnessApp.Converters
{
    public class RatingToStarsConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double rating)
            {
                int fullStars = (int)Math.Floor(rating);
                bool hasHalf = rating - fullStars >= 0.5;
                
                string stars = new string('★', fullStars);
                if (hasHalf) stars += "½";
                stars += new string('☆', 5 - fullStars - (hasHalf ? 1 : 0));
                
                return stars;
            }
            return "☆☆☆☆☆";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}