using HarmonyLib;
using HelpfulAdditions.Properties;
using MelonLoader;

[assembly: MelonInfo(typeof(HelpfulAdditions.Mod), "Helpful Additions", "3.1.0", "Baydock")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonColor(255, 200, 162, 200)]
[assembly: MelonAuthorColor(255, 255, 104, 0)]

namespace HelpfulAdditions {
    [HarmonyPatch]
    public sealed class Mod : MelonMod {
        public static MelonLogger.Instance Logger { get; private set; }

        public override void OnInitializeMelon() {
            Logger = LoggerInstance;

            // Pre-load asset bundles
            _ = AssetBundles.Bloons;
            _ = AssetBundles.UiPrefabs;
        }
    }
}
