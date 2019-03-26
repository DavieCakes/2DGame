using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// These are needed for sqlite
using Mono.Data.Sqlite;
// using System.Data.Sqlite;
using System.Data;
using System;

// TODO GetEnemy(string enemyname)
namespace Databases {
    public class Database
    {

        // connection URIs
        private Dictionary <string, string> paths = new Dictionary<string, string>();

        // connection objects
        private IDbConnection Conn = (IDbConnection) new SqliteConnection("URI=file:" + Application.dataPath + "/Databases/test.db");

        // connection commanders
        // these execute operations through connection objects

        public Database() {

        }

        public Dictionary<string, object> GetItemData(string item_name) {
            
            IDbCommand command = Conn.CreateCommand();
            IDataReader reader;
            Dictionary<string, object> item_data = new Dictionary<string, object>();
            Dictionary<string, long> item_modifiers = new Dictionary<string, long>();
            long item_id;
            Conn.Open();

            command.CommandText = "SELECT id FROM Items WHERE name = '" + item_name + "';";
            item_id = (long)command.ExecuteScalar();

            // Debug.Log(item_id);

            command.CommandText = "SELECT name, value FROM Item_Modifiers WHERE fk_item = '" + item_id + "';";
            reader = command.ExecuteReader();
            while(reader.Read()) {
                item_modifiers.Add((string)reader.GetValue(0), (long)reader.GetValue(1));
            }

            item_data.Add("name", item_name);
            item_data.Add("id", item_id);
            item_data.Add("modifiers", item_modifiers);

            reader.Close();
            reader = null;
            command.Dispose();
            command = null;
            Conn.Close();

            return item_data;
        }

        public Dictionary<string, object> GetItemData(long item_id) {
            
            IDbCommand command = Conn.CreateCommand();
            IDataReader reader;
            Dictionary<string, object> item_data = new Dictionary<string, object>();
            Dictionary<string, long> item_modifiers = new Dictionary<string, long>();
            string item_name;
            Conn.Open();

            command.CommandText = "SELECT name FROM Items WHERE id = '" + item_id + "';";
            item_name = (string)command.ExecuteScalar();
            // Debug.Log(item_name);

            // Debug.Log(item_id);

            command.CommandText = "SELECT name, value FROM Item_Modifiers WHERE fk_item = '" + item_id + "';";
            reader = command.ExecuteReader();
            while(reader.Read()) {
                item_modifiers.Add((string)reader.GetValue(0), (long)reader.GetValue(1));
            }

            item_data.Add("name", item_name);
            item_data.Add("id", item_id);
            item_data.Add("modifiers", item_modifiers);

            reader.Close();
            reader = null;
            command.Dispose();
            command = null;
            Conn.Close();

            return item_data;
        }

        public Dictionary<string, object> GetCreatureData(string name) {
            IDbCommand command = Conn.CreateCommand();
            IDataReader reader;
            Dictionary<string, object> creature_data = new Dictionary<string, object>();
            Dictionary<string, long> creature_attributes = new Dictionary<string, long>();
            List<object> inventory = new List<object>();
            long creature_id;
            List<long> item_ids = new List<long>();
            List<string> item_names = new List<string>();
            Conn.Open();

            command.CommandText = "SELECT id FROM Creatures WHERE name = '" + name + "';";
            creature_id = (long)command.ExecuteScalar();

            command.CommandText = "SELECT name, value FROM Creature_Attributes WHERE fk_creature = '" + creature_id + "';";
            reader = command.ExecuteReader();
            while(reader.Read()) {
                creature_attributes.Add((string)reader.GetValue(0), (long)reader.GetValue(1));
            }
            reader.Close();

            command.CommandText = "SELECT fk_item FROM Creature_Items WHERE fk_creature = '" + creature_id + "';";
            reader = command.ExecuteReader();
            while(reader.Read()) {
                item_ids.Add((long)reader.GetValue(0));
            }
            reader.Close();

            Conn.Close();
            foreach(int id in item_ids) {
                inventory.Add(GetItemData(id));
            }
            creature_data.Add("name", name);
            creature_data.Add("id", creature_id);
            creature_data.Add("attributes", creature_attributes);
            creature_data.Add("inventory", inventory);
            creature_data.Add("item_ids", item_ids);

            // reader.Close();
            command.Dispose();
            // reader = null;
            command = null;
            Conn.Close();

            return creature_data;
        }
        
        public void LogRow(string column, string table, string fieldToCompare, string value) {
            Conn.Open();
            IDbCommand command = Conn.CreateCommand();
            command.CommandText = "select " + column + " from " + table + " WHERE " + fieldToCompare + " = '" + value + "';";
            IDataReader reader = command.ExecuteReader();
            List<string> values = new List<string>();
            while(reader.Read()) {
                for (int i = 0; i < reader.FieldCount; i++) {
                    // values.Add(reader.GetString(i));
                    
                    Debug.Log(reader.GetName(i) + " : " + reader.GetValue(i));
                }
            }

            reader.Close();
            reader = null;
            command.Dispose();
            command = null;
            Conn.Close();

            // return reader.Read();
        }

        public void InsertCreature(Dictionary<string, object> creatureData) {
            long creature_id;
            List<string> creature_inventory = (List<string>) creatureData["inventory"];
            List<long> item_ids = GetItemIdList(creature_inventory);
            Conn.Open();
            IDbCommand command = Conn.CreateCommand();
            
            if (item_ids.Count != creature_inventory.Count) {
                Debug.Log("One or more Items not found in item table, Items must be inserted before linked with a creature");
                return;
            }

            command.CommandText = "INSERT INTO Creatures ('name') VALUES ('" + (string)creatureData["name"] + "');";
            command.ExecuteNonQuery();
            command.CommandText = "SELECT last_insert_rowid();";
            creature_id = (long)command.ExecuteScalar();
            foreach(KeyValuePair<string, int> entry in (Dictionary<string, int>)creatureData["attributes"]) {
                command.CommandText = "INSERT INTO Creature_Attributes (name, value, fk_creature) " +
                    "VALUES ('" + entry.Key + "', '" + entry.Value + "', '" + creature_id + "')";
                command.ExecuteNonQuery();
            }
            
            foreach(int item in item_ids) {
                command.CommandText = "INSERT INTO Creature_Items (fk_creature, fk_item) " +
                    "VALUES ('" + creature_id + "', '" + item + "')";
                command.ExecuteNonQuery();
            }

            command.Dispose();
            command = null;
            Conn.Close();
        }

        public void InsertItem(Dictionary<string, object> itemData) {
            long item_id;

            Conn.Open();
            IDbCommand command = Conn.CreateCommand();

            // insert into ITEMS, get id back

            command.CommandText = "INSERT INTO Items (name) VALUES ('" + (string)itemData["name"] + "');";
            command.ExecuteNonQuery();
            command.CommandText = "SELECT last_insert_rowid();";
            item_id = (long)command.ExecuteScalar();

            foreach(KeyValuePair<string, int> entry in (Dictionary<string, int>)itemData["modifiers"]) {
                command.CommandText = "INSERT INTO Item_Modifiers (fk_item, name, value) VALUES ('"
                    + item_id + "', '" + entry.Key + "', '" + entry.Value + "');";
                command.ExecuteNonQuery();
            }

            command.Dispose();
            command = null;
            Conn.Close();
            
        }

        private List<long> GetItemIdList(List<string> items) {
            Conn.Open();
            IDbCommand command = Conn.CreateCommand();
            IDataReader reader;
            List <long> itemIds = new List<long>();
            foreach(string item in items) {
                command.CommandText = "SELECT (id) FROM Items WHERE name = '" + item + "';";
                reader = command.ExecuteReader();
                reader.Read();
                try {
                    itemIds.Add((long)reader.GetValue(0));
                    // Debug.Log(reader.GetValue(0).GetType());
                } catch (Exception e) {
                    Debug.Log(e.Message);
                    return new List<long>();
                }
                reader.Close();
            }

            command.Dispose();
            command = null;
            Conn.Close();
            
            return itemIds;
        }

        // SELECT "column" FROM "table" WHERE condition;
        // condition => name="vorpal_sword"
    }
}