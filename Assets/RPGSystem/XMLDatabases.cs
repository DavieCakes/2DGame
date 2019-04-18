using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

using Items;

// path => Path.Combine(Application.DataPath, "RPGSystem/Data.xml")
namespace Databases {
    public class Database {

        // private static System.IO.Stream path =  new System.IO.Stream();
        private static string path = Application.dataPath + "/RPGSystem/Data.xml";

        // private static string path = "Data.xml";

        public bool WriteData(string data) {
            XmlDocument doc = new XmlDocument();
            doc.Load(Database.path);
            // Console.WriteLine(doc.DocumentElement.SelectSingleNode("Creatures").SelectSingleNode("Creature").OuterXml);
            // XmlNode match = doc.SelectSingleNode("Creatures");
            // match = match.FirstChild;
            // Console.WriteLine(match.ChildNodes.Count);
            XmlNodeList creatures = doc.SelectNodes("Data/Creatures/Creature");
            foreach (XmlNode node in creatures) {
                Console.Write(node.Attributes["name"].Value);
            }
            // Console.Write(val);
            return true;
        }

        public Dictionary<string, object> GetCreatureData(string creatureName) {
            XmlDocument doc = new XmlDocument();
            doc.Load(Database.path);
            Dictionary<string, object> creatureData = new Dictionary<string, object>();
            List<string> inventory = new List<string>();
            // string path = 
            XmlNode creatureNode = doc.SelectSingleNode("Data/Creatures//Creature[@name = '" + creatureName + "']");
 
            Dictionary<string, long> creatureAttributes = new Dictionary<string, long>();
            // Console.WriteLine(creatureNode.Attributes["name"].Value);

            foreach(XmlNode node in creatureNode.SelectNodes("Attribute")) {
                // Console.WriteLine(node.Attributes["name"].Value);
                creatureAttributes.Add(node.Attributes["name"].Value, Int64.Parse(node.Attributes["value"].Value));
            }

            foreach(XmlNode node in creatureNode.SelectNodes("Inventory/Item")) {
                inventory.Add(node.Attributes["name"].Value);
                // Debug.Log(node.Attributes["name"].Value);
            }

            creatureData.Add("name", creatureName);
            // creatureData.Add("id", Int64.Parse(creatureNode.Attributes["id"].Value));
            creatureData.Add("attributes", creatureAttributes);
            creatureData.Add("inventory", inventory);
 
            return creatureData;
        }

        public Dictionary<string, object> GetItemData(string itemName) {
            Dictionary<string, object> itemData = new Dictionary<string, object>();
            Dictionary <string, long> itemModifiers = new Dictionary<string, long>();
            XmlDocument doc = new XmlDocument();
            doc.Load(Database.path);
            // Debug.Log(doc.InnerXml);
            XmlNode itemNode = doc.SelectSingleNode("Data/Items//Item[@name = '" + itemName + "']");

            // Debug.Log(itemName);
            // Debug.Log(itemNode.Attributes["id"].Value);
            foreach(XmlNode node in itemNode.SelectNodes("Modifier")) {
                itemModifiers.Add(node.Attributes["name"].Value, Int64.Parse(node.Attributes["value"].Value));
            }

            itemData.Add("name", itemName);
            itemData.Add("type", itemNode.Attributes["type"].Value);
            if ((string)itemData["type"] == "equipment") {
                itemData.Add("slot", itemNode.Attributes["slot"].Value);
            }
            // itemData.Add("id", Int64.Parse(itemNode.Attributes["id"].Value));
            itemData.Add("modifiers", itemModifiers);

            // doc = null;
            return itemData;
        }

        public Dictionary<string, object> GetRandomEquipmentData(System.Random rand) {
            Dictionary<string, object> itemData = new Dictionary<string, object>();
            Dictionary <string, long> itemModifiers = new Dictionary<string, long>();
            XmlDocument doc = new XmlDocument();
            int randIndex;

            doc.Load(Database.path);
            XmlNodeList itemNodes = doc.SelectNodes("Data/Items//Item[@type = '" + Items.ItemType.EQUIPMENT.ToString().ToLower() + "']");
            randIndex = rand.Next(0, itemNodes.Count - 1);
            XmlNode selectedNode = itemNodes.Item(randIndex);
            return GetItemData(selectedNode.Attributes["name"].Value);
        }
    }
}