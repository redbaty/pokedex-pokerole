using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Net.Http;
using System.Net.Http.Headers;
using PokeAPI;
using System.Windows.Media.Imaging;

namespace pokedex_pokerole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();           
            List<pokemon> pkmns = LoadPokeData(@"C:\Users\e-eu\source\repos\pokedex pokerole\pokedex pokerole\data\pokemon.json");
            pkmnList.ItemsSource = pkmns;

            

        }

        private List<pokemon> LoadPokeData(string path)//todo :  nao adicionar pokemons mega
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();

                List<pokemon> pkmns = JsonConvert.DeserializeObject<List<pokemon>>(json);
                //ate aqui ok preenchemos nosso array com a lista do jasao e os argonautas
                for (var i = 0; i < pkmns.Count; i++)
                {
                    if (pkmns[i].name.Contains("("))
                    { pkmns.Remove(pkmns[i]); i--; };
                }

                return pkmns;
            }
        }

        private void LoadSingularPkmn(pokemon pkmn) //pkmns[number-1]
        {
            name.Text = pkmn.name;
            number.Text = pkmn.number;
            hieght.Text = pkmn.height+"m";
            weight.Text = pkmn.weight+"kg";
            category.Text = pkmn.category;
            stage1.Text = pkmn.evolutions[0];
            stage2.Text = string.IsNullOrEmpty(pkmn.evolutions.ElementAtOrDefault(1)) ? "" :  pkmn.evolutions[1];
            stage3.Text = string.IsNullOrEmpty(pkmn.evolutions.ElementAtOrDefault(2)) ? "" :  pkmn.evolutions[2];
            str.Content = computeStats(pkmn.strength[0], pkmn.strength[1]);//pkmn.stat[a,b] is a vector in which a is base and b is limit
            dex.Content = computeStats(pkmn.dexterity[0], pkmn.dexterity[1]);
            vit.Content = computeStats(pkmn.vitality[0], pkmn.vitality[1]);
            spe.Content = computeStats(pkmn.special[0], pkmn.special[1]);
            ins.Content = computeStats(pkmn.insight[0], pkmn.insight[1]);
            Sprite(Int32.Parse(pkmn.number));
            about.Text = pkmn.pokedex;
            movesList.ItemsSource = pkmn.moves;
            if (movesList.Items.Count > 0)
            {
                movesList.ScrollIntoView(movesList.Items[0]);
            }
        }

        private void pkmnList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            foreach (pokemon poke in e.AddedItems)
            { LoadSingularPkmn(poke); }

        }

        private string computeStats(string baseStat, string limitStat)
        {
            if (string.IsNullOrEmpty(baseStat)){
                return "◯◯◯◯◯◯◯◯◯◯";
            }
            else {
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
                return composedStat; }
        }

        private async void  magicalThing()
        {
            Pokemon p = await DataFetcher.GetApiObject<Pokemon>(1);            
            Console.WriteLine(p.Name + " " + p.Stats[5].Stat.Name +" " + p.Stats[5].BaseValue);

            Pokemon p2 = await DataFetcher.GetApiObject<Pokemon>(2);
            Console.WriteLine(p2.Name + " " + p2.Stats[5].Stat.Name + " " + p2.Stats[5].BaseValue);

            Pokemon p3 = await DataFetcher.GetApiObject<Pokemon>(3);
            Console.WriteLine(p3.Name + " " + p3.Stats[5].Stat.Name + " " + p3.Stats[5].BaseValue);

            Pokemon p4 = await DataFetcher.GetApiObject<Pokemon>(130);
            Console.WriteLine(p4.Name + " " + p4.Stats[5].Stat.Name + " " + p4.Stats[5].BaseValue);
        }

        private async void Sprite(int id)
        {
            Pokemon p = await DataFetcher.GetApiObject<Pokemon>(id);
            img.Source = new BitmapImage(new Uri(p.Sprites.FrontMale));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            magicalThing();
        }
    
    }
}
