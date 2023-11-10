#if UNITY_EDITOR

using System.IO;
using UnityEditor;

public class CreateAssetBundles : Editor {
    [MenuItem("Assets/Build Asset Bundle")]
    static void CreateBundle() {
        string bundlePath = "Assets/AssetBundles/";
        if (!Directory.Exists(bundlePath))
            Directory.CreateDirectory(bundlePath);
        BuildPipeline.BuildAssetBundles(bundlePath, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows);

    }
}

#endif