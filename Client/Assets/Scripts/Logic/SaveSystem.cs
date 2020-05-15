using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystem
{
    public static void Save<T>(T data, string fileName)
    {
        var formatter = new BinaryFormatter();

        var path = Application.persistentDataPath + $"/{fileName}.save";

        var stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static T? Load<T>(string fileName) where T : struct
    {
        var path = Application.persistentDataPath + $"/{fileName}.save";

        try
        {
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();

                using (var stream = new FileStream(path, FileMode.Open))
                {
                    if (stream.Length == 0)
                    {
                        return null;
                    }

                    var data = (T)formatter.Deserialize(stream);

                    return data;
                }
            }
            else
            {
                Debug.Log("File no found");
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return null;
        }
    }
}