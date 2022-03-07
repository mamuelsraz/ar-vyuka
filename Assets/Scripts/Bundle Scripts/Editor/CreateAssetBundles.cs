using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Tools/Create/AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/Bundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.UncompressedAssetBundle,
                                        BuildTarget.StandaloneWindows);
    }
}