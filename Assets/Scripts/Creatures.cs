using System.Collections.Generic;
using UnityEngine;

using Items;
using Inventories;
using PlayerAbilities;

namespace Models
{
    public struct CreatureAbilities
    {
        public Ability Agility;
        public Ability Attack;
        public HealthAbility Health;
        public Ability Defense;

        public CreatureAbilities(
            int agility,
            int attack,
            int health,
            int defense)
        {

            this.Attack = new Ability(attack);
            this.Agility = new Ability(agility);
            this.Health = new HealthAbility(health);
            this.Defense = new Ability(defense);
        }

        public CreatureAbilities(Dictionary<AbilityType, Ability> _attributes)
        {
            this.Agility = _attributes[AbilityType.AGILITY];
            this.Attack = _attributes[AbilityType.ATTACK];
            this.Health = (HealthAbility)_attributes[AbilityType.HEALTH];
            this.Defense = _attributes[AbilityType.DEFENSE];
        }
    }

    public class PlayerModel : Creature
    {

    }

    public class EnemyModel : Creature
    {

    }

    public class Creature
    {
        public string name;
        public Dictionary<AbilityType, Ability> abilitiesHash;
        public CreatureAbilities abilities;
        public Inventory inventory;
        public Dictionary<EquipSlot, Items.Equipment> currentlyEquipped;

        public Creature()
        {
            this.inventory = new Inventory();
            this.currentlyEquipped = new Dictionary<EquipSlot, Items.Equipment>();
            this.abilities = new CreatureAbilities(10, 10, 10, 10);
            InitAttributeHash();
        }

        public Creature(Dictionary<AbilityType, Ability> _attributes, string name, long id)
        {
            this.name = name;
            this.inventory = new Inventory();
            this.currentlyEquipped = new Dictionary<EquipSlot, Items.Equipment>();
            this.abilities = new CreatureAbilities(_attributes);
            InitAttributeHash();
        }

        public Creature(int health, int agility, int defense, int attack, string name)
        {
            this.name = name;
            this.inventory = new Inventory();
            this.currentlyEquipped = new Dictionary<EquipSlot, Items.Equipment>();
            this.abilities = new CreatureAbilities(agility, attack, health, defense);
            InitAttributeHash();
        }

        private void InitAttributeHash()
        {
            this.abilitiesHash = new Dictionary<AbilityType, Ability>()
            {
                {AbilityType.AGILITY, this.abilities.Agility},
                {AbilityType.ATTACK, this.abilities.Attack},
                {AbilityType.HEALTH, this.abilities.Health},
                {AbilityType.DEFENSE, this.abilities.Defense},
            };
        }

        public void PickUp(Item item)
        {
            inventory.Add(item);
        }

        /// Equips an item into the currentlyEquipped dictionary,
        /// also adds the items ability modifiers to the creature
        // If equip slot is filled, currently equiped item goes into
        // the inventory, and the given item is equiped in it's place
        public void Equip(Items.Equipment item)
        {
            // Debug.Log("Equipping: " + item.ToString());
            foreach (Modifier mod in item.modifiers)
            {
                this.abilitiesHash[mod.attType].AddModifier(mod);
            }
            if (!this.currentlyEquipped.ContainsKey(item.equipSlot))
            {
                this.currentlyEquipped.Add(item.equipSlot, item);
            }
            else
            {
                Unequip(item.equipSlot);
                this.currentlyEquipped.Add(item.equipSlot, item);
            }
        }

        /*
            Unequips an item, unequiping also removes any
            ability bonuses that item has.
            Places item into inventory
         */
        public void Unequip(EquipSlot equipSlot)
        {
            if (this.currentlyEquipped.ContainsKey(equipSlot))
            {
                foreach (Modifier mod in currentlyEquipped[equipSlot].modifiers)
                {
                    this.abilitiesHash[mod.attType].RemoveAllModifiersFromSource(currentlyEquipped[equipSlot]);
                }
                inventory.Add(currentlyEquipped[equipSlot]);
                currentlyEquipped.Remove(equipSlot);
            }
        }

        /*
            Unequips an item, unequiping also removes any
            ability bonuses that item has.
            Places item into inventory
         */
        public void Unequip(Items.Equipment item)
        {
            foreach (Modifier mod in item.modifiers)
            {
                this.abilitiesHash[mod.attType].RemoveAllModifiersFromSource(item);
            }

            currentlyEquipped.Remove(item.equipSlot);
            inventory.Add(item);
        }

        /*
            Removes an Item from the inventory, and sends it to Equip()
         */
        public bool EquipFromInventory(Items.Equipment item)
        {
            if (!inventory.equipmentInventory.Contains(item))
            {
                return false;
            }
            inventory.Remove(item);
            Equip(item);
            return true;
        }

        public bool EquipFromInventory(string iconName)
        {
            foreach (Equipment equipment in this.inventory.equipmentInventory)
            {
                if (equipment.name.Equals(iconName))
                {
                    EquipFromInventory(equipment);
                    return true;
                }
            }
            return false;
        }

        /*
            Uses standard D20 attack roll to determine attack success,
            currently it is attack vs defense
         */
        public bool Attack(Creature target)
        {
            System.Random rand = new System.Random();
            int hitVal = rand.Next(1, 20) + (int)this.abilities.Attack.Value;
            if (hitVal < target.abilities.Defense.Value)
            {
                return false;
            }
            target.TakeDamage(this.abilities.Attack.Value);
            return true;
        }

        /*
            Applies damage to the creature,
            damage is a special case, it is added to
            HealthAbility.damageTaken. Look there for 
            more information
         */
        public void TakeDamage(int value)
        {
            this.abilities.Health.TakeDamage(value);
        }

        public bool isDead()
        {
            return this.abilities.Health.IsDead();
        }

        public bool TryHealthPotion()
        {
            if (this.inventory.potions > 0 && this.abilities.Health.Heal(5))
            {
                this.inventory.RemovePotion();
                Debug.Log("Using Potion");
                return true;
            }
            return false;
        }


        override public string ToString()
        {
            string result = "";
            result += "name: " + this.name + "\n";
            result += "attributes:\n";
            foreach (KeyValuePair<AbilityType, Ability> entry in this.abilitiesHash)
            {
                if (entry.Key == AbilityType.HEALTH)
                {
                    result += entry.Value.ToString() + "\n";
                }
                else
                {
                    result += entry.Key.ToString() + ": " + entry.Value.ToString() + "\n";
                }
            }
            result += this.inventory.potions;
            // result += "inventory:\n";
            // result += this.inventory.ToString();
            // result += currentlyEquipped.Count + " Items Currently Equipped\n";
            // foreach (KeyValuePair<EquipSlot, Equipment> entry in this.currentlyEquipped)
            // {
            //     result += entry.Key.ToString() + ": " + entry.Value.ToString() + "\n";
            // }
            return result;

        }
    }
}

