using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer (PlayerTopDown player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path =  Application.persistentDataPath + "/player.jakol";
        FileStream stream = new FileStream (path,FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.jakol";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter(); 
            FileStream stream = new FileStream (path,FileMode.Open);

            PlayerData data = formatter.Deserialize (stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("not found in" + path);
            return null;
        }
    }
    

}
