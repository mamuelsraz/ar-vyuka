using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Tools/Create/AssetBundles for Windows")]
    static void BuildAllAssetBundlesWindows()
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

    [MenuItem("Tools/Create/AssetBundles for Android")]
    static void BuildAllAssetBundlesAndroid()
    {
        string assetBundleDirectory = "Assets/Bundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.UncompressedAssetBundle,
                                        BuildTarget.Android);
    }
}