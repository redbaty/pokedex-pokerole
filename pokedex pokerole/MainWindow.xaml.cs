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
    /// 

    public partial class MainWindow : Window
    {
        private List<move_complete> moves;
        private List<pokemon> pkmns; 
        public MainWindow()
        {
            InitializeComponent();           
            pkmns = LoadPokeData(@"C:\Users\e-eu\source\repos\pokedex pokerole\pokedex pokerole\data\pokemon.json");
            pkmnList.ItemsSource = pkmns;
            moves = LoadMoves(@"C:\Users\e-eu\source\repos\pokedex pokerole\pokedex pokerole\data\moves.json");


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

        private List<move_complete> LoadMoves(string path)//
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                List<move_complete> moves = JsonConvert.DeserializeObject<List<move_complete>>(json);
                return moves;
            }
        }

        private void LoadSingularPkmn(pokemon pkmn) //pkmns[number-1]
        {
            name.Text = pkmn.name;
            number.Text = pkmn.number;
            hieght.Text = pkmn.height+"m";
            weight.Text = pkmn.weight+"kg";
            category.Text = pkmn.category;
            stage1.Text = string.IsNullOrEmpty(pkmn.evolutions.ElementAtOrDefault(0)) ? pkmn.name : pkmn.evolutions[0]; ;
            stage2.Text = string.IsNullOrEmpty(pkmn.evolutions.ElementAtOrDefault(1)) ? "" :  pkmn.evolutions[1];
            stage3.Text = string.IsNullOrEmpty(pkmn.evolutions.ElementAtOrDefault(2)) ? "" :  pkmn.evolutions[2];
            str.Content = computeStats(pkmn.strength[0], pkmn.strength[1]);//pkmn.stat[a,b] is a vector in which a is base and b is limit
            dex.Content = computeStats(pkmn.dexterity[0], pkmn.dexterity[1]);
            vit.Content = computeStats(pkmn.vitality[0], pkmn.vitality[1]);
            spe.Content = computeStats(pkmn.special[0], pkmn.special[1]);
            ins.Content = computeStats(pkmn.insight[0], pkmn.insight[1]);
            disob.Content = computeStats(pkmn.disobedience, "5");
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
            { 
              LoadSingularPkmn(poke); 
              clearMove(); }

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

            List<PokemonSpecies> all = new List<PokemonSpecies>;
            foreach(pokemon p in pkmns)
            {
                PokemonSpecies p2 = await DataFetcher.GetApiObject<PokemonSpecies>(Int32.Parse(p.number));
                all.Add(p2);
                Console.WriteLine("add " + p2.Name + " ..");
            }
            Console.WriteLine("all added.");         

            Console.WriteLine(p2.Name + " "+ p2.Names +" " + p2.PokedexNumbers);//species.EvolvesFromSpecies = null > is first form
            }
        

        private string hpBase(pokemon pokeRole, PokemonSpecies pokeApi, List<PokemonSpecies> all, Pokemon pokeApiPoke)
        /* The next was extracted from "Pokerole Ze stuff for later" on https://docs.google.com/document/d/180rP_Qc8MrPvNq99HZiBzVYvvqdwu60GnkIwh0papDU/edit
          As a note, Pokémon HP is a difficult thing to put to narrative meaning. How would you compare a 28 foot rock snake having its dreams eaten compared to a small fiery lizard getting squirted with a stream of bubbles? So, it might be easier to just stick to having Base HP based on the video games stats instead of coming up with a narrative reason to determine Base HP.

        The math behind the new tabletop stats; take the video game stats of a Pokémon and divide by 23.5, rounding down, with the following exceptions and guidelines:
        - Total HP
        Base HP + Vitality = Total HP
        Stat changes to Vitality, such as from moves, abilities, items, etc, do not affect total HP in any way.
        - Base HP
        Any Pokémon that can evolve has a minimum Base HP of 3.
        Any Pokémon that does not evolve has a minimum Base HP of 4.
        If a Pokémon evolves into another Pokémon with the same Base HP or less, the evolved Pokémon’s Base HP is one higher (Riolu and Lucario would both be 3, so Lucario is 4). This can stack with each evolution.
        Any Pokémon with Wonder Guard will have Base HP 1 and does not add Vitality to HP. This is an exception to all other rules and guidelines.
         */
        {   
            if (pokeRole.abilities.Contains("Wonder Guard")) { return "1"; } //exception

            int hpMin = 3;
            int hp = Convert.ToInt32(Math.Floor(pokeApiPoke.Stats[5].BaseValue / 23.5));            //stats is an array with the stats, base hp is [5]
            if (Evolves(pokeRole, all)){
                hpMin = 3;
            } else hpMin = 4;
            if (pokeApi.EvolvesFromSpecies != null)
            {
              hpMin = Int32.Parse(pkmns.Find(p => p.name == pokeApi.EvolvesFromSpecies.Name).base_hp) +1; //if it has a previous evolution, it's minimum hp its the previous form +1
            }
            return Math.Max(hp,hpMin).ToString();            
        }

        private bool Evolves(pokemon pokeRole, List<PokemonSpecies> all)
        {
           
            foreach(PokemonSpecies p in all)
            {
                if (p.EvolvesFromSpecies.Name == pokeRole.name) return true; //it does evolve
            }
            return false;
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

        private void movesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Move mov in e.AddedItems)
            { LoadSingularMov(mov); }
        }

        private void LoadSingularMov(Move mov)
        {
            move_complete thunderbolt =  moves.Find(m => m.name.Equals(mov.name));
            moveName.Text = thunderbolt.name;
            moveDesc.Text = thunderbolt.description;
            moveAcc.Text = thunderbolt.accuracy;
            moveType.Text = thunderbolt.type;
            moveTarget.Text = thunderbolt.targets;
            moveEffect1.Text = thunderbolt.effect;
            moveDamage.Text = thunderbolt.damage_pool;
        }

        private void clearMove()
        {
            string yay = "???";
            moveName.Text = yay;
            moveDesc.Text = yay;
            moveAcc.Text = yay;
            moveType.Text = yay;
            moveTarget.Text = yay;
            moveEffect1.Text = yay;
            moveDamage.Text = yay;
        }
    }
}
