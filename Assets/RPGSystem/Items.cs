using PlayerAbilities;
using System.Collections.Generic;

namespace Items {

    public abstract class Item {
        public readonly string name;
        public readonly string iconName;
        public readonly ItemType itemType;

        public Item (string name, string iconName,  ItemType itemType) {
            this.iconName = iconName;
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
        public Gold(int amount) : base ("Gold", "gold", ItemType.GOLD) {
            this.amount = amount;
        }

        public override string ToString()
        {
            return "Gold Cache, contains " + this.amount + " gold.\n";
        }
    }

    public class Key : Item {
        public Key() : base ("Key", "key", ItemType.KEY) {}

        public override string ToString()
        {
            return this.GetItemName() + "\n";
        }
    }

    public class HealthPotion : Item {
       public HealthPotion() : base ("Health Potion", "health_potion", ItemType.HEALTHPOTION) {}

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
        public readonly List<Modifier> modifiers;
        public EquipSlot equipSlot;

        public Equipment(IEnumerable<Modifier> StatMods, string name, string iconName,  EquipSlot equipSlot) : base (name, iconName,  ItemType.EQUIPMENT) {
            this.modifiers = new List<Modifier>();
            this.equipSlot = equipSlot;
            foreach (Modifier mod in StatMods) {
                this.modifiers.Add(mod);
            }
        }
        public Equipment(Modifier StatMod, string name, string iconName,  EquipSlot equipSlot) : base (name, iconName, ItemType.EQUIPMENT) {
            this.modifiers = new List<Modifier>();
            this.equipSlot = equipSlot;
            this.modifiers.Add(StatMod);
        }

        public Equipment(string name, string iconName,  EquipSlot equipSlot) : base (name, iconName, ItemType.EQUIPMENT) {
            this.modifiers = new List<Modifier>();
            this.equipSlot = equipSlot;
        }

        public void AddStatMod(Modifier StatMod) {
            this.modifiers.Add(StatMod);
        }
// Assets/_Alpha/Textures/34x34RPGIcons.png
        override
        public string ToString() {
            string result = "";
            result += this.equipSlot.ToString().ToLower() + "\n" + this.name;
            // result += "Modifiers: \n";
            foreach(Modifier mod in modifiers) {
                if(mod.Value > 0) {
                    result += "\n+";
                }
                result += mod.Value + " " + mod.attType.ToString().ToLower() + "\n";
            }
            return result;
        }
    }
}