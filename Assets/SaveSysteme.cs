using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSysteme
{
    public static void SaveData(GameManager manager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Player.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        Data data = new Data(manager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Data Load_Data()
    {
        string path = Application.persistentDataPath + "/Player.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Data data = formatter.Deserialize(stream) as Data;
            stream.Close();

            return data;
        }

        return null;
    }
    public static void Reset()
    {
        string path = Application.persistentDataPath + "/Player.dat";
        if (File.Exists(path))
        {
            File.Delete(path);
            PlayerPrefs.DeleteKey("First");
        }
    }
}
