using System.Collections;
using System.Collections.Generic;
// using UnityEngine;

using Models;
using Items;
using PlayerAbilities;
using Databases;
using Builders;

public class Tester
{
    // Start is called before the first frame update
    static Database data;
    static void Main()
    {
        // TestDataInsertCreature();
        // TestDataInsertItem();
        // TestGetItemData();
        // TestGetCreatureData();
        // TestBuildItem();
        // TestBuildCreature();
        data = new Database();
        TestBuildRandomDrop();
    }

    // Update is called once per frame
    void Update()
    {
    }

    static void TestBuildRandomDrop() {
        Builder b = new Builder();
        Database data = new Database();
        List<Item> items = Builder.BuildRandomItemDrop();
        foreach (Item item in items) {
            System.Console.Write(item.ToString());
        }
    }

    // void TestBuildCreature() {
    //     Builder builder = new Builder();
    //     Creatures.Creature creature = Builder.BuildCreature("Warrior Drow");
    //     // Debug.Log(creature.ToString());
    // }

    void TestBuildItem() {
        Builder builder = new Builder();
        Items.Equipment item = Builder.BuildEquipment("IceSword");
        // Debug.Log(item.ToString());
    }

    void TestGetItemData() {
        data = new Database();
        Dictionary<string, object> item_data = data.GetItemData("IceSword");
    }

    void TestGetCreatureData() {
        data = new Database();
        Dictionary<string, object> creature_data = data.GetCreatureData("Greater Kobold");
        foreach( KeyValuePair<string, object> row in creature_data) {
            // Debug.Log(row.Key);
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

        // data.InsertItem(itemData);
    }

    void TestDataInsertCreature() {
        data = new Database();
        // data.LogRow("*", "items", "name" , "sword");
        // Debug.Log("out");
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
        // data.InsertCreature(enemy_data);
    }
}
