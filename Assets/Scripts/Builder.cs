using UnityEngine;
using System.Collections.Generic;
using System;

using Creatures;
using Items;
using PlayerAbilities;
using Databases;

namespace Builders
{

    /*
        This is a Singleton that holds all of the equipment the player can recieve.

        When a new Item drop is asked for, Builder asks this EquipmentQueue for the next
        peice of equipment.

        Pop() returns null elements when the list is empty, important to check for null when using this
        container.

     */
    public class EquipmentQueue {
        private static readonly EquipmentQueue instance = new EquipmentQueue();

        /* 
            The list is initialized once, so as soon as it is empty, Pop() will return null elements.
        */
        List <Equipment> equipment = new List<Equipment>();
        bool equipmentAdded = false;
        static EquipmentQueue() {}

        private EquipmentQueue() {}

        public static EquipmentQueue Instance {
            get {
                return instance;
            }
        }

        public bool InitEquimentQueue(List<Equipment> inEquip) {
            if (equipmentAdded) {
                return false;
            }

            bool dupFound = false;
            foreach (Equipment e in inEquip) {
                foreach (Equipment j in equipment) {
                    if( j.name == e.name) {
                        dupFound = true;
                        break;
                    }
                }
                if (dupFound) {
                    continue; // continue to next equipment in inEquip
                } else {
                    equipment.Add(e);
                }
            }

            // no duplicates here, now sort by sum of modifiers
            equipment.Sort();
            equipmentAdded = true;
            return true;
        }

        public Equipment Pop() {
            Equipment result = null;
            if (equipment.Count != 0) {
                result = equipment[0];
                equipment.RemoveAt(0);
                
            }
            return result;
        }

        public Equipment Get(string name) {
            Equipment result = null;

            foreach (Equipment e in equipment) {
                if(e.name == name) {
                    result = e;
                    equipment.Remove(e);
                    break;
                }
            }

            return result;
        }

        public int Count() {
            return equipment.Count;
        }    }
    public class Builder
    {
        private static EquipmentQueue equipmentQueue = null; // starts null, checked if still null in builder methods


        /* This isn't currently used, but I'm keeping it here in case we want to save Creature stats in xml */

        // public static Creature BuildCreature(string creatureName) {

        //     Database database = new Database();
        //     Dictionary<string, object> creatureData = database.GetCreatureData(creatureName);
        //     get creature id, get creature abilitiess, get item_ids, get items, equip items, return
        //     long creatureId = (long)creatureData["id"];
        //     List<long> item_ids = (List<long>)creatureData["item_ids"];
        //     List<Items.Equipment> inventory = new List<Items.Equipment>();
        //     Dictionary<AbilityType, Ability> abilities = new Dictionary<AbilityType, Ability>();
        //     Creatures.Creature creature;

        //     foreach(string item in (List<string>)creatureData["inventory"]) {
        //         inventory.Add(BuildEquipment(item));
        //     }

        //     foreach(KeyValuePair<string, long> entry in (Dictionary<string, long>)creatureData["Abilitys"]) {
        //         Debug.Log(entry.Key + " " + entry.Value);
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

        private static void InitEquipmentQueue() {
            if (equipmentQueue == null) {
                equipmentQueue = EquipmentQueue.Instance;
            }
            equipmentQueue.InitEquimentQueue(BuildAllEquipment());
        }

        /*
            Only Builds Equipment,
            This may introduce duplicates, it doesn't route through the EquipmentQueue
        */
        private static Equipment BuildEquipment(string equipmentName)
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

        /* 
            Safe builder method, attempts to place object of given itemName into
            the 'toReturn' reference. On failures returns false, on success returns
            true.
         */
        public static bool GetEquipment(string itemName, Equipment toReturn) {
            if (equipmentQueue == null) {
                InitEquipmentQueue();
            }
            toReturn = equipmentQueue.Get(itemName);
            if (toReturn == null) {
                return false;
            } 
            return true;
        }

        // builds all equipment in data.xml, for testing
        private static List<Equipment> BuildAllEquipment()
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
            }
            return result;
        }
        /*
            Builds a random list of items, can include any item type (gold, potions, keys, equipment)
         */
        public static List<Item> BuildRandomItemDrop()
        {
            if(equipmentQueue == null) {
                InitEquipmentQueue();
            }
            List<Item> items = new List<Item>();
            System.Random rand = new System.Random();
            int size = rand.Next(1, 3); // .Next(int, int) returns int between [int, int), inclusive -> exclusive

            for (int i = 0; i < size; i++) {
                items.Add(new HealthPotion());
            }

            Equipment toAdd = equipmentQueue.Pop();
            if (toAdd != null) items.Add(toAdd);

            Debug.Log("Building Random Drop List");
            foreach (Item i in items)
            {
                Debug.Log("Building: " + i.ToString());
            }
            return items;
        }
    }
}