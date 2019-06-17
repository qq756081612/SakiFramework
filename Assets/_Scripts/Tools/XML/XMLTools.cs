using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//using UnityEditor.AssetsBundleConfig;

public class XMLTools
{
    public static void Test()
    {
        //TestVO vo = new TestVO();
        //vo.id = 1;
        //vo.name = "Hello World";
        //vo.list = new List<int>();
        //vo.list.Add(1);
        //vo.list.Add(2);
        //vo.list.Add(3);

        //SerializeXml(vo);
        //LoadABTest();
    }

#if UNITY_EDITOR
    //public static void LoadABTest()
    //{
    //    TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(Application.dataPath + "LocalAssetBundleConfig.bytes");
    //    MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
    //    BinaryFormatter binaryFormatter = new BinaryFormatter();
    //    LocalAssetsBundleConfig config = binaryFormatter.Deserialize(memoryStream) as LocalAssetsBundleConfig;
    //    memoryStream.Close();

    //    string path = "Assets/GameData/Prefabs/Attack.prefab";
    //    uint crc = Crc32.GetCrc32(path);

    //    LocalAssetBundleVO info = null;
    //    for (int i = 0; i < config.configList.Count; ++i)
    //    {
    //        if (config.configList[i].Crc == crc)
    //        {
    //            info = config.configList[i];
    //        }
    //    }

    //    for (int i = 0; i < info.ABDependce.Count; ++i)
    //    {
    //        AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + info.ABDependce[i]);
    //    }
    //    AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + info.BundleName);
    //    GameObject obj = GameObject.Instantiate(assetBundle.LoadAsset<GameObject>(info.AssetName));
    //}
#endif

    //    //序列化XML
    //    public static void SerializeXml(TestVO vo)
    //    {
    //        FileStream fs = new FileStream(Application.dataPath + "/_Scripts/Tools/XML/test.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
    //        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
    //        XmlSerializer xs = new XmlSerializer(vo.GetType());
    //        xs.Serialize(sw, vo);

    //        sw.Close();
    //        fs.Close();
    //    }

    //    //反序列化XML
    //    public static TestVO DeSerializeXml(string path, string name)
    //    {
    //        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
    //        XmlSerializer xs = new XmlSerializer(typeof(TestVO));
    //        TestVO vo = xs.Deserialize(fs) as TestVO;
    //        fs.Close();

    //        return vo;
    //    }

    //    //序列化二进制
    //    public static void SerilizeBinary(TestVO vo)
    //    {
    //        string path = Application.dataPath + "/_Scripts/Tools/XML/test.bytes";
    //        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
    //        BinaryFormatter bf = new BinaryFormatter();
    //        bf.Serialize(fs, vo);
    //        fs.Close();
    //    }

    //    //反序列化二进制
    //    public static TestVO DeSerilizeBinary(string path)
    //    {
    //        TextAsset ta = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path);
    //        MemoryStream ms = new MemoryStream(ta.bytes);
    //        BinaryFormatter bf = new BinaryFormatter();
    //        TestVO vo = bf.Deserialize(ms) as TestVO;
    //        ms.Close();
    //        return vo;
    //    }

    //    public static TestAssetsVO DeSerilizeAssets(string path)
    //    {
    //        TestAssetsVO assets = UnityEditor.AssetDatabase.LoadAssetAtPath<TestAssetsVO>(path);
    //        return assets;
    //    }
    //}

    //[System.Serializable]
    //public class TestVO
    //{
    //    [XmlAttribute("id")]
    //    public int id { get; set; }

    //    [XmlAttribute("name")]
    //    public string name { get; set; }

    //    [XmlElement("List")]
    //    public List<int> list { get; set; }
}

//[CreateAssetMenu(fileName = "TestAssetsVO", menuName = "CreateAssets", order = 0)]
//public class TestAssetsVO : ScriptableObject
//{
//    public int ID;
//    public string Name;
//    public List<int> List;
//}
