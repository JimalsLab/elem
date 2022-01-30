using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
public class SQLiteConnector{

    string connection;
    public IDbConnection dbcon;
    public string monsterTable;
    public SQLiteConnector(){
        monsterTable = "monsters";
        connection = "URI=file:" + Application.persistentDataPath + "/My_Database";
        dbcon = new SqliteConnection(connection);
        dbcon.Open();
        InitTables();
    }

    private void InitTables()
    {
        IDbCommand dbCommand = dbcon.CreateCommand();
        dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS " + monsterTable + "( " +
        "startinglevel" + " INTEGER, " +
        "endinglevel" + " INTEGER, " +
        "id" + " TEXT, " +
        "type" + " TEXT, " +
        "isactive" + " BOOLEAN )";

        dbCommand.ExecuteNonQuery();
    }

    public object ExecuteQuery(IDbCommand dbCommand)
    {
        return dbCommand.ExecuteScalar();
    }
}
