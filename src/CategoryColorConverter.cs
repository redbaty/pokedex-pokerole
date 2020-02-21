using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Pokedex.Abstractions;

namespace Pokedex.Pokerole
{
    public class PokemonTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PokemonLocal pokemonLocal)
            {
                return $"{pokemonLocal.number} - {pokemonLocal.name}";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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