using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using PokeAPI;
using Pokedex.Abstractions;
using Pokedex.Pokerole.Extensions;
using Move = PokeAPI.Move;

namespace Pokedex.Pokerole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(Dispatcher);
        }

        private void LoadSingularPkmn(PokemonLocal pkmn) //pkmns[number-1]
        {
            // name.Text = pkmn.name;
            // number.Text = pkmn.number;
            // hieght.Text = pkmn.height + "m";
            // weight.Text = pkmn.weight + "kg";
            // category.Text = pkmn.category;
            // stage1.Text = pkmn.evolutions[0];
            // stage2.Text = string.IsNullOrEmpty(pkmn.evolutions.ElementAtOrDefault(1)) ? "" : pkmn.evolutions[1];
            // stage3.Text = string.IsNullOrEmpty(pkmn.evolutions.ElementAtOrDefault(2)) ? "" : pkmn.evolutions[2];
            // str.Content =
            //     computeStats(pkmn.strength[0],
            //         pkmn.strength[1]); //pkmn.stat[a,b] is a vector in which a is base and b is limit
            // dex.Content = computeStats(pkmn.dexterity[0], pkmn.dexterity[1]);
            // vit.Content = computeStats(pkmn.vitality[0], pkmn.vitality[1]);
            // spe.Content = computeStats(pkmn.special[0], pkmn.special[1]);
            // ins.Content = computeStats(pkmn.insight[0], pkmn.insight[1]);
            // disob.Content = computeStats(pkmn.disobedience, "5");
            // Sprite(Int32.Parse(pkmn.number));
            // about.Text = pkmn.pokedex;
            // movesList.ItemsSource = pkmn.moves;
            // if (movesList.Items.Count > 0)
            // {
            //     movesList.ScrollIntoView(movesList.Items[0]);
            // }
        }

        string computeStats(string baseStat, string limitStat)
        {
            if (string.IsNullOrEmpty(baseStat))
            {
                return "◯◯◯◯◯◯◯◯◯◯";
            }
            else
            {
                int baseStat2 = Int32.Parse(baseStat);
                int limitStat2 = Int32.Parse(limitStat);

                //lets take this slow, its 3am
                string composedStat = "";
                for (int i = 0; i < baseStat2; i++)
                {
                    composedStat += "⚫";
                }

                for (int j = 0; j < limitStat2 - baseStat2; j++)
                {
                    composedStat += "◯";
                }

                return composedStat;
            }
        }

        private async void Sprite(int id)
        {
            Pokemon p = await DataFetcher.GetApiObject<Pokemon>(id);
            //img.Source = new BitmapImage(new Uri(p.Sprites.FrontMale));
        }

        private void movesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Move mov in e.AddedItems)
            {
                LoadSingularMov(mov);
            }
        }

        private void LoadSingularMov(Move mov)
        {
        }

        private void SelectPokemon(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement &&
                frameworkElement.DataContext is PokemonLocal pokemonLocal &&
                DataContext is MainWindowViewModel viewModel)
            {
                viewModel.SelectedPokemon = pokemonLocal;
            }
        }
    }
}