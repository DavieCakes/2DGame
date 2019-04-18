using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace PlayerAbilities {

    /*
        Ability maintains a base value and a list of modifers currently
        affecting it. These modifiers can be added and removed by passing in the
        object that that modifier was original attatched to, e.g.:
            Item item = new Item()
            Modifier m = new Modifer( 10, HEALTH, item)
            item.addModifer(m)
                // now, the new modifier should hold a reference to the item it was attatched to.
            creature.ability.addModifier(i.modifier)
                // later
            creature.ability.RemoveAllModifiersFromSource(item)
            
     */
    public class Ability {
        public int BaseValue; // base stat value
        private readonly List<Modifier> _modifiers;
        public readonly ReadOnlyCollection<Modifier> Modifiers; // mirrors stateModifiers for access, keeps original values private
        public int Value {
            get {
                if(isDirty){
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        protected bool isDirty = true;
        protected int _value;

        public Ability(int baseValue) {
            // this.attributeType = attributeType;
            BaseValue = baseValue;
            _modifiers = new List<Modifier>();
            Modifiers = _modifiers.AsReadOnly();
        }

        public void AddModifier(Modifier mod) {
            isDirty = true;
            _modifiers.Add(mod);
        }

        public bool RemoveModifier(Modifier mod) {
            if (_modifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        protected int CalculateFinalValue() {
            float finalValue = BaseValue;
            int sumPercentAdd = 0;
            for (int i = 0; i < _modifiers.Count; i++) {
                Modifier mod = _modifiers[i];
                if (mod.Type == ModifierType.Flat) {
                    finalValue += mod.Value;
                } else if (mod.Type == ModifierType.Percent) {
                    sumPercentAdd += _modifiers[i].Value;
                }
            }
            finalValue *= 1 + sumPercentAdd;
            return (int)Math.Round(finalValue);
        }

        public bool RemoveAllModifiersFromSource(object source) {
            bool didRemove = false;
            for (int i = _modifiers.Count - 1; i >= 0; i--) {
                if (_modifiers[i].Source == source) {
                    isDirty = true;
                    didRemove = true;
                    _modifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

        override
        public string ToString() {
            string result = "";
            // result += this.attributeType.ToString() + ": " + this.Value;
            if(this.Value < 0) {
                result += this.Value;
            } else {
                result += "+" + this.Value;
            }
            return result;
        }
    }

/*
    Health Ability was added so we can maintain a 'maxvalue' separate from 'currentvalue',
    it just extends Ability.

    max value = base value +/- modifiers 
        ... remember modifiers are mainly added and removed through items
    current value = max value - damage taken
    healing => damage taken - heal amount

 */
    public class HealthAbility : Ability
    {
        public int damageTaken;

        public int maxHealth {
             get {
                if(isDirty){
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        new
        public int Value {
            get {
                if(isDirty){
                    _value = CalculateFinalValue() - damageTaken;
                    isDirty = false;
                }
                return _value;
            }
        }

        public bool Heal(int amount) {
            if (damageTaken < 1) {
                return false;
            }
            damageTaken -= amount;
            return true;
            
        }

        public HealthAbility(int baseValue) : base(baseValue)
        {

        }


    }

    public enum ModifierType {
        Flat,
        Percent, // Additive: +10%, + 20% => Val + 30%
    }

    public enum AbilityType {
        AGILITY,
        ATTACK,
        HEALTH,
        DEFENSE,
    }

    public class Modifier {
        public readonly int Value;
        public readonly ModifierType Type;
        public readonly AbilityType attType;
        public readonly object Source;
        public Modifier(int value, ModifierType type) {
            Type = type;
            Value = value;
        }

        public Modifier(int value, ModifierType type, object source) {
            Source = source;
            Type = type;
            Value = value;
        }

        public Modifier(int value, ModifierType type, object source, AbilityType modType) {
            Source = source;
            Type = type;
            attType = modType;
            Value = value;
        }

        override
        public string ToString() {
            string result = "";
            if (this.Value < 0.0) {
                result += this.Value + " ";
            } else {
                result += "+" + this.Value + " ";
            }
            result += "(" + this.Type.ToString() + ") " + this.attType.ToString();
            return result; 
        }
    }
}