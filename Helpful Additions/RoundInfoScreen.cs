using HarmonyLib;
using HelpfulAdditions.MonoBehaviors;
using HelpfulAdditions.Properties;
using Il2CppAssets.Scripts.Unity.UI_New;
using Il2CppAssets.Scripts.Unity.UI_New.Main.PowersSelect;
using Il2CppTMPro;
using UnityEngine;

#if DEBUG
using Il2CppAssets.Scripts.Models;
#endif

namespace HelpfulAdditions {
    [HarmonyPatch]
    internal static class RoundInfoScreen {

        public const string Identifier = "RoundInfoScreen";

        // dont want the menu to set itself up to make things easier
        [HarmonyPatch(typeof(PowersSelectScreen), nameof(PowersSelectScreen.Open))]
        [HarmonyPrefix]
        public static bool InterceptMenu(Il2CppSystem.Object data) => !(Settings.RoundInfoScreenOn && (data?.ToString().Contains(Identifier) ?? false));

        [HarmonyPatch(typeof(PowersSelectScreen), nameof(PowersSelectScreen.Open))]
        [HarmonyPostfix]
        public static void InterceptMenu(ref PowersSelectScreen __instance, Il2CppSystem.Object data) {
            string message = data?.ToString();
            if (Settings.RoundInfoScreenOn && message is not null && message.Contains(Identifier)) {
                int currentRound = int.Parse(message[Identifier.Length..]);

                TMP_FontAsset font = __instance.menuTitleTxt.font;

                __instance.name = Identifier;

                // don't want monkey money showing
                CommonForegroundScreen.instance.Hide();
                CommonForegroundScreen.instance.Show(true, "", false, false, false, false, false, false);

                __instance.menuTitleTxt.text = "Round Info";

                __instance.GetComponentInParent<CanvasGroup>().alpha = 0;

                for (int i = 0; i < __instance.transform.childCount; i++) {
                    Transform child = __instance.transform.GetChild(i);
                    if (!__instance.menuTitleTxt.transform.IsChildOf(child))
                        child.gameObject.SetActive(false);
                }

                GameObject roundInfo = Object.Instantiate(AssetBundles.UiPrefabs.GetResource<GameObject>("RoundInfo"), __instance.transform);
                roundInfo.GetComponent<RoundInfo>().Init(__instance, currentRound, font);
            }
        }

#if DEBUG
        [HarmonyPatch(typeof(GameModelLoader), nameof(GameModelLoader.Load))]
        [HarmonyPostfix]
        public static void AddTestRoundSet(ref GameModel __result) {
            List<RoundModel> allBloons = new();

            for (int i = 0; i < __result.bloons.Length; i++)
                allBloons.Add(new RoundModel("", new(1) { [0] = new BloonGroupModel("", __result.bloons[i].id, 0, 1, 1) }));

            __result.roundSets = __result.roundSets.Add(new RoundSetModel("AllBloonsRoundSet", allBloons.ToArray(), false));
        }
#endif
    }
}
