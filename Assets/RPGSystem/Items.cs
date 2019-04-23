using PlayerAbilities;
using System.Collections.Generic;

namespace Items {

    public abstract class Item {
        public readonly string displayName;
        public readonly string name;
        public readonly ItemType itemType;

        public Item (string displayName, string name, ItemType itemType) {
            this.name = name;
            this.displayName = displayName;
            this.itemType = itemType;
        }

        override
        public abstract string ToString();

        public string GetItemName() {
            return name;
        }


    }

    public class Gold : Item {
        public int amount;
        public Gold(int amount) : base ("Gold", "gold", ItemType.GOLD) {
            if (amount < 0) {
                throw new System.Exception("Gold can't be initialized with a negative value");
            }
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

        public Equipment(IEnumerable<Modifier> StatMods, string displayName, string name,  EquipSlot equipSlot) : base (displayName, name,  ItemType.EQUIPMENT) {
            this.modifiers = new List<Modifier>();
            this.equipSlot = equipSlot;
            foreach (Modifier mod in StatMods) {
                this.modifiers.Add(mod);
            }
        }
        public Equipment(Modifier StatMod, string displayName, string name,  EquipSlot equipSlot) : base (displayName, name, ItemType.EQUIPMENT) {
            this.modifiers = new List<Modifier>();
            this.equipSlot = equipSlot;
            this.modifiers.Add(StatMod);
        }

        public Equipment(string displayName, string name,  EquipSlot equipSlot) : base (displayName, name, ItemType.EQUIPMENT) {
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
            result += this.equipSlot.ToString().ToLower() + "\n" + this.displayName;
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