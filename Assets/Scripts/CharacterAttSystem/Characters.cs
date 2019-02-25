using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

namespace Characters {
    struct Character {
        int id;
        string name;
    }

    public class CharacterStat {
        public float BaseValue; // base stat value
        private readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers; // mirrors stateModifiers for access, keeps original values private

        private bool isDirty = true; 
        // true if value doesn't reflect final value
        // becomes true after changing modifiers
        private float _value; // stored final value after calculation

        public CharacterStat(float baseValue) {
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

    public class StatModifier {
        public readonly float Value;
        public readonly StatModType Type;
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

// With Source
public class Item
{
    public void Equip(Character c)
    {
        // Create the modifiers and set the Source to "this"
        // Note that we don't need to store the modifiers in variables anymore
        c.Strength.AddModifier(new StatModifier(10, StatModType.Flat, this));
        c.Strength.AddModifier(new StatModifier(0.1, StatModType.Percent, this));
    }
 
    public void Unequip(Character c)
    {
        // Remove all modifiers applied by "this" Item
        c.Strength.RemoveAllModifiersFromSource(this);
    }
}
 */

}