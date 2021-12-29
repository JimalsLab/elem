using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
public class SQLiteConnector{

    string connection;
    public SQLiteConnector(){
        connection = "URI=file:" + Application.persistentDataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
    }
}
