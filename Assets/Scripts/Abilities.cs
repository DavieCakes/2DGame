using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace PlayerAbilities
{

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
    public class Ability : IObservable<Ability>
    {
        public int BaseValue; // base stat value
        private readonly List<Modifier> _modifiers;
        public readonly ReadOnlyCollection<Modifier> Modifiers; // mirrors stateModifiers for access, keeps original values private

        private List<IObserver<Ability>> observers;

        public int Value
        {
            get
            {
                if (isDirty)
                {
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        protected bool isDirty = true;
        protected int _value;

        public Ability(int baseValue)
        {
            // this.attributeType = attributeType;
            BaseValue = baseValue;
            _modifiers = new List<Modifier>();
            Modifiers = _modifiers.AsReadOnly();
            observers = new List<IObserver<Ability>>();
        }

        public void AddModifier(Modifier mod)
        {
            isDirty = true;
            _modifiers.Add(mod);
            NotifyObservers();
        }

        public bool RemoveModifier(Modifier mod)
        {
            if (_modifiers.Remove(mod))
            {
                isDirty = true;
                NotifyObservers();
                return true;
            }
            return false;
        }

        protected int CalculateFinalValue()
        {
            float finalValue = BaseValue;
            int sumPercentAdd = 0;
            for (int i = 0; i < _modifiers.Count; i++)
            {
                Modifier mod = _modifiers[i];
                if (mod.Type == ModifierType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == ModifierType.Percent)
                {
                    sumPercentAdd += _modifiers[i].Value;
                }
            }
            finalValue *= 1 + sumPercentAdd;
            return (int)Math.Round(finalValue);
        }

        public bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;
            for (int i = _modifiers.Count - 1; i >= 0; i--)
            {
                if (_modifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    _modifiers.RemoveAt(i);
                }
            }
            if (didRemove)
            {
                NotifyObservers();
            }
            return didRemove;
        }

        override
        public string ToString()
        {
            string result = "";
            // result += this.attributeType.ToString() + ": " + this.Value;
            if (this.Value < 0)
            {
                result += this.Value;
            }
            else
            {
                result += "+" + this.Value;
            }
            return result;
        }

        public IDisposable Subscribe(IObserver<Ability> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                observer.OnNext(this);
            }
            return new Unsubscriber<Ability>(observers, observer);
        }

        protected void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.OnNext(this);
            }
        }
    }

    internal class Unsubscriber<Ability> : IDisposable
    {
        private List<IObserver<Ability>> _observers;
        private IObserver<Ability> _observer;
        public Unsubscriber(List<IObserver<Ability>> observers, IObserver<Ability> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
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
    public class HealthAbility : Ability, IObservable<HealthAbility>
    {
        private int damageTaken;

        public int maxHealth
        {
            get
            {
                if (isDirty)
                {
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        new
        public int Value
        {
            get
            {
                if (isDirty)
                {
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value - damageTaken;
            }
        }

        public bool Heal(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Heal amount: " + amount.ToString() + " cannot be negative.");
            }
            if (damageTaken < 1)
            {
                return false;
            }
            if (damageTaken - amount < 0)
            {
                damageTaken = 0;
                // this.isDirty = true;
                NotifyObservers();
                return true;
            }
            damageTaken = damageTaken - amount;
            // this.isDirty = true;
            NotifyObservers();
            return true;
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Damage amount: " + amount.ToString() + " cannot be negative.");
            }
            this.damageTaken += amount;
            NotifyObservers();
            // this.isDirty = true;
        }

        public bool IsDead()
        {
            return this.Value < 1;
        }

        public HealthAbility(int baseValue) : base(baseValue)
        {

        }

        public override string ToString()
        {
            string result = "";
            result += "Health: " + this.Value + "/" + this.maxHealth;
            return result;
        }

        public IDisposable Subscribe(IObserver<HealthAbility> observer)
        {
            throw new NotImplementedException();
        }
    }

    public enum ModifierType
    {
        Flat,
        Percent, // Additive: +10%, + 20% => Val + 30%
    }

    public enum AbilityType
    {
        AGILITY,
        ATTACK,
        HEALTH,
        DEFENSE,
    }

    public class Modifier
    {
        public readonly int Value;
        public readonly ModifierType Type;
        public readonly AbilityType attType;
        public readonly object Source;
        public Modifier(int value, ModifierType type)
        {
            Type = type;
            Value = value;
        }

        public Modifier(int value, ModifierType type, object source)
        {
            Source = source;
            Type = type;
            Value = value;
        }

        public Modifier(int value, ModifierType type, object source, AbilityType modType)
        {
            Source = source;
            Type = type;
            attType = modType;
            Value = value;
        }

        override
        public string ToString()
        {
            string result = "";
            if (this.Value < 0.0)
            {
                result += this.Value + " ";
            }
            else
            {
                result += "+" + this.Value + " ";
            }
            result += "(" + this.Type.ToString() + ") " + this.attType.ToString();
            return result;
        }
    }
}