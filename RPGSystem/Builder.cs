using UnityEngine;
using System.Collections.Generic;

using Creatures;
using Items;
using Attributes;
using Databases;

namespace Builders {
    public class Builder {
        public Creature BuildCreature(string creatureName) {

            XMLDatabase database = new XMLDatabase();
            Dictionary<string, object> creatureData = database.GetCreatureData(creatureName);
            // get creature id, get creature attributes, get item_ids, get items, equip items, return
            // long creatureId = (long)creatureData["id"];
            // List<long> item_ids = (List<long>)creatureData["item_ids"];
            List<Item> inventory = new List<Item>();
            Dictionary<AttributeType, Attribute> attributes = new Dictionary<AttributeType, Attribute>();
            Creature creature;

            foreach(string item in (List<string>)creatureData["inventory"]) {
                inventory.Add(BuildItem(item));
            }

            foreach(KeyValuePair<string, long> entry in (Dictionary<string, long>)creatureData["attributes"]) {
                // Debug.Log(entry.Key + " " + entry.Value);
                attributes.Add(StringToAttributeType(entry.Key), new Attribute((float)entry.Value));
            }

            creature = new Creature(attributes, (string)creatureData["name"], (long)creatureData["id"]);
            foreach(Item item in inventory) {
                creature.PickUp(item);
            }
            return creature;
        }

        private AttributeType StringToAttributeType(string typeString) {
                    AttributeType attributeType;
                    switch(typeString.ToLower()) {
                    case "strength":
                        attributeType = AttributeType.STRENGTH;
                        break;
                    case "dexterity":
                        attributeType = AttributeType.DEXTERITY;
                        break;
                    case "constitution":
                        attributeType = AttributeType.CONSTITUTION;
                        break;
                    case "wisdom":
                        attributeType = AttributeType.WISDOM;
                        break;
                    case "charisma":
                        attributeType = AttributeType.CHARISMA;
                        break;
                    case "intelligence":
                        attributeType = AttributeType.INTELLIGENCE;
                        break;
                    case "initiative":
                        attributeType = AttributeType.INITIATIVE;
                        break;
                    case "tohit":
                        attributeType = AttributeType.ATTACK;
                        break;
                    case "attack":
                        attributeType = AttributeType.ATTACK;
                        break;
                    case "damage":
                        attributeType = AttributeType.DAMAGE;
                        break;
                    case "health":
                        attributeType = AttributeType.HEALTH;
                        break;
                    case "ac":
                        attributeType = AttributeType.AC;
                        break;
                    default:
                        throw new System.Exception("String '" + typeString + "' does not match known AttributeType");
                }
                return attributeType;
        }

        public Item BuildItem(string itemName) {
            XMLDatabase database = new XMLDatabase();
            Dictionary<string, object> itemData = database.GetItemData(itemName);
            List<Modifier> modifiers = new List<Modifier>();
            Item item = new Item((string)itemData["name"], (long)itemData["id"]);

            foreach(KeyValuePair<string, long> row in (Dictionary<string, long>)itemData["modifiers"]) {
                // Debug.Log(row.Key + row.Value);
                item.AddStatMod(new Modifier((float)row.Value, ModifierType.Flat, item, StringToAttributeType(row.Key)));
            }

            return item;
        }

        // public Item BuildItem(long itemId) {
        //     XMLDatabase database = new XMLDatabase();
        //     Dictionary<string, object> itemData = database.GetItemData(itemId);
        //     List<Modifier> modifiers = new List<Modifier>();
        //     Item item = new Item((string)itemData["name"], (long)itemData["id"]);

        //     foreach(KeyValuePair<string, long> row in (Dictionary<string, long>)itemData["modifiers"]) {
        //         // Debug.Log(row.Key + row.Value);
        //         item.AddStatMod(new Modifier((float)row.Value, ModifierType.Flat, item, StringToAttributeType(row.Key)));
        //     }

        //     return item;
        // }
    }
}