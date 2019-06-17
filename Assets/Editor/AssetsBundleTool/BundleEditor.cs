using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

public class BundleEditor
{
    public static string AB_CONFIG_PATH = "Assets/Editor/AssetsBundleTool/AssetsBundleConfig.asset";
    public static string AB_TARGET_PATH = Application.streamingAssetsPath;
    public static string LOCAL_CONFIG_PATH = Application.dataPath + "LocalAssetBundleConfig";

    //ab包名和对应的路径字典 key:ab包名字 value:路径
    public static Dictionary<string, string> FolderPathDic = new Dictionary<string, string>();

    //单个预设对应的所有依赖项字典 key:Prefab名字 value:依赖项路径列表
    public static Dictionary<string, List<string>> PrefabDependDic = new Dictionary<string, List<string>>();

    //已经提取的ab包路径列表 用于过滤冗余ab包
    public static List<string> FilterPathList = new List<string>();

    //Prefab所有依赖的路径对应的ab包名
    public static Dictionary<string, string> PathBundleNameDic = new Dictionary<string, string>();

    //需要生成本地依赖表的路径列表
    private static List<string> VaildPathList = new List<string>();

    //英文版文本处理工具
    //[MenuItem("AssetsBundle/BuildTxt")]
    //public static void BuildTxt()
    //{
    //    FileStream fileStream = new FileStream(Application.dataPath + "/text1.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

    //    StreamReader streamReader = new StreamReader(fileStream, System.Text.Encoding.GetEncoding("utf-8"));

    //    FileStream fileStream1 = new FileStream(Application.dataPath + "/text2.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

    //    FileStream fileStream2 = new FileStream(Application.dataPath + "/text3.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

    //    StreamWriter sw1 = new StreamWriter(fileStream1, System.Text.Encoding.UTF8);
    //    StreamWriter sw2 = new StreamWriter(fileStream2, System.Text.Encoding.UTF8);


    //    while (streamReader.Peek() > 0)
    //    {
    //        string line = streamReader.ReadLine();
            

    //        string[] lines = Regex.Split(line, "	", RegexOptions.IgnoreCase);

    //        if(lines[0] == null || lines[0] == "")
    //        {
    //            Debug.LogError(lines[0]);
    //        }

    //        if (lines[1] == null || lines[1] == "")
    //        {
    //            Debug.LogError(lines[1]);
    //        }

    //        sw1.WriteLine(lines[0]);            sw2.WriteLine(lines[1]);

    //        Debug.Log(lines[0]);
    //        Debug.Log(lines[1]);
    //    }

    //    sw1.Close();
    //    sw2.Close();
    //    fileStream1.Close();
    //    fileStream2.Close();
    //    streamReader.Close();
    //    fileStream.Close();
    //}


    [MenuItem("AssetsBundle/Build")]
    public static void Build()
    {
        //初始化
        Init();

        //初始化路径字典
        InitPathDic();

        //设置AB包名
        SetAssetsBundleName();

        //打包ab包
        BuildAssetsBundle();

        //创建本地依赖表
        CreateLocalConfig();

        //清除旧的路径改变了的ab包
        DeleteOldAssetsBundle();

        //清除缓存
        Clear();
    }

    //初始化0
    private static void Init()
    {
        FolderPathDic.Clear();
        FilterPathList.Clear();
        PrefabDependDic.Clear();
        PathBundleNameDic.Clear();
        VaildPathList.Clear();
    }

    //从本地配置生成路径字典
    private static void InitPathDic()
    {
        AssetsBundleConfig config = AssetDatabase.LoadAssetAtPath<AssetsBundleConfig>(AB_CONFIG_PATH);

        //获取已文件夹为单位打包的字典
        foreach (var info in config.FolderPathList)
        {
            if (FolderPathDic.ContainsKey(info.Name) == false)
            {
                FolderPathDic.Add(info.Name, info.Path);
                FilterPathList.Add(info.Path);
                VaildPathList.Add(info.Path);
            }
            else
            {
                Debug.LogError("配置错误,包名重复");
            }
        }

        //获取以单个Prefab为单位打包的路径字典
        string[] allPath = AssetDatabase.FindAssets("t:Prefab", config.SinglePathList.ToArray());

        for (int i = 0; i < allPath.Length; ++i)
        {
            string path = AssetDatabase.GUIDToAssetPath(allPath[i]);

            EditorUtility.DisplayProgressBar("查找Prefab", path, i * 1.0f / allPath.Length);

            VaildPathList.Add(path);

            if (PathExist(path) == false)
            {
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                string[] allDepend = AssetDatabase.GetDependencies(path);
                List<string> allDependPath = new List<string>();

                for (int j = 0; j < allDepend.Length; ++j)
                {
                    //过滤掉已存在的路径和C#脚本
                    if (PathExist(allDepend[j]) == false && allDepend[j].EndsWith(".cs") == false)
                    {
                        FilterPathList.Add(allDepend[j]);
                        allDependPath.Add(allDepend[j]);
                    }
                }

                if (PrefabDependDic.ContainsKey(go.name) == false)
                {
                    PrefabDependDic.Add(go.name, allDependPath);
                }
                else
                {
                    Debug.LogError("存在相同名字的Prefab:" + go.name);
                }
            }
        }
    }

    //设置AB包的名字
    private static void SetAssetsBundleName()
    {
        //设置AB包的名字
        foreach (KeyValuePair<string, string> info in FolderPathDic)
        {
            SetAssetsBundleName(info.Key, info.Value);
        }

        foreach (KeyValuePair<string, List<string>> info in PrefabDependDic)
        {
            SetAssetsBundleName(info.Key, info.Value);
        }
    }

    //设置ab包名
    private static void SetAssetsBundleName(string name, string path)
    {
        AssetImporter assetImporter = AssetImporter.GetAtPath(path);

        if (assetImporter != null)
        {
            assetImporter.assetBundleName = name;
        }
        else
        {
            Debug.LogError("路径错误:" + path);
        }
    }

    //设置ab包名
    private static void SetAssetsBundleName(string name, List<string> paths)
    {
        for(int i = 0; i < paths.Count ; i++)
        {
            SetAssetsBundleName(name, paths[i]);
        }
    }

    //打包ab包
    private static void BuildAssetsBundle()
    {
        string[] allBundles = AssetDatabase.GetAllAssetBundleNames();

        //key为全路径 value为ab包名
        //Dictionary<string, string> resPathDic = new Dictionary<string, string>();

        for (int i = 0; i < allBundles.Length; ++i)
        {
            string[] paths = AssetDatabase.GetAssetPathsFromAssetBundle(allBundles[i]);
            for (int j = 0; j < paths.Length; ++j)
            {
                if (paths[j].EndsWith(".cs"))
                {
                    continue;
                }

                //判断是否是需要生成本地依赖表的资源(需要动态加载的资源)
                bool isVaildPath = false;
                for (int k = 0; k < VaildPathList.Count; ++k)
                {
                    if (paths[j].Contains(VaildPathList[k]))
                    {
                        isVaildPath = true;
                    }
                }

                if (isVaildPath == false)
                {
                    continue;
                }

                PathBundleNameDic.Add(paths[j],allBundles[i]);
            }
        }

        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
    }

    //创建AssetsBundle本地依赖表
    private static void CreateLocalConfig()
    {
        LocalAssetsBundleConfig config = new LocalAssetsBundleConfig();
        config.configList = new List<LocalAssetBundleVO>();

        foreach (string path in PathBundleNameDic.Keys)
        {
            LocalAssetBundleVO vo = new LocalAssetBundleVO();
            vo.Path = path;
            vo.Crc = Crc32.GetCrc32(path);
            vo.BundleName = PathBundleNameDic[path];
            vo.AssetName = path.Remove(0, path.LastIndexOf("/") + 1);   //取最后一个/后的内容
            vo.ABDependce = new List<string>();

            string[] allDependce = AssetDatabase.GetDependencies(path);

            //Debug.LogError(allDependce.Length + "_Length");

            for (int i = 0; i < allDependce.Length; ++i)
            {
                if (allDependce[i] == path || path.EndsWith(".cs"))
                {
                    //Debug.LogError("continue");
                    continue;
                }

                string tempName = "";
                if (PathBundleNameDic.TryGetValue(allDependce[i], out tempName))
                {
                    //Debug.LogError("??????????");
                    //同一个ab包无须添加依赖
                    if (tempName == PathBundleNameDic[path])
                    {
                        //Debug.LogError("continue22222222222222222222");
                        continue;
                    }

                    //可能存在一个prefab引用某ab包下多个文件的情况
                    if (!vo.ABDependce.Contains(tempName))
                    {
                        //Debug.LogError("可能存在一个prefab引用某ab包下多个文件的情况");
                        vo.ABDependce.Add(tempName);
                    }
                }
            }
            config.configList.Add(vo);
        }

        //写入Xml
        string xmlPath = Application.dataPath + "/LocalAssetBundleConfig.xml";
        if (File.Exists(xmlPath))
        {
            File.Delete(xmlPath);
        }

        FileStream fs = new FileStream(xmlPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fs,System.Text.Encoding.UTF8);
        XmlSerializer xs = new XmlSerializer(config.GetType());
        xs.Serialize(sw, config);
        sw.Close();
        fs.Close();

        //写入二进制

        //二进制不需要路径 减少文件大小
        foreach (var info in config.configList)
        {
            info.Path = "";
        }


        string binaryPath = Application.dataPath + "/LocalAssetBundleConfig.bytes";
        //为什么删除二进制文件会出错
        //if (File.Exists(binaryPath))
        //{
        //    File.Delete(binaryPath);
        //}
        FileStream binaryFs = new FileStream(binaryPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(binaryFs, config);
        binaryFs.Close();
    }

    //清除设置的AB包名字(避免本地的.mat文件批量发生改变)
    private static void Clear()
    {
        //一系列操作完成后 清除设置的AB包名字 避免本地的.mat文件批量发生改变
        string[] allABNames = AssetDatabase.GetAllAssetBundleNames();

        for (int i = 0; i < allABNames.Length; ++i)
        {
            AssetDatabase.RemoveAssetBundleName(allABNames[i], true);
            EditorUtility.DisplayProgressBar("清除AB包名", "Path:" + allABNames[i], i * 1.0f / allABNames.Length);
        }
        //清空进度条
        EditorUtility.ClearProgressBar();
    }

    //清除路径不存在的ab包
    private static void DeleteOldAssetsBundle()
    {
        string[] allBundleName = AssetDatabase.GetAllAssetBundleNames();
        DirectoryInfo directoryInfo = new DirectoryInfo(AB_TARGET_PATH);

        FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; ++i)
        {
            if (ContainsBundleName(files[i].Name, allBundleName) || files[i].Name.EndsWith(".meta"))
            {
                continue;
            }
            else
            {
                if (File.Exists(files[i].FullName))
                {
                    File.Delete(files[i].FullName);
                }
            }
        }
    }

    //字符创列表中是否存在某字符串
    private static bool ContainsBundleName(string name, string[] strs)
    {
        for (int i = 0; i < strs.Length; i++)
        {
            if (name == strs[i])
            {
                return true;
            }
        }
        return false;
    }

    //检验路径是否已存在 避免冗余ab包
    private static bool PathExist(string path)
    {
        foreach (string existPath in FilterPathList)
        {
            if (existPath == path)
            {
                return true;
            }

            if (path.Contains(existPath) && path.Replace(existPath,"")[0] == '/')
            {
                return true;
            }
        }

        return false;
    }


    [MenuItem("AssetsBundle/LoadTest")]
    public static void LoadABTest()
    {
        TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(Application.dataPath + "LocalAssetBundleConfig.bytes");
        MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        LocalAssetsBundleConfig config = binaryFormatter.Deserialize(memoryStream) as LocalAssetsBundleConfig;
        memoryStream.Close();

        string path = "Assets/GameData/Prefabs/Attack.prefab";
        uint crc = Crc32.GetCrc32(path);

        LocalAssetBundleVO info = null;
        for (int i = 0; i < config.configList.Count; ++i)
        {
            if (config.configList[i].Crc == crc)
            {
                info = config.configList[i];
            }
        }

        for (int i = 0; i < info.ABDependce.Count; ++i)
        {
            AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + info.ABDependce[i]);
        }
        AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + info.BundleName);
        GameObject obj = GameObject.Instantiate(assetBundle.LoadAsset<GameObject>(info.AssetName));
    }
}
