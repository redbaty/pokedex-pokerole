using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Newtonsoft.Json;
using PokeAPI;
using Pokedex.Abstractions;
using Pokedex.Pokerole.Extensions;
using PropertyChanged;

namespace Pokedex.Pokerole
{
    public class TextBoxBehavior
    {
        public static bool GetSelectAllTextOnFocus(TextBox textBox)
        {
            return (bool)textBox.GetValue(SelectAllTextOnFocusProperty);
        }

        public static void SetSelectAllTextOnFocus(TextBox textBox, bool value)
        {
            textBox.SetValue(SelectAllTextOnFocusProperty, value);
        }

        public static readonly DependencyProperty SelectAllTextOnFocusProperty =
            DependencyProperty.RegisterAttached(
                "SelectAllTextOnFocus",
                typeof(bool),
                typeof(TextBoxBehavior),
                new UIPropertyMetadata(false, OnSelectAllTextOnFocusChanged));

        private static void OnSelectAllTextOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox == null) return;

            if (e.NewValue is bool == false) return;

            if ((bool)e.NewValue)
            {
                textBox.GotFocus += SelectAll;
                textBox.PreviewMouseDown += IgnoreMouseButton;
            }
            else
            {
                textBox.GotFocus -= SelectAll;
                textBox.PreviewMouseDown -= IgnoreMouseButton;
            }
        }

        private static void SelectAll(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox == null) return;
            textBox.SelectAll();
        }

        private static void IgnoreMouseButton(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null || (!textBox.IsReadOnly && textBox.IsKeyboardFocusWithin)) return;

            e.Handled = true;
            textBox.Focus();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel
    {
        public Dispatcher Dispatcher { get; }
        private PokemonLocal _selectedPokemon;

        private CancellationTokenSource CancellationTokenSource { get; set; }

        public List<PokemonLocal> Pokemons { get; set; }


        public PokemonLocal SelectedPokemon
        {
            get => _selectedPokemon;
            set
            {
                _selectedPokemon = value;
                PokemonImage = null;
                CancellationTokenSource?.Cancel();

                if (value?.number != null)
                {
                    CancellationTokenSource = new CancellationTokenSource();
                    DataFetcher.GetApiObject<Pokemon>(value.number.Value).ContinueWith(
                        t => { Dispatcher.Invoke(() => { PokemonImage = new Uri(t.Result.Sprites.FrontMale); }); },
                        CancellationTokenSource.Token);
                }
            }
        }

        public Uri PokemonImage { get; set; }

        public MainWindowViewModel(Dispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;
            Pokemons = LoadPokeData(Assembly.GetExecutingAssembly().GetFileStream("pokemon.json"));
        }

        private static List<PokemonLocal> LoadPokeData(Stream fileStream) //todo :  nao adicionar pokemons mega
        {
            using var r = new StreamReader(fileStream);
            var json = r.ReadToEnd();

            var pkmns = JsonConvert.DeserializeObject<List<PokemonLocal>>(json);
            //ate aqui ok preenchemos nosso array com a lista do jasao e os argonautas
            for (var i = 0; i < pkmns.Count; i++)
            {
                if (pkmns[i].name.Contains("("))
                {
                    pkmns.Remove(pkmns[i]);
                    i--;
                }

                ;
            }

            return pkmns;
        }
    }
}