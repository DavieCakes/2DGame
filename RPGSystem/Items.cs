using Attributes;
using System.Collections.Generic;

namespace Items {
    public class Item
    { 
        public readonly List<Modifier> StatMods;
        public readonly string name;
        public readonly long id;

        public Item(IEnumerable<Modifier> StatMods, string name, long id) {
            this.StatMods = new List<Modifier>();
            foreach (Modifier mod in StatMods) {
                this.StatMods.Add(mod);
            }
            this.name = name;
            this.id = id;
        }
        public Item(Modifier StatMod, string name, long id) {
            this.StatMods = new List<Modifier>();
            this.StatMods.Add(StatMod);
            this.name = name;
            this.id = id;
        }

        public Item(string name, long id) {
            this.StatMods = new List<Modifier>();
            this.name = name;
            this.id = id;
        }

        public void AddStatMod(Modifier StatMod) {
            this.StatMods.Add(StatMod);
        }

        override
        public string ToString() {
            string result = "";
            result += "id: " + this.id + "\n";
            result += "name: " + this.name + "\n";
            result += "Modifiers: \n";
            foreach(Modifier mod in StatMods) {
                if(mod.Value > 0) {
                    result += "+";
                }
                result += mod.Value + " (" + mod.Type.ToString() + ") " + mod.attType.ToString() + "\n";
            }
            return result;
        }
    }
}