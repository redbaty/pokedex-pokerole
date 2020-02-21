using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Pokedex.Pokerole
{
    public class CategoryColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Green":
                    return new SolidColorBrush(Colors.DarkGreen);
                case "Poison":
                    return new SolidColorBrush(Colors.GreenYellow);
                case "Fire":
                    return new SolidColorBrush(Colors.Red);
            }

            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}