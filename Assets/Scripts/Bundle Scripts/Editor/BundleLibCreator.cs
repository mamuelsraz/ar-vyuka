using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class BundleLibCreator
{
    static string path = "Assets/Bundles/list.asset";

    [MenuItem("Tools/Create/AR bundle")]
    public static void Generate()
    {
        Dictionary<Category, List<ArObject>> list = ObjectLoadHandler.LoadFromResources();

        ArObjectLibrary library = ScriptableObject.CreateInstance<ArObjectLibrary>();
        library.Serialize(list);
        AssetDatabase.CreateAsset(library, path);
        AssetDatabase.SaveAssets();
    }
}
