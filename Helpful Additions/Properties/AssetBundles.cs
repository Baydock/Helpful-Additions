using UnityEngine;

namespace HelpfulAdditions.Properties {
    internal static class AssetBundles {
        public static AssetBundle Bloons => Resources.GetAssetBundle("bloons");

        public static AssetBundle UiPrefabs => Resources.GetAssetBundle("uiprefabs");
    }
}
