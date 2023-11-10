using HarmonyLib;
using HelpfulAdditions.MonoBehaviors;
using HelpfulAdditions.Properties;
using HelpfulAdditions.Utils;
using Il2Cpp;
using Il2CppAssets.Scripts.Unity.UI_New.ChallengeEditor;
using Il2CppAssets.Scripts.Unity.UI_New.Settings;
using Il2CppTMPro;
using UnityEngine;

namespace HelpfulAdditions {
    [HarmonyPatch]
    internal static class SettingsMenu {
        public const string HelpfulAdditionsCode = "Helpful Additions Setting";

        [HarmonyPatch(typeof(SettingsScreen), nameof(SettingsScreen.Open))]
        [HarmonyPostfix]
        public static void AddSettingsButton(SettingsScreen __instance) {
            Transform hotkeysBtn = __instance.hotkeysBtn.transform;
            NK_TextMeshProUGUI hotkeysBtnText = hotkeysBtn.GetComponentInChildren<NK_TextMeshProUGUI>();

            GameObject settingsButton = Object.Instantiate(AssetBundles.UiPrefabs.GetResource<GameObject>("HASettingsButton"), hotkeysBtn.parent.parent);
            settingsButton.GetComponent<SettingsButton>().Init(hotkeysBtnText.font);
        }

        [HarmonyPatch(typeof(ExtraSettingsScreen), nameof(ExtraSettingsScreen.Open))]
        [HarmonyPostfix]
        public static void AddSettings(ref ExtraSettingsScreen __instance, Il2CppSystem.Object menuData) {
            if (menuData is not null && menuData.Equals(HelpfulAdditionsCode)) {
                TMP_FontAsset font = __instance.bigBloons.GetComponentInChildren<TextMeshProUGUI>().font;

                Transform parent = __instance.bigBloons.transform.parent;
                parent.ClearChildren();

                Transform haSettings = Object.Instantiate(AssetBundles.UiPrefabs.GetResource<GameObject>("HASettings"), parent).transform;
                haSettings.GetComponent<HASettings>().Init(font);
            }
        }
    }
}