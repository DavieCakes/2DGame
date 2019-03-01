using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

using LevelingStrategies;
using ActionStrategies;
using Items;


namespace Creatures {

    public class Ability {

    }

    public class CreatureAction {

    }

    public class Creature {
        public int id;
        public string name;
        public Dictionary<CreatureAttributeType, CreatureStat> Attributes;
        public List<Item> inventory;
        public Dictionary<string, Item> EquipSlots;

        // similar to items -> statmods, abilities contain statmods and actionmods
        public List<Ability> Abilities;
        public List<ActionStrategy> Actions;
        // public LevelingStrategy Leveler;

        public List<string> spells;

        /**
        
        on_click(variable) -> character.Actions(variable)
        on_trigger_ai(variable) -> character.Actions(variable)
        
         */
        public bool[] BonusActionsTaken;
        public bool[] ReactionsTaken; //? how are we going to use these?

        public Creature() {
            inventory = new List<Item>();
            this.Attributes = new Dictionary<CreatureAttributeType, CreatureStat>()
            {
                {CreatureAttributeType.Strength, new CreatureStat(-1)},
                {CreatureAttributeType.Dexterity, new CreatureStat(-1)},
                {CreatureAttributeType.Constitution, new CreatureStat(-1)},
                {CreatureAttributeType.Wisdom, new CreatureStat(-1)},
                {CreatureAttributeType.Charisma, new CreatureStat(-1)},
                {CreatureAttributeType.Intelligence, new CreatureStat(-1)},
                // calculate secondary attributes separately
                {CreatureAttributeType.Initiative, new CreatureStat(-1)},
                {CreatureAttributeType.ToHit, new CreatureStat(-1)},
                {CreatureAttributeType.Damage, new CreatureStat(-1)},
            };
            EquipSlots = new Dictionary<string, Item>();
            Abilities = new List<Ability>();
            Actions = new List<ActionStrategy>();
            // Leveler = new LevelingStrategy();
            spells = new List<string>();
        }

        public void Equip(Item item)
        {
            foreach (StatModifier mod in item.StatMods)
            {
                this.Attributes[mod.attType].AddModifier(mod);
            }
            inventory.Add(item); //TODO these should equip into equip slots
        }

        public void Unequip(Item item)
        {
            foreach (StatModifier mod in item.StatMods)
            {
                this.Attributes[mod.attType].RemoveAllModifiersFromSource(item);
            }

            inventory.Remove(item); //TODO these should unequip from equip slots
        }

        public void CalculateStats() {
            // primary attributes => secondary attributes
            // reconsider items
            // reconsider abilities
        }
    }

    public class CreatureStat {
        public float BaseValue; // base stat value
        private readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers; // mirrors stateModifiers for access, keeps original values private

        private bool isDirty = true;
        // true if value doesn't reflect final value
        // becomes true after changing modifiers
        private float _value; // stored final value after calculation

        public CreatureStat(float baseValue) {
            BaseValue = baseValue;
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public float Value {
            get {
                if(isDirty){
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        public void AddModifier(StatModifier mod) {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        private int CompareModifierOrder(StatModifier a, StatModifier b) {
            if (a.Order < b.Order) {
                return -1;
            } else if (a.Order > b.Order) {
                return 1;
            } else {
                return 0; // if ==
            }
        }

        public bool RemoveModifier(StatModifier mod) {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        private float CalculateFinalValue() {
            float finalValue = BaseValue;
            float sumPercentAdd = 0; // sum of percentadd modifiers
            for (int i = 0; i < statModifiers.Count; i++) {
                StatModifier mod = statModifiers[i];
                // finalValue += statModifiers[i].Value;

                // Perc Mult is applied to base values, and stacked by order
                if (mod.Type == StatModType.Flat) {
                    finalValue += mod.Value;
                } else if (mod.Type == StatModType.PercentMult) {
                    finalValue *= 1 + mod.Value; // Value = Value * (1 + percentage)
                } else if (mod.Type == StatModType.PercentAdd) {
                    sumPercentAdd += statModifiers[i].Value;

                    // if at end of list, or next value is diff type, add sum to final
                    if(i+1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd){
                        // TODO this may result in inconsistent mod applications, dependent on order
                        finalValue *= 1 + sumPercentAdd; // mult usm with prev. final value
                        sumPercentAdd = 0;
                    }
                }
            }

            // TODO do we want to stack percentages at the end ALWAYS, or just define an order per entity?
            // finalValue *= 1 + sumPercentAdd;

            // Rounding gets around dumb float calculation errors
            // 4 significaiton digits is usually enough
            return (float)Math.Round(finalValue, 4);
        }

        public bool RemoveAllModifiersFromSource(object source) {
            bool didRemove = false;
            for (int i = statModifiers.Count - 1; i >= 0; i--) {
                if (statModifiers[i].Source == source) {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }
    }

    public enum StatModType {
        Flat,
        PercentAdd, // Additive: +10%, + 20% => Val + 30%
        PercentMult, // Multipliciative: *10%, *20% => (Val * 1.1) * 1.2
    }

    public enum CreatureAttributeType {
        // Primary Attributes
        Strength,
        Dexterity,
        Constitution,
        Wisdom,
        Intelligence,
        Charisma,
        // Secondary Attributes
        ToHit,
        Damage,
        Initiative,
    }

    public class StatModifier {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly CreatureAttributeType attType;
        public readonly int Order; // order of stat in stat list
        public readonly object Source;
        public StatModifier(float value, StatModType type) {
            Type = type;
            Value = value;
        }
        public StatModifier(float value, StatModType type, int order) {
            Type = type;
            Value = value;
            Order = order;
        }

        public StatModifier(float value, StatModType type, int order, object source) {
            Source = source;
            Type = type;
            Value = value;
            Order = order;
        }

        public StatModifier(float value, StatModType type, CreatureAttributeType modType, object source) {
            Source = source;
            Type = type;
            attType = modType;
            Value = value;
        }
    }

/*
without setting soruces:
 public class Item // Hypothetical item class
{
    public void Equip(Character c)
    {
            We need to store our modifiers in variables before adding them to the stat.
        mod1 = new StatModifier(10, StatModType.Flat);
        mod2 = new StatModifier(0.1, StatModType.Percent);
        c.Strength.AddModifier(mod1);
        c.Strength.AddModifier(mod2);
    }
 
    public void Unequip(Character c)
    {
            Here we need to use the stored modifiers in order to remove them.
            Otherwise they would be "lost" in the stat forever.
        c.Strength.RemoveModifier(mod1);
        c.Strength.RemoveModifier(mod2);
    }
}
*/
// With Source

}