using HarmonyLib;
using HelpfulAdditions.MonoBehaviors;
using HelpfulAdditions.Properties;
using HelpfulAdditions.Utils;
using Il2Cpp;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppAssets.Scripts.Unity.UI_New.Utils;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HelpfulAdditions {
    [HarmonyPatch]
    internal static class RoundSetSwitcher {
        public const string Identifier = "RoundSetSwitcherCode";
        private static readonly int IdentifierLength = Identifier.Length;

        private static bool RoundSetSwitcherEnabled() => Settings.RoundSetSwitcherOn && InGameUtils.InSandbox();

        [HarmonyPatch(typeof(DebugValueScreen), nameof(DebugValueScreen.Init),
            new System.Type[] { typeof(Il2CppSystem.Action<string>), typeof(string), typeof(PopupScreen.BackGround), typeof(Popup.TransitionAnim) })]
        [HarmonyPrefix]
        // __state is true if it is a switcher popup, false if otherwise.
        public static bool AddSwitcherPopupPre(ref string defaultValue, out bool __state) {
            if (RoundSetSwitcherEnabled()) {
                if (defaultValue.Length > IdentifierLength) {
                    string testCode = defaultValue[..IdentifierLength];
                    if (testCode.Equals(Identifier)) {
                        defaultValue = defaultValue[IdentifierLength..];
                        __state = true;
                        return true;
                    }
                }
            }
            __state = false;
            return true;
        }

        [HarmonyPatch(typeof(DebugValueScreen), nameof(DebugValueScreen.Init),
            new System.Type[] { typeof(Il2CppSystem.Action<string>), typeof(string), typeof(PopupScreen.BackGround), typeof(Popup.TransitionAnim) })]
        [HarmonyPostfix]
        // __state is true if it is a switcher popup, false if otherwise.
        public static void AddSwitcherPopupPost(DebugValueScreen __instance, string defaultValue, bool __state, Il2CppSystem.Action<string> okCallback) {
            if (__state) {
                Button confirmButton = __instance.input.transform.parent.Find("Confirm").GetComponent<Button>();
                confirmButton.onClick.AddListener(new System.Action(() => {
                    __instance.OnConfirm(okCallback);
                }));

                TMP_InputField oldInput = __instance.input;
                GameObject oldInputObject = oldInput.gameObject;
                RectTransform oldInputRect = oldInputObject.GetComponent<RectTransform>();
                Transform oldParent = oldInputObject.transform.parent;
                Vector3 oldPosition = oldInputRect.localPosition;
                NK_TextMeshProUGUI oldText = oldInputObject.GetComponentInChildren<NK_TextMeshProUGUI>();
                TMP_FontAsset font = oldText.font;
                Object.Destroy(oldInputObject);

                Transform roundSetPicker = Object.Instantiate(AssetBundles.UiPrefabs.GetResource<GameObject>("RoundSetPicker"), oldParent).transform;
                roundSetPicker.GetComponent<RoundSetPicker>().Init(defaultValue, oldPosition.y, font);
            }
        }

        [HarmonyPatch(typeof(DebugValueScreen), nameof(DebugValueScreen.OnConfirm), new System.Type[] { typeof(Il2CppSystem.Action<string>) })]
        [HarmonyPrefix]
        public static bool ConfirmSwitcherPopup(DebugValueScreen __instance, Il2CppSystem.Action<string> okCallback) {
            if (RoundSetSwitcherEnabled()) {
                TMP_Dropdown dropdown = __instance.GetComponentInChildren<TMP_Dropdown>();
                if (dropdown is not null) {
                    okCallback?.Invoke(GameData.Instance.roundSets[dropdown.value].name);
                    return false;
                }
            }
            return true;
        }
    }
}
