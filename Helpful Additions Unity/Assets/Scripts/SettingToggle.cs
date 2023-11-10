using UnityEngine;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
using HelpfulAdditions.Properties;
using static HelpfulAdditions.Properties.Settings;
using Il2CppInterop.Runtime.Attributes;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine.UI;
#endif

namespace HelpfulAdditions.MonoBehaviors {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
    [RegisterTypeInIl2Cpp(logSuccess: false)]
#endif
    internal class SettingToggle : MonoBehaviour {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
        private static readonly Color OnColor = Color.white;
        private static readonly Color OffColor = new(.85f, .85f, .85f);

        private TextMeshProUGUI Text;
        private Image ToggleIcon;
        private BoolEntry Setting;

        void Start() {
            Transform text = transform.Find("Text");
            Text = text.GetComponent<TextMeshProUGUI>();

            Transform toggle = transform.Find("Toggle");
            Button toggleButton = toggle.GetComponent<Button>();
            toggleButton.onClick.AddListener(new System.Action(Toggle));

            Transform toggleIcon = toggle.Find("Icon");
            ToggleIcon = toggleIcon.GetComponent<Image>();

            UpdateUi();
        }

        private void Toggle() {
            Setting.Value = !Setting;
            UpdateUi();
        }

        private void UpdateUi() {
            ToggleIcon.sprite = Setting ? Sprites.OnIcon : Sprites.OffIcon;
            Text.color = Setting ? OnColor : OffColor;
        }

        [HideFromIl2Cpp]
        public void SetSetting(BoolEntry setting) => Setting = setting;
#endif
    }
}
