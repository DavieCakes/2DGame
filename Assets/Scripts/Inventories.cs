using System.Collections.Generic;
using UnityEngine;
using Items;
using System.Collections;
using System.Collections.ObjectModel;

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

        public ReadOnlyCollection<Equipment> AsReadOnly()
        {
            return new ReadOnlyCollection<Equipment>(this);
        }
    }

    // A container class, it holds all possible types of items in one container, 
    //this can be used for item drops, chests, enemy inventories, shopkeep inventories, 
    //the player inventory, or any other collection of in game items
    public class Inventory
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
                potions = value;
            }
        }
        public readonly ReadOnlyCollection<Equipment> EquipmentInventory;
        private readonly EquipmentInventory equipmentInventory;


        public Inventory()
        {
            this.keys = 0;
            this.gold = 0;
            this.potions = 0;
            this.equipmentInventory = new EquipmentInventory();
            this.EquipmentInventory = equipmentInventory.AsReadOnly();
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
                    break;
                case ItemType.GOLD:
                    this.gold += ((Gold)item).amount;
                    break;
            }
        }

        // copies the contents of the given inventory into this inventory
        public void Add(Inventory inventory)
        {
            this.gold += inventory.gold;
            this.keys += inventory.keys;
            this.potions += inventory.potions;
            foreach (Equipment equipment in inventory.equipmentInventory)
            {
                this.Add(equipment);
            }
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
                    this.gold -= ((Gold)item).amount;
                    break;
                case ItemType.EQUIPMENT:
                    equipmentInventory.Remove((Items.Equipment)item);
                    break;
            }
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
        public void RemoveGold(int amount)
        {
            this.gold -= amount;
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
        public void RemoveKeys(int amount)
        {
            this.keys -= amount;
        }

        public void AddPotion()
        {
            this.potions++;
        }
        public void AddPotions(int amount)
        {
            this.potions += amount;
        }
        public void RemovePotion()
        {
            this.potions--;
        }
        public void RemovePotions(int amount)
        {
            this.potions -= amount;
        }

        override
        public string ToString()
        {
            string result = "";
            result += "keys: " + keys + "\n";
            result += "potions: " + potions + "\n";
            foreach (Equipment item in equipmentInventory)
            {
                result += item.ToString();
            }
            return result;
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