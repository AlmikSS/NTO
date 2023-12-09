using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

public static class SaveManager
{
    public static void Save(List<ItemData> invData)
    {
        XmlSerializer xml = new XmlSerializer(typeof(List<ItemData>));
        FileStream fs = new FileStream("inventoryData.xml", FileMode.Create);
        xml.Serialize(fs, invData);
    }

    public static List<ItemData> Load()
    {
        if (File.Exists("inventoryData.xml"))
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<ItemData>));
            FileStream fs = new FileStream("inventoryData.xml", FileMode.Open);
            return (List<ItemData>)xml.Deserialize(fs);
        }
        return null;
    }
}