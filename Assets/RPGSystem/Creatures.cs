using System.Collections.Generic;

using Items;
using Inventories;
using Attributes;


namespace Creatures {
    public class Creature {
        public long id;
        public string name;
        // public List<Attribute> Attributes;
        public Dictionary<AttributeType, Attribute> attributes;
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
            this.attributes = new Dictionary<AttributeType, Attribute>()
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

        public Creature(Dictionary<AttributeType, Attribute> _attributes, string name, long id) {
            this.name = name;
            this.id = id;
            inventory = new Inventory();

            // attribute enforcement
            this.attributes = new Dictionary<AttributeType, Attribute>()
            {
                {AttributeType.Strength, _attributes[AttributeType.Strength]},
                {AttributeType.Dexterity, _attributes[AttributeType.Dexterity]},
                {AttributeType.Constitution, _attributes[AttributeType.Constitution]},
                {AttributeType.Wisdom, _attributes[AttributeType.Wisdom]},
                {AttributeType.Charisma, _attributes[AttributeType.Charisma]},
                {AttributeType.Intelligence, _attributes[AttributeType.Intelligence]},
                {AttributeType.Initiative, _attributes[AttributeType.Initiative]},
                {AttributeType.Attack, _attributes[AttributeType.Attack]},
                {AttributeType.Damage, _attributes[AttributeType.Damage]},
                {AttributeType.Health, _attributes[AttributeType.Health]},
            };
        }

        public void Equip(Item item)
        {
            foreach (Modifier mod in item.StatMods)
            {
                this.attributes[mod.attType].AddModifier(mod);
            }
            inventory.Add(item); //TODO these should equip into equip slots
        }

        public void Unequip(Item item)
        {
            foreach (Modifier mod in item.StatMods)
            {
                this.attributes[mod.attType].RemoveAllModifiersFromSource(item);
            }

            inventory.Remove(item); //TODO these should unequip from equip slots
        }

        // public void CalculateSecondaryStats() {
        //     // primary attributes => secondary attributes
        //     // reconsider items
        //     // reconsider abilities
        // }

        public bool Attack(Creature target) {
            System.Random rand = new System.Random();
            int hitVal = rand.Next(1, 20) + (int)this.attributes[AttributeType.Attack].Value;
            if (hitVal < target.attributes[AttributeType.AC].Value) {
                return false;
            }
            target.attributes[AttributeType.Health].AddModifier(new Modifier((this.attributes[AttributeType.Damage].Value * -1), ModifierType.Flat));
            return true;
        }

        public void TakeDamage(int value) {
            // TODO This could cause issues
            if (value < 1) {
                return;
            }
            this.attributes[AttributeType.Health].AddModifier(new Modifier(value * -1, ModifierType.Flat));
        }

        public bool isDead() {
            return this.attributes[AttributeType.Health].Value <= 0.0f;
        }

        override public string ToString() {
            string result = "";
            result += "id: " + this.id + "\n";
            result += "name: " + this.name + "\n";
            result += "attributes:\n";
            foreach(KeyValuePair<AttributeType, Attribute> entry in this.attributes) {
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

