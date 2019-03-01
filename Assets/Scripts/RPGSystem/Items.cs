using Creatures;
using System.Collections.Generic;

namespace Items {
    public class Item
    {
        public readonly List<StatModifier> StatMods;
        public readonly string name;
        public readonly int id;

        public Item(StatModifier[] StatMods, string name, int id) {
            this.StatMods = new List<StatModifier>();
            foreach (StatModifier mod in StatMods) {
                this.StatMods.Add(mod);
            }
            this.name = name;
            this.id = id;
        }
        public Item(StatModifier StatMod, string name, int id) {
            this.StatMods = new List<StatModifier>();
            this.StatMods.Add(StatMod);
            this.name = name;
            this.id = id;
        }

        public Item(string name, int id) {
            this.StatMods = new List<StatModifier>();
            this.name = name;
            this.id = id;
        }
        public void AddStatMod(StatModifier StatMod) {
            this.StatMods.Add(StatMod);
        }

        public void Equip(Creature c)
        {
            foreach (StatModifier mod in StatMods)
            {
                c.Attributes[mod.attType].AddModifier(mod);
            }
            // Create the modifiers and set the Source to "this"
            // Note that we don't need to store the modifiers in variables anymore
            // c.Strength.AddModifier(new StatModifier(10f, StatModType.Flat, this));
            // c.Strength.AddModifier(new StatModifier(0.1f, StatModType.PercentAdd, this));
        }
    
        public void Unequip(Creature c)
        {
            // Remove all modifiers applied by "this" Item
            // c.Strength.RemoveAllModifiersFromSource(this);

            foreach (StatModifier mod in StatMods)
            {
                c.Attributes[mod.attType].RemoveAllModifiersFromSource(this);
            }
        }
    }
}