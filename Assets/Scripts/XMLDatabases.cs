using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

using Items;

// path => Path.Combine(Application.DataPath, "RPGSystem/Data.xml")
namespace Databases {
    public class Database {

        private static string path = Application.dataPath + "/Scripts/Data.xml";

        public Dictionary<string, object> GetItemData(string itemName) {
            Dictionary<string, object> itemData = new Dictionary<string, object>();
            Dictionary <string, long> itemModifiers = new Dictionary<string, long>();
            XmlDocument doc = new XmlDocument();
            doc.Load(Database.path);
            XmlNode itemNode = doc.SelectSingleNode("Data/Items//Item[@name = '" + itemName + "']");

            foreach(XmlNode node in itemNode.SelectNodes("Modifier")) {
                itemModifiers.Add(node.Attributes["name"].Value, Int64.Parse(node.Attributes["value"].Value));
            }

            itemData.Add("display_name", itemName);
            itemData.Add("type", itemNode.Attributes["type"].Value);
            itemData.Add("name", itemNode.Attributes["name"].Value);
            if ((string)itemData["type"] == "equipment") {
                itemData.Add("slot", itemNode.Attributes["slot"].Value);
            }
            itemData.Add("modifiers", itemModifiers);

            return itemData;
        }

        public List<Dictionary<string, object>> GetAllEquipmentData() {
            List<Dictionary<string, object>> itemList = new List<Dictionary<string, object>>();
            XmlDocument doc = new XmlDocument();
            doc.Load(Database.path);
            XmlNodeList itemNodes = doc.SelectNodes("Data/Items//Item[@type = 'equipment']");
            foreach (XmlNode node in itemNodes) {
                itemList.Add(GetItemData(node.Attributes["name"].Value));
            }
            return itemList;
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