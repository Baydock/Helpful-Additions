using HarmonyLib;
using HelpfulAdditions.Properties;
using Il2CppAssets.Scripts.Unity.Scenes;

namespace HelpfulAdditions {
    [HarmonyPatch]
    internal static class SkipStartScreen {
        [HarmonyPatch(typeof(TitleScreen), "Start")]
        [HarmonyPostfix]
        public static void QuickOpenBtd6(TitleScreen __instance) {
            if (Settings.SkipStartScreen) {
                __instance.SkipTitleAnimClicked();
                __instance.OnPlayButtonClicked();
            }
        }
    }
}
