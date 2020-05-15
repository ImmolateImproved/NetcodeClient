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

    public static void Connect()
    {
        var path = Path.Combine("URI=file:" + Application.dataPath, "mydb.db");
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
        //DataBase.ExecuteQueryWithoutAnswer("INSERT INTO friends(id,nick) VALUES('25','Dead');");

        //var data = DataBase.GetTable("SELECT * FROM friends;");

        //var id = (data.Rows[0][1].ToString());

        //FriendsDataBaseAccessor.AddFriend(new UserData { id = 1, nick = "Rembo" });
        //FriendsDataBaseAccessor.AddFriend(new UserData { id = 2, nick = "QWEr" });
        //FriendsDataBaseAccessor.AddFriend(new UserData { id = 3, nick = "Shotaketa" });

        FriendsTableAccessor.GetFriendsList();
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