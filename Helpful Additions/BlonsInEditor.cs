using HarmonyLib;
using HelpfulAdditions.Properties;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Unity.UI_New.ChallengeEditor;

namespace HelpfulAdditions {
    [HarmonyPatch]
    internal static class BlonsInEditor {
        [HarmonyPatch(typeof(ChallengeEditor), nameof(ChallengeEditor.MapSelectClicked))]
        [HarmonyPrefix]
        // Allow all to be seen
        public static bool ShowHiddenMapsPre(out bool[] __state) {
            if (Settings.BlonsInEditorOn) {
                bool[] isHidden = new bool[GameData.Instance.mapSet.maps.Length];
                for (int i = 0; i < isHidden.Length; i++) {
                    isHidden[i] = GameData.Instance.mapSet.maps[i].isBrowserOnly;
                    GameData.Instance.mapSet.maps[i].isBrowserOnly = false;
                }
                __state = isHidden;
                return true;
            }
            __state = null;
            return true;
        }

        [HarmonyPatch(typeof(ChallengeEditor), nameof(ChallengeEditor.MapSelectClicked))]
        [HarmonyPostfix]
        // Set back to normal
        public static void ShowHiddenMapsPost(bool[] __state) {
            if (Settings.BlonsInEditorOn && __state is not null)
                for (int i = 0; i < __state.Length; i++)
                    GameData.Instance.mapSet.maps[i].isBrowserOnly = __state[i];
        }
    }
}
