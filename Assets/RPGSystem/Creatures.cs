using System.Collections.Generic;

using Items;
using Inventories;
using Attributes;
 

namespace Creatures {
    public class Creature {
        public long id;
        public string name;
        // public List<Attribute> Attributes;
        public Dictionary<AttributeType, Attribute> Attributes;
        // // creature.Attributes.Strength = ...
        // public struct _Attributes {
        //     Attribute Strength;
        //     Attribute Dexterity;
        //     Attribute Constitution;
        //     Attribute Wisdom;
        //     Attribute Charisma;
        //     Attribute Intelligence;
        //     Attribute Initiative;
        //     Attribute Attack;
        //     Attribute Damage;
        // }
        // private _Attributes a = new _Attributes();
        public Inventory inventory;

        public Creature() {
            inventory = new Inventory();
            this.Attributes = new Dictionary<AttributeType, Attribute>()
            {
                {AttributeType.Strength, new Attribute(10)},
                {AttributeType.Dexterity, new Attribute(10)},
                {AttributeType.Constitution, new Attribute(10)},
                {AttributeType.Wisdom, new Attribute(10)},
                {AttributeType.Charisma, new Attribute(10)},
                {AttributeType.Intelligence, new Attribute(10)},
                // calculate secondary attributes separately
                {AttributeType.Initiative, new Attribute(10)},
                {AttributeType.Attack, new Attribute(10)},
                {AttributeType.Damage, new Attribute(10)},
            };

        // this.Attributes = new List<Attribute>()
        //     {
        //         {new Attribute(10, AttributeType.Strength)},
        //         {new Attribute(10, AttributeType.Dexterity)},
        //         {new Attribute(10, AttributeType.Constitution)},
        //         {new Attribute(10, AttributeType.Wisdom)},
        //         {new Attribute(10, AttributeType.Charisma)},
        //         {new Attribute(10, AttributeType.Intelligence)},
        //         {new Attribute(10, AttributeType.Initiative)},
        //         {new Attribute(10, AttributeType.Attack)},
        //         {new Attribute(10, AttributeType.Damage)},
        //     };
        }

        public Creature(Dictionary<AttributeType, Attribute> attributes, string name, long id) {
            this.name = name;
            this.id = id;
            inventory = new Inventory();

            // attribute enforcement
            this.Attributes = new Dictionary<AttributeType, Attribute>()
            {
                {AttributeType.Strength, attributes[AttributeType.Strength]},
                {AttributeType.Dexterity, attributes[AttributeType.Dexterity]},
                {AttributeType.Constitution, attributes[AttributeType.Constitution]},
                {AttributeType.Wisdom, attributes[AttributeType.Wisdom]},
                {AttributeType.Charisma, attributes[AttributeType.Charisma]},
                {AttributeType.Intelligence, attributes[AttributeType.Intelligence]},
                {AttributeType.Initiative, attributes[AttributeType.Initiative]},
                {AttributeType.Attack, attributes[AttributeType.Attack]},
                {AttributeType.Damage, attributes[AttributeType.Damage]},
            };
        }

        public void Equip(Item item)
        {
            foreach (Modifier mod in item.StatMods)
            {
                this.Attributes[mod.attType].AddModifier(mod);
            }
            inventory.Add(item); //TODO these should equip into equip slots
        }

        public void Unequip(Item item)
        {
            foreach (Modifier mod in item.StatMods)
            {
                this.Attributes[mod.attType].RemoveAllModifiersFromSource(item);
            }

            inventory.Remove(item); //TODO these should unequip from equip slots
        }

        // public void CalculateSecondaryStats() {
        //     // primary attributes => secondary attributes
        //     // reconsider items
        //     // reconsider abilities
        // }

        public void Attack(Creature target) {
            System.Random rand = new System.Random();
        }

        override public string ToString() {
            string result = "";
            result += "id: " + this.id + "\n";
            result += "name: " + this.name + "\n";
            result += "attributes:\n";
            foreach(KeyValuePair<AttributeType, Attribute> entry in this.Attributes) {
                result += entry.Key.ToString() + ": " + entry.Value.ToString() + "\n";
            }
            
            result +="inventory:\n";
            foreach(Item item in this.inventory) {
                result += item.ToString() + "\n";
            }
            return result;
            
        }
    }
}

