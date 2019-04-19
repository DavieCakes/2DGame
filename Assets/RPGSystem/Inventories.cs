using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Inventories
{
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

    public class Inventory
    {
        public int keys;
        // {
        //     set
        //     {
        //         // keys should not be negative
        //         keys = value;
        //         Debug.Log("keys set: " + keys);
        //     }
        //     get
        //     {
        //         Debug.Log("get keys : " + keys);
        //         return keys;
        //     }
        // }
        public int gold;
        // {
        //     set
        //     {
        //         // gold should not be negative
        //         gold = value;
        //     }
        //     get
        //     {
        //         return gold;
        //     }
        // }
        public int potions;
        // {
        //     set
        //     {
        //         // potions should not be negative
        //         potions = (value < 0) ? 0 : value;
        //     }
        //     get
        //     {
        //         return potions;
        //     }
        // }
        public EquipmentInventory equipmentInventory;

        public Inventory()
        {
            this.keys = 0;
            this.gold = 0;
            this.potions = 0;
            this.equipmentInventory = new EquipmentInventory();
        }

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
            Items.Equipment item = new Items.Equipment("name","armor_1", EquipSlot.ARMOR);
            i.Add(item); // success
        }
    }
}