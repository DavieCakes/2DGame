using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace Attributes {
    public class Attribute {
        public float BaseValue; // base stat value
        private readonly List<Modifier> _modifiers;
        public readonly ReadOnlyCollection<Modifier> Modifiers; // mirrors stateModifiers for access, keeps original values private
        public float Value {
            get {
                if(isDirty){
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        private bool isDirty = true;
        // true if value doesn't reflect final value
        // becomes true after changing _modifiers
        private float _value; // stored final value after calculation
        // public readonly AttributeType attributeType;
    

        public Attribute(float baseValue) {
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

        // percentile _modifiers are added at the end
        private float CalculateFinalValue() {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;
            for (int i = 0; i < _modifiers.Count; i++) {
                Modifier mod = _modifiers[i];
                if (mod.Type == ModifierType.Flat) {
                    finalValue += mod.Value;
                } else if (mod.Type == ModifierType.Percent) {
                    sumPercentAdd += _modifiers[i].Value;
                }
            }
            finalValue *= 1 + sumPercentAdd;
            return (float)Math.Round(finalValue, 4);
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

    public enum ModifierType {
        Flat,
        Percent, // Additive: +10%, + 20% => Val + 30%
    }

    public enum AttributeType {
        // Primary Attributes
        Strength,
        Dexterity,
        Constitution,
        Wisdom,
        Intelligence,
        Charisma,
        // Secondary Attributes
        Attack,
        Damage,
        Initiative,
        AC,
    }

    public class Modifier {
        public readonly float Value;
        public readonly ModifierType Type;
        public readonly AttributeType attType;
        public readonly object Source;
        public Modifier(float value, ModifierType type) {
            Type = type;
            Value = value;
        }

        public Modifier(float value, ModifierType type, object source) {
            Source = source;
            Type = type;
            Value = value;
        }

        public Modifier(float value, ModifierType type, object source, AttributeType modType) {
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