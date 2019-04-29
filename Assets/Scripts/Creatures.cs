using System.Collections.Generic;
using System;
using UnityEngine;

using Items;
using Inventories;
using PlayerAbilities;


namespace Models
{
    public struct PlayerAbilities
    {
        public Ability Agility;
        public Ability Attack;
        public HealthAbility Health;
        public Ability Defense;

        public PlayerAbilities(
            int agility,
            int attack,
            int health,
            int defense)
        {

            this.Attack = new Ability(attack);
            this.Agility = new Ability(agility);
            this.Health = new HealthAbility(health);
            this.Defense = new Ability(defense);
        }

        public PlayerAbilities(Dictionary<AbilityType, Ability> _attributes)
        {
            this.Agility = _attributes[AbilityType.AGILITY];
            this.Attack = _attributes[AbilityType.ATTACK];
            this.Health = (HealthAbility)_attributes[AbilityType.HEALTH];
            this.Defense = _attributes[AbilityType.DEFENSE];
        }
    }

    public class PlayerModel : IObserver<Ability>, IObserver<Inventory>, IObservable<PlayerModel>
    {
        public string name;
        public Dictionary<AbilityType, Ability> abilitiesDict;
        public PlayerAbilities abilities;
        public Inventory inventory;
        public Dictionary<EquipSlot, Items.Equipment> currentlyEquipped;
        private List<IObserver<PlayerModel>> observers;

        // public PlayerModel()
        // {
        //     this.inventory = new Inventory();
        //     this.currentlyEquipped = new Dictionary<EquipSlot, Items.Equipment>();
        //     this.abilities = new PlayerAbilities(10, 10, 10, 10);
        //     InitAttributeHash();
        // }

        // public PlayerModel(Dictionary<AbilityType, Ability> _attributes, string name, long id)
        // {
        //     this.name = name;
        //     this.inventory = new Inventory();
        //     this.currentlyEquipped = new Dictionary<EquipSlot, Items.Equipment>();
        //     this.abilities = new PlayerAbilities(_attributes);
        //     InitAttributeHash();
        // }

        public PlayerModel(int health, int agility, int defense, int attack, string name)
        {
            this.name = name;
            this.inventory = new Inventory();
            this.currentlyEquipped = new Dictionary<EquipSlot, Items.Equipment>();
            this.abilities = new PlayerAbilities(agility, attack, health, defense);
            observers = new List<IObserver<PlayerModel>>();
            InitAttributeDict();
            InitObservables();
        }

        private void InitAttributeDict()
        {
            this.abilitiesDict = new Dictionary<AbilityType, Ability>()
            {
                {AbilityType.AGILITY, this.abilities.Agility},
                {AbilityType.ATTACK, this.abilities.Attack},
                {AbilityType.HEALTH, this.abilities.Health},
                {AbilityType.DEFENSE, this.abilities.Defense},
            };
        }

        private void InitObservables() {
            foreach (KeyValuePair<AbilityType, Ability> ability in abilitiesDict) {
                ability.Value.Subscribe(this);
            }
            inventory.Subscribe(this);
        }

        public void PickUp(Item item)
        {
            inventory.Add(item);
        }

        /// Equips an item into the currentlyEquipped dictionary,
        /// also adds the items ability modifiers to the creature
        // If equip slot is filled, currently equiped item goes into
        // the inventory, and the given item is equiped in it's place
        public void Equip(Items.Equipment item)
        {
            // Debug.Log("Equipping: " + item.ToString());
            foreach (Modifier mod in item.modifiers)
            {
                this.abilitiesDict[mod.attType].AddModifier(mod);
            }
            if (!this.currentlyEquipped.ContainsKey(item.equipSlot))
            {
                this.currentlyEquipped.Add(item.equipSlot, item);
            }
            else
            {
                Unequip(item.equipSlot);
                this.currentlyEquipped.Add(item.equipSlot, item);
            }
        }

        /*
            Unequips an item, unequiping also removes any
            ability bonuses that item has.
            Places item into inventory
         */
        public void Unequip(EquipSlot equipSlot)
        {
            if (this.currentlyEquipped.ContainsKey(equipSlot))
            {
                foreach (Modifier mod in currentlyEquipped[equipSlot].modifiers)
                {
                    this.abilitiesDict[mod.attType].RemoveAllModifiersFromSource(currentlyEquipped[equipSlot]);
                }
                inventory.Add(currentlyEquipped[equipSlot]);
                currentlyEquipped.Remove(equipSlot);
            }
        }

        /*
            Unequips an item, unequiping also removes any
            ability bonuses that item has.
            Places item into inventory
         */
        public void Unequip(Items.Equipment item)
        {
            foreach (Modifier mod in item.modifiers)
            {
                this.abilitiesDict[mod.attType].RemoveAllModifiersFromSource(item);
            }

            currentlyEquipped.Remove(item.equipSlot);
            inventory.Add(item);
        }

        /*
            Removes an Item from the inventory, and sends it to Equip()
         */
        public bool EquipFromInventory(Items.Equipment item)
        {
            if (!inventory.EquipmentInventory.Contains(item))
            {
                return false;
            }
            inventory.Remove(item);
            Equip(item);
            return true;
        }

        /*  */
        public bool EquipFromInventory(string iconName)
        {
            foreach (Equipment equipment in this.inventory.EquipmentInventory)
            {
                if (equipment.name.Equals(iconName))
                {
                    EquipFromInventory(equipment);
                    return true;
                }
            }
            return false;
        }

        /*
            Uses standard D20 attack roll to determine attack success,
            currently it is attack vs defense
         */
        public bool Attack(PlayerModel target)
        {
            System.Random rand = new System.Random();
            int hitVal = rand.Next(1, 20) + (int)this.abilities.Attack.Value;
            if (hitVal < target.abilities.Defense.Value)
            {
                return false;
            }
            target.TakeDamage(this.abilities.Attack.Value);
            return true;
        }

        /*
            Applies damage to the creature,
            damage is a special case, it is added to
            HealthAbility.damageTaken. Look there for 
            more information
         */
        public void TakeDamage(int value)
        {
            this.abilities.Health.TakeDamage(value);
        }

        public bool isDead()
        {
            return this.abilities.Health.IsDead();
        }

        public bool TryHealthPotion()
        {
            if (this.inventory.Potions > 0 && this.abilities.Health.Heal(5))
            {
                this.inventory.RemovePotion();
                Debug.Log("Using Potion");
                return true;
            }
            return false;
        }


        override public string ToString()
        {
            string result = "";
            result += "name: " + this.name + "\n";
            result += "attributes:\n";
            foreach (KeyValuePair<AbilityType, Ability> entry in this.abilitiesDict)
            {
                if (entry.Key == AbilityType.HEALTH)
                {
                    result += entry.Value.ToString() + "\n";
                }
                else
                {
                    result += entry.Key.ToString() + ": " + entry.Value.ToString() + "\n";
                }
            }
            result += this.inventory.Potions;
            // result += "inventory:\n";
            // result += this.inventory.ToString();
            // result += currentlyEquipped.Count + " Items Currently Equipped\n";
            // foreach (KeyValuePair<EquipSlot, Equipment> entry in this.currentlyEquipped)
            // {
            //     result += entry.Key.ToString() + ": " + entry.Value.ToString() + "\n";
            // }
            return result;

        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Inventory value)
        {
            NotifyObservers();
        }

        public void OnNext(Ability value)
        {
            NotifyObservers();
        }

        private void NotifyObservers() {
            if (observers != null) {
                foreach (var observer in observers) {
                    observer.OnNext(this);
                }
            }
        }

        public IDisposable Subscribe(IObserver<PlayerModel> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                observer.OnNext(this);/// 
            }
            return new Unsubscriber<PlayerModel>(observers, observer);
        }
    }

    internal class Unsubscriber<PlayerModel> : IDisposable
    {
        private List<IObserver<PlayerModel>> _observers;
        private IObserver<PlayerModel> _observer;
        public Unsubscriber(List<IObserver<PlayerModel>> observers, IObserver<PlayerModel> observer)
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
}

