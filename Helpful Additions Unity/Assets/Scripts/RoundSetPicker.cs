using UnityEngine;
using Il2CppAssets.Scripts.Data;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppTMPro;
using MelonLoader;
using System.Text.RegularExpressions;
#endif

namespace HelpfulAdditions.MonoBehaviors {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
    [RegisterTypeInIl2Cpp(logSuccess: false)]
#endif
    public class RoundSetPicker : MonoBehaviour {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
        private string DefaultValue;
        private float YPos;
        private TMP_FontAsset Font;

        void Start() {
            transform.localPosition = new(0, YPos);

            Transform label = transform.Find("Label");
            TextMeshProUGUI labelText = label.GetComponent<TextMeshProUGUI>();
            labelText.font = Font;

            TMP_Dropdown dropdown = transform.GetComponent<TMP_Dropdown>();
            dropdown.options.Clear();
            int roundSetCount = GameData.Instance.roundSets.Length;
            for (int i = 0; i < roundSetCount; i++) {
                string name = GameData.Instance.roundSets[i].name;
                string displayName = GetDisplayName(name);
                dropdown.options.Add(new TMP_Dropdown.OptionData(displayName));
                if (DefaultValue.Equals(name)) {
                    dropdown.SetValue(i);
                    // If the default value is the first in the list it won't update the text
                    labelText.text = displayName;
                }
            }

            Transform template = transform.Find("Template");

            Transform viewport = template.Find("Viewport");
            // i dunno why
            if (roundSetCount < 3)
                viewport.localPosition = new(0, viewport.localPosition.y - (roundSetCount == 1 ? 150 : 50));

            Transform content = viewport.Find("Content");

            Transform item = content.Find("Item");

            Transform itemLabel = item.Find("ItemLabel");
            TextMeshProUGUI itemLabelText = itemLabel.GetComponent<TextMeshProUGUI>();
            itemLabelText.font = Font;
        }

        public void Init(string defaultValue, float yPos, TMP_FontAsset font) {
            DefaultValue = defaultValue;
            YPos = yPos;
            Font = font;
        }

        private static string GetDisplayName(string name) => Regex.Replace(name, "roundset", "", RegexOptions.IgnoreCase);
#endif
    }
}
