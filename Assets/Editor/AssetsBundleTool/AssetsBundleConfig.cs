using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

//可视化编辑资源
[CreateAssetMenu(fileName = "AssetsBundleConfig",menuName = "CreateABConfig",order = 0)]
public class AssetsBundleConfig : ScriptableObject
{
    //以单个文件名打包
    public List<string> SinglePathList = new List<string>();
    //已文件夹为单位打包
    public List<FolderPath> FolderPathList = new List<FolderPath>();

    [System.Serializable]
    public struct FolderPath
    {
        public string Path;
        public string Name;
    }
}

//本地生成的AB包依赖关系配置
[System.Serializable]
public class LocalAssetsBundleConfig
{
    [XmlElement("configList")]
    public List<LocalAssetBundleVO> configList { get; set; }
}

[System.Serializable]
public class LocalAssetBundleVO
{
    [XmlAttribute("Path")]
    public string Path { get; set; }
    [XmlAttribute("Crc")]
    public uint Crc { get; set; }
    [XmlAttribute("BundleName")]
    public string BundleName { get; set; }
    [XmlAttribute("AssetName")]
    public string AssetName { get; set; }
    [XmlElement("ABDependce")]
    public List<string> ABDependce { get; set; }
}
