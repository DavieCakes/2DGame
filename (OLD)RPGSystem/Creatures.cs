using System.Collections.Generic;

using Items;
using Inventories;
using Attributes;
        public struct CreatureAttributes {
            public Attribute Strength;
            public Attribute Dexterity;
            public Attribute Constitution;
            public Attribute Wisdom;
            public Attribute Charisma;
            public Attribute Intelligence;
            public Attribute Initiative;
            public Attribute Attack;
            public Attribute Damage;
            public Attribute Health;
            public Attribute AC;

            public CreatureAttributes (
                int strength,
                int dexterity,
                int constitution,
                int wisdom,
                int charisma,
                int intelligence,
                int initiative,
                int attack,
                int damage,
                int health,
                int ac ) {
                this.Strength = new Attribute(strength);
                this.Dexterity = new Attribute(dexterity);
                this.Constitution = new Attribute(constitution);
                this.Wisdom = new Attribute(wisdom);
                this.Charisma = new Attribute(charisma);
                this.Intelligence = new Attribute(intelligence);
                this.Initiative = new Attribute(initiative);
                this.Attack = new Attribute(attack);
                this.Damage = new Attribute(damage);
                this.Health = new Attribute(health);
                this.AC = new Attribute(ac);
            }

            public CreatureAttributes(Dictionary<AttributeType, Attribute> _attributes) {
                
                this.Strength = _attributes[AttributeType.STRENGTH];
                this.Dexterity = _attributes[AttributeType.DEXTERITY];
                this.Constitution = _attributes[AttributeType.CONSTITUTION];
                this.Wisdom = _attributes[AttributeType.WISDOM];
                this.Charisma = _attributes[AttributeType.CHARISMA];
                this.Intelligence = _attributes[AttributeType.INTELLIGENCE];
                this.Initiative = _attributes[AttributeType.INITIATIVE];
                this.Attack = _attributes[AttributeType.ATTACK];
                this.Damage = _attributes[AttributeType.DAMAGE];
                this.AC = _attributes[AttributeType.AC];
                this.Health = _attributes[AttributeType.HEALTH];
            }
        }

namespace Creatures {
    public class Creature {
        public long id;
        public string name;
        // public List<Attribute> Attributes;
        public Dictionary<AttributeType, Attribute> attributesHash;
        public CreatureAttributes attributes;
        // creature.Attributes.Strength = ...

        // private _Attributes a = new _Attributes();
        public Inventory inventory;
        public Inventory equipped;

        public Creature() {
            this.inventory = new Inventory();
            this.attributes = new CreatureAttributes(10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10);
            InitAttributeHash();
        }

        public Creature(Dictionary<AttributeType, Attribute> _attributes, string name, long id) {
            this.name = name;
            this.id = id;
            inventory = new Inventory();

            // attribute enforcement
            this.attributes = new CreatureAttributes (_attributes);

            InitAttributeHash();
        }

        private void InitAttributeHash() {
            this.attributesHash = new Dictionary<AttributeType, Attribute>()
            {
                {AttributeType.STRENGTH, this.attributes.Strength},
                {AttributeType.DEXTERITY, this.attributes.Dexterity},
                {AttributeType.CONSTITUTION, this.attributes.Constitution},
                {AttributeType.WISDOM, this.attributes.Wisdom},
                {AttributeType.CHARISMA, this.attributes.Charisma},
                {AttributeType.INTELLIGENCE, this.attributes.Intelligence},
                {AttributeType.INITIATIVE, this.attributes.Initiative},
                {AttributeType.ATTACK, this.attributes.Attack},
                {AttributeType.DAMAGE, this.attributes.Damage},
                {AttributeType.HEALTH, this.attributes.Health}
            };
        }

        public void PickUp(Item item) {
            inventory.Add(item);
        }
        
        public void Equip(Item item)
        {
            foreach (Modifier mod in item.StatMods)
            {
                this.attributesHash[mod.attType].AddModifier(mod);
            }
            equipped.Add(item);
        }

        public void Unequip(Item item)
        {
            foreach (Modifier mod in item.StatMods)
            {
                this.attributesHash[mod.attType].RemoveAllModifiersFromSource(item);
            }

            equipped.Remove(item); 
            inventory.Add(item);
        }

        public bool EquipFromInventory(Item item) {
            if(!inventory.Contains(item)) {
                return false;
            }
                inventory.Remove(item);
                Equip(item);
                return true;
        }

        // public void CalculateSecondaryStats() {
        //     // primary attributes => secondary attributes
        //     // reconsider items
        //     // reconsider abilities
        // }

        public bool Attack(Creature target) {
            System.Random rand = new System.Random();
            int hitVal = rand.Next(1, 20) + (int)this.attributes.Attack.Value;
            if (hitVal < target.attributes.AC.Value) {
                return false;
            }
            target.TakeDamage(this.attributes.Damage.Value);
            return true;
        }

        public void TakeDamage(int value) {
            // TODO This could cause issues
            if (value < 1) {
                return;
            }
            this.attributes.Health.AddModifier(new Modifier(value * -1, ModifierType.Flat));
        }

        public void TakeDamage(float value) {
            // TODO This could cause issues
            if (value < 1) {
                return;
            }
            this.attributes.Health.AddModifier(new Modifier(value * -1, ModifierType.Flat));
        }

        public bool isDead() {
            return this.attributes.Health.Value <= 0.0f;
        }

        override public string ToString() {
            string result = "";
            result += "id: " + this.id + "\n";
            result += "name: " + this.name + "\n";
            result += "attributes:\n";
            foreach(KeyValuePair<AttributeType, Attribute> entry in this.attributesHash) {
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

