using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Inventories
{
    // This is just a class that is a list of equipment objects, it is included in the general 'Inventory' class
    public class EquipmentInventory : List<Items.Equipment>
    {
        public EquipmentInventory()
        {
        }

        public EquipmentInventory(IEnumerable<Items.Equipment> collection) : base(collection)
        {
        }

        public EquipmentInventory(int capacity) : base(capacity)
        {
        }
    }

    // A container class, it holds all possible types of items in one container, 
    //this can be used for item drops, chests, enemy inventories, shopkeep inventories, 
    //the player inventory, or any other collection of in game items
    public class Inventory
    {
        public int keys;

        public int gold;

        public int potions;
 
        public EquipmentInventory equipmentInventory;

        public Inventory()
        {
            this.keys = 0;
            this.gold = 0;
            this.potions = 0;
            this.equipmentInventory = new EquipmentInventory();
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
        public bool Contains(Item item) {
            if (item.GetType() == typeof(Equipment)) {
                return this.equipmentInventory.Contains((Equipment)item);
            }
            if (item.GetType() == typeof(Gold)) {
                return this.gold != 0;
            }
            if (item.GetType() == typeof(HealthPotion)) {
                return this.potions != 0;
            }
            if (item.GetType() == typeof(Key)) {
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
            Items.Equipment item = new Items.Equipment("armor_1","test", EquipSlot.ARMOR);
            i.Add(item); // success
        }
    }
}