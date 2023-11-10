using UnityEngine;

namespace HelpfulAdditions.Properties {
    internal static class Sprites {

        #region Ui

        public static Sprite OnIcon => AssetBundles.UiPrefabs.GetResource<Sprite>("OnIcon");

        public static Sprite OffIcon => AssetBundles.UiPrefabs.GetResource<Sprite>("OffIcon");

        #endregion

        #region UnknownBloon

        public static Sprite UnknownBloon => AssetBundles.Bloons.GetResource<Sprite>("UnknownBloon");

        public static Sprite UnknownEdge => AssetBundles.Bloons.GetResource<Sprite>("TestEdge");

        public static Sprite UnknownSpan => AssetBundles.Bloons.GetResource<Sprite>("TestSpan");

        #endregion
    }
}
