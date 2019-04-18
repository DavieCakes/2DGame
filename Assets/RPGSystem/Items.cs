using PlayerAbilities;
using System.Collections.Generic;

namespace Items {

    public abstract class Item {
        public readonly string name;
        public readonly ItemType itemType;

        public Item (string name, ItemType itemType) {
            this.name = name;
            this.itemType = itemType;
        }

        override
        public abstract string ToString();

        public string GetItemName() {
            return name;
        }


    }

    public class Gold : Item {
        public int amount {
            get {
                return amount;
            }
            // currently only settable in constructor
            private set {
                amount = value;
            }
        }
        public Gold(int amount) : base ("gold", ItemType.GOLD) {
            this.amount = amount;
        }

        public override string ToString()
        {
            return "Gold Cache, contains " + this.amount + " gold.\n";
        }
    }

    public class Key : Item {
        public Key() : base ("key", ItemType.KEY) {}

        public override string ToString()
        {
            return this.GetItemName() + "\n";
        }
    }

    public class HealthPotion : Item {
       public HealthPotion() : base ("Health Potion", ItemType.HEALTHPOTION) {}

       public int healAmount = 5;

        public override string ToString()
        {
            return this.GetItemName() + " +" + this.healAmount.ToString() + "\n";
        }
    }

    public enum ItemType {
        KEY,
        HEALTHPOTION,
        EQUIPMENT,
        GOLD
    }

    public enum EquipSlot {
        WEAPON,
        ARMOR,
        BOOTS,
        HELMET

    }
    public class Equipment : Item
    { 
        public readonly List<Modifier> StatMods;
        public EquipSlot equipSlot;

        public Equipment(IEnumerable<Modifier> StatMods, string name, EquipSlot equipSlot) : base (name, ItemType.EQUIPMENT) {
            this.StatMods = new List<Modifier>();
            this.equipSlot = equipSlot;
            foreach (Modifier mod in StatMods) {
                this.StatMods.Add(mod);
            }
        }
        public Equipment(Modifier StatMod, string name, EquipSlot equipSlot) : base (name, ItemType.EQUIPMENT) {
            this.StatMods = new List<Modifier>();
            this.equipSlot = equipSlot;
            this.StatMods.Add(StatMod);
        }

        public Equipment(string name, EquipSlot equipSlot) : base (name, ItemType.EQUIPMENT) {
            this.StatMods = new List<Modifier>();
            this.equipSlot = equipSlot;
        }

        public void AddStatMod(Modifier StatMod) {
            this.StatMods.Add(StatMod);
        }

        override
        public string ToString() {
            string result = "";
            result += "name: " + this.name + "\n";
            result += "Modifiers: \n";
            foreach(Modifier mod in StatMods) {
                if(mod.Value > 0) {
                    result += "+";
                }
                result += mod.Value + " (" + mod.Type.ToString() + ") " + mod.attType.ToString() + "\n";
            }
            return result;
        }
    }
}