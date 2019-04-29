using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine;
using Items;
using System.Collections;

namespace Inventories
{
    // This is just a class that is a list of equipment objects, it is included in the general 'Inventory' class
    public class EquipmentInventory : IList<Items.Equipment>
    {
        private List<Items.Equipment> internalList = new List<Equipment>();

        public Equipment this[int index] { get => ((IList<Equipment>)internalList)[index]; set => ((IList<Equipment>)internalList)[index] = value; }

        public int Count => ((IList<Equipment>)internalList).Count;

        public bool IsReadOnly => ((IList<Equipment>)internalList).IsReadOnly;

        public void Add(Equipment item)
        {
            ((IList<Equipment>)internalList).Add(item);
        }

        public void Clear()
        {
            ((IList<Equipment>)internalList).Clear();
        }

        public bool Contains(Equipment item)
        {
            return ((IList<Equipment>)internalList).Contains(item);
        }

        public void CopyTo(Equipment[] array, int arrayIndex)
        {
            ((IList<Equipment>)internalList).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Equipment> GetEnumerator()
        {
            return ((IList<Equipment>)internalList).GetEnumerator();
        }

        public int IndexOf(Equipment item)
        {
            return ((IList<Equipment>)internalList).IndexOf(item);
        }

        public void Insert(int index, Equipment item)
        {
            ((IList<Equipment>)internalList).Insert(index, item);
        }

        public bool Remove(Equipment item)
        {
            return ((IList<Equipment>)internalList).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<Equipment>)internalList).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<Equipment>)internalList).GetEnumerator();
        }

        public bool TryGetEquipment(Equipment _in, Equipment _out)
        {
            Equipment temp;
            for (int i = 0; i < this.Count; i++)
            {
                temp = this[i];
                if (temp.name.Equals(_in.name))
                {
                    _out = temp;
                    this.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool TryGetEquipment(string _in, Equipment _out)
        {
            Equipment temp;
            for (int i = 0; i < this.Count; i++)
            {
                temp = this[i];
                if (temp.name.Equals(_in))
                {
                    _out = temp;
                    this.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public ReadOnlyCollection<Equipment> AsReadOnly()
        {
            return new ReadOnlyCollection<Equipment>(this);
        }


        // public bool TryGetEquipment(Items.Equipment in, Items.Equipment out) {
        //     Equipment temp;
        //     for (int i = 0; i < this.Count; i++) {
        //         temp = this[i];
        //         if (temp.name.Equals(in.name)) {
        //             out = temp;
        //             this.RemoveAt(i);
        //             return true;
        //         }
        //     }
        //     return false;
        // }
    }

    // A container class, it holds all possible types of items in one container, 
    //this can be used for item drops, chests, enemy inventories, shopkeep inventories, 
    //the player inventory, or any other collection of in game items
    public class Inventory : IObservable<Inventory>
    {
        private int keys;
        public int Keys
        {
            get
            {
                return keys;
            }
            private set
            {
                NotifyObservers();
                keys = value;
            }
        }

        private int gold;
        public int Gold
        {
            get
            {
                return gold;
            }
            private set
            {
                NotifyObservers();
                gold = value;
            }
        }

        private int potions;
        public int Potions
        {
            get
            {
                return potions;
            }
            private set
            {
                NotifyObservers();
                potions = value;
            }
        }
        public readonly ReadOnlyCollection<Equipment> EquipmentInventory;
        private readonly EquipmentInventory equipmentInventory;
        private List<IObserver<Inventory>> observers;
        public Inventory()
        {
            this.keys = 0;
            this.gold = 0;
            this.potions = 0;
            this.equipmentInventory = new EquipmentInventory();
            EquipmentInventory = equipmentInventory.AsReadOnly();
            observers = new List<IObserver<Inventory>>();
        }

        // Adds a single item, of any subtype, to the inventory
        public void Add(Item item)
        {
            switch (item.itemType)
            {
                case ItemType.KEY:
                    this.keys++;
                    break;
                case ItemType.HEALTHPOTION:
                    this.potions++;
                    break;
                case ItemType.EQUIPMENT:
                    this.equipmentInventory.Add((Items.Equipment)item);
                    NotifyObservers();
                    break;
                case ItemType.GOLD:
                    this.gold += ((Gold)item).amount;
                    break;
            }
        }

        // copies the contents of the given inventory into this inventory
        public void Add(Inventory inventory)
        {
            this.gold += inventory.Gold;
            this.keys += inventory.Keys;
            this.potions += inventory.Potions;
            foreach (Equipment equipment in inventory.equipmentInventory)
            {
                this.Add(equipment);
            }
            NotifyObservers();
        }

        // removies a single instances of the given item, of any subtype, from this inventory
        public void Remove(Item item)
        {
            switch (item.itemType)
            {
                case ItemType.KEY:
                    keys--;
                    break;
                case ItemType.HEALTHPOTION:
                    potions--;
                    break;
                case ItemType.GOLD:
                    gold -= ((Gold)item).amount;
                    break;
                case ItemType.EQUIPMENT:
                    equipmentInventory.Remove((Items.Equipment)item);
                    NotifyObservers();
                    break;

            }
        }

        /*
            Retrieves an item (of any child type) from the inventory
         */
        public Item Get(Item item)
        {
            if (item.GetType() == typeof(Equipment))
            {
                Equipment temp = null;
                if (equipmentInventory.TryGetEquipment((Equipment)item, temp))
                {
                    return temp;
                }
                else
                {
                    throw new ArgumentException("Equipment not found in inventory");
                }
                // equipmentInventory.
            }
            if (item.GetType() == typeof(Gold))
            {
                this.Remove(item);
                return item;
            }
            if (item.GetType() == typeof(HealthPotion))
            {
                this.Remove(item);
                return item;
            }
            if (item.GetType() == typeof(Key))
            {
                this.Remove(item);
                return item;
            }
            throw new ArgumentException("Inventory.Get() has no handle for sent item type");
        }

        public Equipment GetEquipment(string name)
        {
            Equipment temp = null;
            equipmentInventory.TryGetEquipment(name, temp);
            return temp;
        }

        // Checks whether this inventory contains at least a single instance of the given item
        public bool Contains(Item item)
        {
            if (item.GetType() == typeof(Equipment))
            {
                return this.equipmentInventory.Contains((Equipment)item);
            }
            if (item.GetType() == typeof(Gold))
            {
                return this.gold != 0;
            }
            if (item.GetType() == typeof(HealthPotion))
            {
                return this.potions != 0;
            }
            if (item.GetType() == typeof(Key))
            {
                return this.keys != 0;
            }
            return false;
        }

        /*
            The following methods add or remove amounts from keys, potions, and gold. This
            may be unweidly, but they're here to make the inventory interface easy and flexible
            to use.
         */

        public void AddGold(int amount)
        {
            this.gold += amount;
        }


        public void AddKey()
        {
            this.keys++;
        }

        public void AddKeys(int amount)
        {
            this.keys += amount;
        }

        public void RemoveKey()
        {
            this.keys--;
        }

        public void AddPotions(int amount)
        {
            this.potions += amount;
        }
        public void RemovePotion()
        {
            this.potions--;
        }

        override
        public string ToString()
        {
            string result = "";
            result += "keys: " + keys + "\n";
            result += "potions: " + Potions + "\n";
            foreach (Equipment item in equipmentInventory)
            {
                result += item.ToString();
            }
            return result;
        }

        public IDisposable Subscribe(IObserver<Inventory> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                observer.OnNext(this);
            }
            return new Unsubscriber<Inventory>(observers, observer);
        }

        private void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.OnNext(this);
            }
        }
    }

    internal class Unsubscriber<Inventory> : IDisposable
    {
        private List<IObserver<Inventory>> _observers;
        private IObserver<Inventory> _observer;
        public Unsubscriber(List<IObserver<Inventory>> observers, IObserver<Inventory> observer)
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

    public class Test
    {
        public Test()
        {
            EquipmentInventory i = new EquipmentInventory();
            Items.Equipment item = new Items.Equipment("armor_1", "test", EquipSlot.ARMOR);
            i.Add(item); // success
        }
    }
}