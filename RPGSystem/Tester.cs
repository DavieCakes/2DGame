using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Creatures;
using Items;
using Attributes;
using Databases;
using Builders;

public class Tester : MonoBehaviour
{
    Database data;
    // Start is called before the first frame update
    void Start()
    {
        
        // TestDataInsertCreature();
        // TestDataInsertItem();
        // TestGetItemData();
        // TestGetCreatureData();
        // TestBuildItem();
        TestBuildCreature();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TestBuildCreature() {
        Builder builder = new Builder();
        Creature creature = builder.BuildCreature("Warrior Drow");
        Debug.Log(creature.ToString());
    }

    void TestBuildItem() {
        Builder builder = new Builder();
        Item item = builder.BuildItem("IceSword");
        Debug.Log(item.ToString());
    }

    void TestGetItemData() {
        data = new Database();
        Dictionary<string, object> item_data = data.GetItemData("IceSword");
    }

    void TestGetCreatureData() {
        data = new Database();
        Dictionary<string, object> creature_data = data.GetCreatureData("Greater Kobold");
        foreach( KeyValuePair<string, object> row in creature_data) {
            Debug.Log(row.Key);
        }
    }

    void Test() {
        // Creature c;
        // Item i;
        // Attribute a;
        // Modifier m;
        // data.LogRow("*", "items", "name" , "sword");
    }

    void TestDataInsertItem() {
        data = new Database();
        Dictionary<string, int> itemModifiers = new Dictionary<string, int>()
        {
            {"damage", 5},
            {"constitution", -1}
        };
        Dictionary<string, object> itemData = new Dictionary<string, object>()
        {
            {"name", "IceSword"},
            {"modifiers", itemModifiers}
        };

        data.InsertItem(itemData);
    }

    void TestDataInsertCreature() {
        data = new Database();
        data.LogRow("*", "items", "name" , "sword");
        Debug.Log("out");
        Dictionary<string, int> enemy_attributes = new Dictionary<string, int>()
        {
            {"Strength", 10},
            {"Dexterity", 10},
            {"Constitution", 10},
            {"Wisdom", 10},
            {"Charisma", 10},
            {"Intelligence", 10},
            {"Initiative", 10},
            {"ToHit", 10},
            {"Damage", 10},
        };
        List<string> enemy_items = new List<string>() {
            // "whip",
            // "Chalice of Souleating",
            "IceSword"
        };
        Dictionary<string, object> enemy_data = new Dictionary<string, object>()
        {
            {"name", "Greater Kobold"},
            {"attributes", enemy_attributes},
            {"inventory", enemy_items}

        };
        data.InsertCreature(enemy_data);
    }
}
