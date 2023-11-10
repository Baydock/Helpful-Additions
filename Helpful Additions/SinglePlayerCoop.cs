using HarmonyLib;
using HelpfulAdditions.Properties;
using Il2CppAssets.Scripts.Unity.UI_New.Coop;

namespace HelpfulAdditions {
    [HarmonyPatch]
    internal static class SinglePlayerCoop {
        [HarmonyPatch(typeof(CoopLobbyScreen), nameof(CoopLobbyScreen.Update))]
        [HarmonyPostfix]
        public static void AllowSinglePlayerCoop(CoopLobbyScreen __instance) {
            if (Settings.SinglePlayerCoopOn) {
                __instance.readyBtn.enabled = true;
                __instance.readyBtn.interactable = true;
            }
        }
    }
}
