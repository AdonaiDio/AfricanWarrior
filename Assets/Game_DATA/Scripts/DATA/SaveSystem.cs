using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savegame.save";
        FileStream file = new FileStream(path, FileMode.Create);

        PlayerGameDATA data = new PlayerGameDATA();

        formatter.Serialize(file, data);
    }
}
