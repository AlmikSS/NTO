using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

public static class SaveManager
{
    public static void Save(List<ItemData> invData, string path)
    {
        if (File.Exists(path + "Data.xml"))
            File.Delete(path + "Data.xml");

        XmlSerializer xml = new XmlSerializer(typeof(List<ItemData>));
        FileStream fs = new FileStream(path + "Data.xml", FileMode.CreateNew);
        xml.Serialize(fs, invData);
        fs.Close();
    }

    public static List<ItemData> Load(string path)
    {
        if (File.Exists(path + "Data.xml"))
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<ItemData>));
            FileStream fs = new FileStream(path + "Data.xml", FileMode.Open);
            List<ItemData> datas = (List<ItemData>)xml.Deserialize(fs);
            fs.Close();
            return datas;
        }
        return null;
    }

    public static void SaveScene(int currentScene, string path)
    {
        if (File.Exists(path + "Data.xml"))
            File.Delete(path + "Data.xml");

        XmlSerializer xml = new XmlSerializer(typeof(int));
        FileStream fs = new FileStream(path + "Data.xml", FileMode.Create);
        xml.Serialize(fs, currentScene);
        fs.Close();
    }

    public static int LoadScene(string path)
    {
        if (File.Exists(path + "Data.xml"))
        {
            XmlSerializer xml = new XmlSerializer(typeof(int));
            FileStream fs = new FileStream(path + "Data.xml", FileMode.Open);
            int scene = (int)xml.Deserialize(fs);
            fs.Close();
            return scene;
        }
        return 0;
    }
}