using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using Newtonsoft.Json;
using PokeAPI;
using Pokedex.Abstractions;
using Pokedex.Pokerole.Extensions;
using PropertyChanged;

namespace Pokedex.Pokerole
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel
    {
        public Dispatcher Dispatcher { get; }
        private PokemonLocal _selectedPokemon;
        private string _searchText;

        private CancellationTokenSource ImageCancellationTokenSource { get; set; }
        private CancellationTokenSource SearchCancellationTokenSource { get; set; }

        public List<PokemonLocal> Pokemons { get; set; }

        public ICollectionView FilteredPokemons { get; set; }

        public bool IsFiltering = false;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                if (string.IsNullOrEmpty(value))
                {
                    FilteredPokemons.Filter = null;
                }
                else
                {
                    FilteredPokemons.Filter = o =>
                        o is PokemonLocal pokemonLocal &&
                        pokemonLocal.name.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
                }
            }
        }


        public PokemonLocal SelectedPokemon
        {
            get => _selectedPokemon;
            set
            {
                _selectedPokemon = value;
                PokemonImage = null;
                ImageCancellationTokenSource?.Cancel();

                if (value?.number != null)
                {
                    ImageCancellationTokenSource = new CancellationTokenSource();
                    DataFetcher.GetApiObject<Pokemon>(value.number.Value).ContinueWith(
                        t => { Dispatcher.Invoke(() => { PokemonImage = new Uri(t.Result.Sprites.FrontMale); }); },
                        ImageCancellationTokenSource.Token);
                }
            }
        }

        public Uri PokemonImage { get; set; }

        public MainWindowViewModel(Dispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;
            Pokemons = LoadPokeData(Assembly.GetExecutingAssembly().GetFileStream("pokemon.json"));
            FilteredPokemons = CollectionViewSource.GetDefaultView(Pokemons);
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