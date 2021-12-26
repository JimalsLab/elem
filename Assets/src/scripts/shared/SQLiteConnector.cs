using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

string connection;
public class SQLiteConnector{
    public SQLiteConnector(){
        connection = "URI=file:" + Application.persistentDataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
    }
}
