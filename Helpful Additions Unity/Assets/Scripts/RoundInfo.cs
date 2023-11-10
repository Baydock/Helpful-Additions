using UnityEngine;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
using HelpfulAdditions.Properties;
using HelpfulAdditions.Utils;
using Il2CppAssets.Scripts.Models.Rounds;
using Il2CppAssets.Scripts.Simulation.Track;
using Il2CppAssets.Scripts.Unity.UI_New;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.Main.PowersSelect;
using Il2CppTMPro;
using MelonLoader;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Math = System.Math;
#endif

namespace HelpfulAdditions.MonoBehaviors {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
    [RegisterTypeInIl2Cpp(logSuccess: false)]
#endif
    public class RoundInfo : MonoBehaviour {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
        private const float RoundInfoPanelHeight = 300;

        public RoundInfo(System.IntPtr ptr) : base(ptr) { }

        private PowersSelectScreen PSS;
        private int CurrentRound = 0;
        private TMP_FontAsset Font;

        private Button PrevArrowButton;
        private Image PrevArrowImage;

        private Button NextArrowButton;
        private Image NextArrowImage;

        private TMP_InputField RoundNumberInput;

        private Transform Scroll;
        private Transform ScrollContent;

        private RectTransform TopInfo;
        private CanvasGroup CanvasGroup;

        void Start() {
            Transform roundSelector = transform.Find("RoundSelector");

            Transform prevArrow = roundSelector.Find("Prev");
            PrevArrowButton = prevArrow.GetComponent<Button>();
            PrevArrowImage = prevArrow.GetComponent<Image>();

            Transform nextArrow = roundSelector.Find("Next");
            NextArrowButton = nextArrow.GetComponent<Button>();
            NextArrowImage = nextArrow.GetComponent<Image>();

            Transform roundText = roundSelector.Find("RoundText");
            TextMeshProUGUI roundTextText = roundText.GetComponent<TextMeshProUGUI>();
            roundTextText.font = Font;

            Transform roundNumber = roundSelector.Find("RoundNumber");
            RoundNumberInput = roundNumber.GetComponent<TMP_InputField>();
            RoundNumberInput.textComponent.font = Font;

            PrevArrowButton.onClick.AddListener(new System.Action(OnPrevArrowButtonClicked));
            NextArrowButton.onClick.AddListener(new System.Action(OnNextArrowButtonClicked));
            RoundNumberInput.onSubmit.AddListener(new System.Action<string>(OnRoundInputChanged));
            RoundNumberInput.onDeselect.AddListener(new System.Action<string>(OnRoundInputChanged));

            Scroll = transform.Find("Scroll");
            ScrollContent = Scroll.Find("Viewport").Find("Content");

            TopInfo = PSS.menuTitleTxt.transform.parent.parent.GetComponent<RectTransform>();
            CanvasGroup = PSS.GetComponentInParent<CanvasGroup>();

            UpdateUI();
        }

        public void Init(PowersSelectScreen pss, int currentRound, TMP_FontAsset font) {
            PSS = pss;
            CurrentRound = currentRound;
            Font = font;

            ValidateCurrentRound();
        }

        void Update() {
            transform.localPosition = new Vector3(transform.localPosition.x, TopInfo.localPosition.y - TopInfo.sizeDelta.y - 150);

            if (CommonBackgroundScreen.instance.customBackgroundOut.gameObject.active)
                CanvasGroup.alpha = CommonBackgroundScreen.instance.customBackgroundOut.color.a;
            else
                CanvasGroup.alpha = CommonBackgroundScreen.instance.customBackgroundIn.color.a;
        }

        private void OnPrevArrowButtonClicked() {
            CurrentRound--;
            UpdateUI();
        }

        private void OnNextArrowButtonClicked() {
            CurrentRound++;
            UpdateUI();
        }

        private void OnRoundInputChanged(string text) {
            if (string.IsNullOrEmpty(text))
                CurrentRound = 0;
            else {
                CurrentRound = int.Parse(text) - 1;
                ValidateCurrentRound();
            }
            UpdateUI();
            EventSystem.current.SetSelectedGameObject(null);
        }

        private void UpdateUI() {
            PopulateRoundInfo();
            PrevArrowImage.enabled = PrevArrowButton.enabled = NotFirstRound();
            NextArrowImage.enabled = NextArrowButton.enabled = NotLastRound();
            RoundNumberInput.text = $"{CurrentRound + 1}";
        }

        private void ValidateCurrentRound() => CurrentRound = Math.Max(Math.Min(CurrentRound, LastRound), 0);
        private bool NotLastRound() => CurrentRound < LastRound;
        private bool NotFirstRound() => CurrentRound > 0;
        private static int LastRound => InGame.Bridge.Model.roundSet.rounds.Count - 1;

        private void PopulateRoundInfo() {
            bool is4x3 = ScreenRatio.GetCurrent().ToRatioType() == ScreenRatioType.Ratio4_3;
            LayoutElement scrollLayout = Scroll.GetComponent<LayoutElement>();
            scrollLayout.minHeight = RoundInfoPanelHeight * (is4x3 ? 7.5f : 6.5f);

            ScrollContent.ClearChildren();

            RoundSetModel roundSet = InGame.Bridge.Model.roundSet;
            List<BloonGroupModel> bloonGroups = new(roundSet.rounds[CurrentRound].groups);

            bloonGroups.Sort((a, b) => a.start > b.start ? 1 : a.start < b.start ? -1 : 0);

            bool showsBoss = false;
            int bossTier = 0;
            if (Settings.ShowBossBloonsOn) {
                BossBloonManager bossManager = InGame.Bridge.simulation.map.spawner.bossBloonManager;
                if (bossManager is not null) {
                    for (int i = 0; i < bossManager.spawnRounds.Count; i++) {
                        if (CurrentRound == bossManager.spawnRounds[i]) {
                            bossTier = i + 1;
                            break;
                        }
                    }
                    showsBoss = bossTier > 0;
                }
            }

            float end = bloonGroups[^1].end;
            for (int i = 0; i < bloonGroups.Count; i++)
                if (bloonGroups[i].end > end)
                    end = bloonGroups[i].end;

            if (showsBoss) {
                string boss = InGame.Bridge.Model.bossBloonType;
                if (InGame.Bridge.Model.bossEliteMode)
                    boss += "Elite";
                boss += bossTier;
                AddRoundInfoPanel(new BloonGroupModel("", boss, 0, end, 1), end, true);
            }

            for (int i = 0; i < bloonGroups.Count; i++)
                AddRoundInfoPanel(bloonGroups[i], end);
        }

        private void AddRoundInfoPanel(BloonGroupModel group, float endTime, bool isRealBoss = false) {
            GameObject roundInfoPanel = Instantiate(AssetBundles.UiPrefabs.GetResource<GameObject>("RoundInfoPanel"), ScrollContent);
            roundInfoPanel.GetComponent<RoundInfoPanel>().Init(group, endTime, isRealBoss, Font);
        }
#endif
    }
}
