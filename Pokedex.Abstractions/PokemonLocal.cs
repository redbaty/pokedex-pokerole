using System.Collections.Generic;

namespace Pokedex.Abstractions
{
    public class PokemonLocal
    {
        public int? number { get; set; }
        public string name { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string category { get; set; }
        public string pokedex { get; set; }
        public List<string> evolutions { get; set; }
        public string evolution_method { get; set; }
        public string stage { get; set; }
        public string generation { get; set; }
        public List<string> types { get; set; }
        public List<string> strength { get; set; }
        public List<string> dexterity { get; set; }
        public List<string> vitality { get; set; }
        public List<string> special { get; set; }
        public List<string> insight { get; set; }
        public List<string> abilities { get; set; }
        public string base_hp { get; set; }
        public string disobedience { get; set; }
        public List<Move> moves { get; set; }
    }

    public class Move
    {
        public string name { get; set; }
        public string exp { get; set; }
    }
}