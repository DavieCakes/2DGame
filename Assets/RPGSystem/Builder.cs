using UnityEngine;
using System.Collections.Generic;

using Creatures;
using Items;
using Attributes;
using Databases;

namespace Builders {
    public class Builder {
        public Creature BuildCreature(string creatureName) {

            Database database = new Database();
            Dictionary<string, object> creatureData = database.GetCreatureData(creatureName);
            // get creature id, get creature attributes, get item_ids, get items, equip items, return
            // long creatureId = (long)creatureData["id"];
            // List<long> item_ids = (List<long>)creatureData["item_ids"];
            List<Item> inventory = new List<Item>();
            Dictionary<AttributeType, Attribute> attributes = new Dictionary<AttributeType, Attribute>();
            Creature creature;

            foreach(long id in (List<long>)creatureData["item_ids"]) {
                inventory.Add(BuildItem(id));
            }

            foreach(KeyValuePair<string, long> entry in (Dictionary<string, long>)creatureData["attributes"]) {
                // Debug.Log(entry.Key + " " + entry.Value);
                attributes.Add(StringToAttributeType(entry.Key), new Attribute((float)entry.Value));
            }

            creature = new Creature(attributes, (string)creatureData["name"], (long)creatureData["id"]);
            foreach(Item item in inventory) {
                creature.Equip(item);
            }
            return creature;
        }

        private AttributeType StringToAttributeType(string typeString) {
                    AttributeType attributeType;
                    switch(typeString.ToLower()) {
                    case "strength":
                        attributeType = AttributeType.Strength;
                        break;
                    case "dexterity":
                        attributeType = AttributeType.Dexterity;
                        break;
                    case "constitution":
                        attributeType = AttributeType.Constitution;
                        break;
                    case "wisdom":
                        attributeType = AttributeType.Wisdom;
                        break;
                    case "charisma":
                        attributeType = AttributeType.Charisma;
                        break;
                    case "intelligence":
                        attributeType = AttributeType.Intelligence;
                        break;
                    case "initiative":
                        attributeType = AttributeType.Initiative;
                        break;
                    case "tohit":
                        attributeType = AttributeType.Attack;
                        break;
                    case "attack":
                        attributeType = AttributeType.Attack;
                        break;
                    case "damage":
                        attributeType = AttributeType.Damage;
                        break;
                    case "health":
                        attributeType = AttributeType.Health;
                        break;
                    default:
                        throw new System.Exception("String '" + typeString + "' does not match known AttributeType");
                }
                return attributeType;
        }

        public Item BuildItem(string itemName) {
            Database database = new Database();
            Dictionary<string, object> itemData = database.GetItemData(itemName);
            List<Modifier> modifiers = new List<Modifier>();
            Item item = new Item((string)itemData["name"], (long)itemData["id"]);

            foreach(KeyValuePair<string, long> row in (Dictionary<string, long>)itemData["modifiers"]) {
                // Debug.Log(row.Key + row.Value);
                item.AddStatMod(new Modifier((float)row.Value, ModifierType.Flat, item, StringToAttributeType(row.Key)));
            }

            return item;
        }

        public Item BuildItem(long itemId) {
            Database database = new Database();
            Dictionary<string, object> itemData = database.GetItemData(itemId);
            List<Modifier> modifiers = new List<Modifier>();
            Item item = new Item((string)itemData["name"], (long)itemData["id"]);

            foreach(KeyValuePair<string, long> row in (Dictionary<string, long>)itemData["modifiers"]) {
                // Debug.Log(row.Key + row.Value);
                item.AddStatMod(new Modifier((float)row.Value, ModifierType.Flat, item, StringToAttributeType(row.Key)));
            }

            return item;
        }
    }
}