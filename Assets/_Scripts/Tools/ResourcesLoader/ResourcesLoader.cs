using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 资源加载的四种方式
 * 1.取得游戏物体 GameObject.Instantiate()
 * 2.Resources.Load()
 * 3.assetBundle.LoadAsset()
 * 4.UnityEditor.AssetDatabase.LoadAssetAtPath<T>("")  编译器下专用
*/


public class ResourcesLoader
{
    public GameObject go;

    public void LoadGameObject()
    {
        ////1.
        //go = GameObject.Instantiate(go);
        ////2.
        //go = Resources.Load("a") as GameObject;
        ////3.
        //AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "");
        //go = ab.LoadAsset("") as GameObject;
        ////4.
        //go = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("绝对路径");
    }
}