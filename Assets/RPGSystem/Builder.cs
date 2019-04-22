using UnityEngine;
using System.Collections.Generic;
using System;

using Models;
using Items;
using PlayerAbilities;
using Databases;

namespace Builders
{
    public class Builder
    {
        // public static Creature BuildCreature(string creatureName) {

        //     Database database = new Database();
        //     Dictionary<string, object> creatureData = database.GetCreatureData(creatureName);
        //     // get creature id, get creature abilitiess, get item_ids, get items, equip items, return
        //     // long creatureId = (long)creatureData["id"];
        //     // List<long> item_ids = (List<long>)creatureData["item_ids"];
        //     List<Items.Equipment> inventory = new List<Items.Equipment>();
        //     Dictionary<AbilityType, Ability> abilities = new Dictionary<AbilityType, Ability>();
        //     Creatures.Creature creature;

        //     foreach(string item in (List<string>)creatureData["inventory"]) {
        //         inventory.Add(BuildEquipment(item));
        //     }

        //     foreach(KeyValuePair<string, long> entry in (Dictionary<string, long>)creatureData["Abilitys"]) {
        //         // Debug.Log(entry.Key + " " + entry.Value);
        //         abilities.Add(StringToAbilityType(entry.Key), new Ability((int)entry.Value));
        //     }

        //     creature = new Creatures.Creature(abilities, (string)creatureData["name"], (long)creatureData["id"]);
        //     foreach(Items.Equipment item in inventory) {
        //         creature.PickUp(item);
        //     }
        //     return creature;
        // }

        private static AbilityType StringToAbilityType(string typeString)
        {
            AbilityType AbilityType;
            switch (typeString.ToLower())
            {
                case "agility":
                    AbilityType = AbilityType.AGILITY;
                    break;
                case "attack":
                    AbilityType = AbilityType.ATTACK;
                    break;
                case "health":
                    AbilityType = AbilityType.HEALTH;
                    break;
                case "defense":
                    AbilityType = AbilityType.DEFENSE;
                    break;
                default:
                    throw new System.Exception("String '" + typeString + "' does not match known AbilityType");
            }
            return AbilityType;
        }

        private static EquipSlot StringToEquipSlot(string slotString)
        {
            EquipSlot equipSlot;
            switch (slotString.ToLower())
            {
                case "helmet":
                    equipSlot = EquipSlot.HELMET;
                    break;
                case "armor":
                    equipSlot = EquipSlot.ARMOR;
                    break;
                case "weapon":
                    equipSlot = EquipSlot.WEAPON;
                    break;
                case "boots":
                    equipSlot = EquipSlot.BOOTS;
                    break;
                default:
                    throw new SystemException("String '" + slotString + "' does not match known equip slot");
            }
            return equipSlot;
        }

        /*
            Only Builds Equipment
        */
        public static Equipment BuildEquipment(string equipmentName)
        {
            Database database = new Database();
            Dictionary<string, object> itemData = database.GetItemData(equipmentName);
            Items.Equipment item = new Items.Equipment((string)itemData["display_name"], (string)itemData["name"], StringToEquipSlot((string)itemData["slot"]));
            foreach (KeyValuePair<string, long> row in (Dictionary<string, long>)itemData["modifiers"])
            {
                item.AddStatMod(new Modifier((int)row.Value, ModifierType.Flat, item, StringToAbilityType(row.Key)));
            }

            Debug.Log("Building: " + item.ToString());

            return item;
        }

        // builds all equipment in data.xml, for testing
        public static List<Equipment> BuildAllEquipment()
        {
            Database database = new Database();
            List<Equipment> result = new List<Equipment>();
            List<Dictionary<string, object>> itemDataList = database.GetAllEquipmentData();
            Debug.Log(itemDataList.Count);
            foreach (Dictionary<string, object> itemData in itemDataList)
            {
                Items.Equipment temp = new Items.Equipment((string)itemData["display_name"], (string)itemData["name"], StringToEquipSlot((string)itemData["slot"]));
                foreach (KeyValuePair<string, long> row in (Dictionary<string, long>)itemData["modifiers"])
                {
                    temp.AddStatMod(new Modifier((int)row.Value, ModifierType.Flat, temp, StringToAbilityType(row.Key)));
                }
                result.Add(temp);
                // Debug.Log(temp.ToString());
            }
            return result;
        }
        /*
            Builds a random list of items, can include any item type (gold, potions, keys, equipment)
         */
        public static List<Item> BuildRandomItemDrop()
        {

            List<Item> items = new List<Item>();
            System.Random rand = new System.Random();
            int size = rand.Next(1, 5);
            Database data = new Database();
            Dictionary<string, object> itemData;

            for (int i = 0; i < size; i++)
            {
                int choice = rand.Next(1, 100);
                if (choice <= 40)
                {
                    items.Add(new HealthPotion());
                }
                if (choice >= 90)
                {
                    items.Add(new Key());
                    items.Add(new Gold(10));
                }
                else
                {
                    itemData = data.GetRandomEquipmentData(rand);
                    Items.Equipment item = BuildEquipment((string)itemData["name"]);
                    items.Add(item);
                }
            }

            Debug.Log("Building Random Drop List");
            foreach (Item i in items)
            {
                Debug.Log("Building: " + i.ToString());
            }
            return items;
        }

        // public Item BuildItem(long itemId) {
        //     XMLDatabase database = new XMLDatabase();
        //     Dictionary<string, object> itemData = database.GetItemData(itemId);
        //     List<Modifier> modifiers = new List<Modifier>();
        //     Item item = new Item((string)itemData["name"], (long)itemData["id"]);

        //     foreach(KeyValuePair<string, long> row in (Dictionary<string, long>)itemData["modifiers"]) {
        //         // Debug.Log(row.Key + row.Value);
        //         item.AddStatMod(new Modifier((float)row.Value, ModifierType.Flat, item, StringToAbilityType(row.Key)));
        //     }

        //     return item;
        // }
    }
}