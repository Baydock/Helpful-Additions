using UnityEngine;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
using HelpfulAdditions.Properties;
using MelonLoader;
using Il2CppTMPro;
#endif

namespace HelpfulAdditions.MonoBehaviors {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
    [RegisterTypeInIl2Cpp(logSuccess: false)]
#endif
    public class HASettings : MonoBehaviour {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
        private TMP_FontAsset Font;

        void Start() {
            Transform roundInfoScreen = transform.Find("LeftColumn/RoundInfoScreen/All");
            roundInfoScreen.GetComponent<SettingToggle>().SetSetting(Settings.RoundInfoScreenOn);

            Transform showBloonIds = transform.Find("LeftColumn/RoundInfoScreen/ShowBloonIds");
            showBloonIds.GetComponent<SettingToggle>().SetSetting(Settings.ShowBloonIdsOn);

            Transform showBossBloons = transform.Find("LeftColumn/RoundInfoScreen/ShowBossBloons");
            showBossBloons.GetComponent<SettingToggle>().SetSetting(Settings.ShowBossBloonsOn);

            Transform roundSetSwitcher = transform.Find("RightColumn/RoundSetSwitcher");
            roundSetSwitcher.GetComponent<SettingToggle>().SetSetting(Settings.RoundSetSwitcherOn);

            Transform deleteAllProjectilesButton = transform.Find("RightColumn/DeleteAllProjectilesButton");
            deleteAllProjectilesButton.GetComponent<SettingToggle>().SetSetting(Settings.DeleteAllProjectilesOn);

            Transform powersInSandbox = transform.Find("LeftColumn/PowersInSandbox");
            powersInSandbox.GetComponent<SettingToggle>().SetSetting(Settings.PowersInSandboxOn);

            Transform singlePlayerCoop = transform.Find("RightColumn/SinglePlayerCoop");
            singlePlayerCoop.GetComponent<SettingToggle>().SetSetting(Settings.SinglePlayerCoopOn);

            Transform blonsInEditor = transform.Find("LeftColumn/BlonsInEditor");
            blonsInEditor.GetComponent<SettingToggle>().SetSetting(Settings.BlonsInEditorOn);

            Transform skipStartScreen = transform.Find("RightColumn/SkipStartScreen");
            skipStartScreen.GetComponent<SettingToggle>().SetSetting(Settings.SkipStartScreen);

            foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
                text.font = Font;
        }

        public void Init(TMP_FontAsset font) {
            Font = font;
        }
#endif
    }
}
