using UnityEngine;
using Il2CppAssets.Scripts.Data;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
using HelpfulAdditions.Properties;
using HelpfulAdditions.Utils;
using Il2Cpp;
using Il2CppAssets.Scripts.Unity.Menu;
using Il2CppAssets.Scripts.Unity.UI_New;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using MelonLoader;
using UnityEngine.EventSystems;
#endif

namespace HelpfulAdditions.MonoBehaviors {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
    [RegisterTypeInIl2Cpp(logSuccess: false)]
#endif
    public class RoundButtons : MonoBehaviour {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
        private NK_TextMeshProUGUI RoundText;
        private NK_TextMeshProUGUI TitleText;

        void Start() {
            GameObject roundInfo = transform.Find("RoundInfoButton").gameObject;
            roundInfo.SetActive(Settings.RoundInfoScreenOn && !InGame.Bridge.Model.gameMode.Equals("Apopalypse"));
            ButtonExtended roundInfoButton = roundInfo.GetComponent<ButtonExtended>();
            roundInfoButton.OnPointerUpEvent = new System.Action<PointerEventData>(p => MenuManager.instance.OpenMenu("PowersSelectUI", $"{RoundInfoScreen.Identifier}{InGame.Bridge.GetCurrentRound()}"));

            GameObject roundSetSwitcher = transform.Find("RoundSetSwitcherButton").gameObject;
            roundSetSwitcher.SetActive(Settings.RoundSetSwitcherOn && InGameUtils.InSandbox());
            ButtonExtended roundSetSwitcherButton = roundSetSwitcher.GetComponent<ButtonExtended>();
            roundSetSwitcherButton.OnPointerUpEvent = new System.Action<PointerEventData>(p => {
                PopupScreen.instance.ShowSetNamePopup("Set Round Set", "Choose the current round set.", new System.Action<string>(roundSet => {
                    InGame.Bridge.Model.SetRoundSet(GameData.Instance.RoundSetByName(roundSet));
                }), RoundSetSwitcher.Identifier + InGame.Bridge.Model.roundSet.name);
            });

            RoundText = MainHudRightAlign.instance.roundButton.GetComponent<NK_TextMeshProUGUI>();
            TitleText = MainHudRightAlign.instance.roundObj.transform.Find("Title").GetComponent<NK_TextMeshProUGUI>();
        }

        void Update() {
            float roundWidth = RoundText.preferredWidth;
            float titleWidth = TitleText.preferredWidth;
            float x = System.Math.Max(roundWidth, titleWidth) + 200;
            transform.localPosition = new Vector3(-x, 0);
        }
#endif
    }
}
