using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.src.scripts.entities
{
    public class Character
    {
        public List<Action> Actions { get; set; }
        public List<Modifier> Modifiers { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public Tile Tile { get; set; }
        public BaseStats BaseStats { get; set; }
    }
}
