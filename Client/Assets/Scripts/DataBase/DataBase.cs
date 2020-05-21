using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SQLite;
using System.Data;
using System.IO;

public class DataBase : MonoBehaviour
{
    private static SQLiteConnection connection;
    private static SQLiteCommand command;

    private static string dbName;

    private const string originalDBName = "mydb.db";

    public static void Init(string dbName)
    {
        DataBase.dbName = dbName;
        var path = Path.Combine(Application.persistentDataPath, dbName + ".db");

        if (!File.Exists(path))
        {
            var pathToOriginalBD = Path.Combine(Application.persistentDataPath, originalDBName);
            File.Copy(pathToOriginalBD, path);
        }
    }

    public static void Connect()
    {
        var path = Path.Combine("URI=file:" + Application.persistentDataPath, dbName + ".db");

        connection = new SQLiteConnection(path);
        command = new SQLiteCommand(connection);
        connection.Open();
    }

    public static void CloseConnection()
    {
        connection.Close();
        command.Dispose();
    }

    public static void ExecuteQueryWithoutAnswer(string query)
    {
        Connect();
        command.CommandText = query;
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static string ExecuteQueryWithAnswer(string query)
    {
        Connect();
        command.CommandText = query;
        var answer = command.ExecuteScalar();
        CloseConnection();

        if (answer != null) return answer.ToString();
        else return null;
    }

    public static DataTable GetTable(string query)
    {
        Connect();

        SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);

        DataSet DS = new DataSet();
        adapter.Fill(DS);
        adapter.Dispose();

        CloseConnection();

        return DS.Tables[0];
    }

    public void Perform()
    {
        FriendsTableAccessor.AddFriend(new UserData { id = 4, nick = "Unnamed" });
    }
}

public static class FriendsTableAccessor
{
    public static void AddFriend(UserData userData)
    {
        DataBase.ExecuteQueryWithoutAnswer($"INSERT INTO friends(id,nick) VALUES('{userData.id}','{userData.nick}');");
    }

    public static List<UserData> GetFriendsList()
    {
        var data = DataBase.GetTable("SELECT * FROM friends;").Rows;

        var userDatas = new List<UserData>(data.Count);

        for (int i = 0; i < data.Count; i++)
        {
            var userData = new UserData
            {
                id = int.Parse(data[i][0].ToString()),
                nick = data[i][1].ToString()
            };

            userDatas.Add(userData);
        }

        return userDatas;
    }
}