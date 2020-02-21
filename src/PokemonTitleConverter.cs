using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Pokedex.Abstractions;

namespace Pokedex.Pokerole
{
    public class VisibleIfNotNull : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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
}