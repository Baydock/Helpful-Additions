using HarmonyLib;
using HelpfulAdditions.Properties;
using Il2CppAssets.Scripts.Unity.UI_New;
using UnityEngine;

namespace HelpfulAdditions {
    [HarmonyPatch]
    internal static class RoundButtons {

        [HarmonyPatch(typeof(MainHudRightAlign), nameof(MainHudRightAlign.Initialise))]
        [HarmonyPostfix]
        public static void AddRoundButtons(MainHudRightAlign __instance) {
            Object.Instantiate(AssetBundles.UiPrefabs.GetResource<GameObject>("RoundButtons"), __instance.panel.transform);
        }
    }
}
