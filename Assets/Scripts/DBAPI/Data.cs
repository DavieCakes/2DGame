using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// These are needed for sqlite
using Mono.Data.Sqlite;
// using System.Data.Sqlite;
using System.Data;
using System;

public class Data
{

    // connection URIs
    // these are used once, they represent the path to the appropriate database
    private Dictionary <string, string> paths = new Dictionary<string, string>();

    // connection objects
    // similar to sockets, they represent the open connection to the database
    private IDbConnection Conn;

    // connection commanders
    // these execute operations through connection objects

    public Data() {
        Conn = (IDbConnection) new SqliteConnection("URI=file:" + Application.dataPath + "/Databases/test.db");
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

    // static void Main() {
    //     Data d = new Data();
    // }

    // SELECT "column" FROM "table" WHERE condition;
    // condition => name="vorpal_sword"
}
