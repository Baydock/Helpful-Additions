using UnityEngine;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
using HelpfulAdditions.Properties;
using HelpfulAdditions.Utils;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Models.Rounds;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine.UI;
using Math = System.Math;
#endif

namespace HelpfulAdditions.MonoBehaviors {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
    [RegisterTypeInIl2Cpp(logSuccess: false)]
#endif
    public class RoundInfoPanel : MonoBehaviour {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
        private const float RoundInfoTimesWidth = 3000;

        public RoundInfoPanel(System.IntPtr ptr) : base(ptr) { }

        private BloonGroupModel Group;
        private float UnitsPerTime;
        private TMP_FontAsset Font;
        private bool IsRealBoss;

        private BloonModel BloonModel;
        private bool IsUnknown;

        private Sprite IconSprite;
        private Sprite EdgeSprite;
        private Sprite SpanSprite;

        void Start() {
            Transform iconAndCountBox = transform.Find("IconAndCountBox");

            RectTransform icon = iconAndCountBox.Find("Icon").Cast<RectTransform>();
            float height = (float)Math.Min(IconSprite.rect.height * 1.5, 300);
            float width = height * ((float)IconSprite.rect.width / IconSprite.rect.height);
            icon.sizeDelta = new(width, height);
            Image iconImage = icon.GetComponent<Image>();
            iconImage.sprite = IconSprite;

            Transform count = iconAndCountBox.Find("Count");
            count.gameObject.SetActive(!IsRealBoss);
            TextMeshProUGUI countText = count.GetComponent<TextMeshProUGUI>();
            countText.text = $"x{Group.count}";
            countText.font = Font;

            Transform tier = iconAndCountBox.Find("Tier");
            tier.gameObject.SetActive(BloonModel.isBoss || BloonModel.IsRock);
            TextMeshProUGUI tierText = tier.GetComponent<TextMeshProUGUI>();
            tierText.text = $"T{BloonUtils.GetBossTier(BloonModel)}";
            tierText.font = Font;

            Transform infoBox = transform.Find("InfoBox");

            float start = Math.Max(0, Group.start);
            float end = Group.end;
            int startPadding = (int)Math.Round(start * UnitsPerTime);

            Transform timeBox = infoBox.Find("TimeBox");
            VerticalLayoutGroup timeBoxVLG = timeBox.GetComponent<VerticalLayoutGroup>();

            float startSec = (float)Math.Round(start / 60, 2);
            float endSec = (float)Math.Round(end / 60, 2);
            Transform times = timeBox.Find("Times");
            times.gameObject.SetActive(!IsRealBoss);
            TextMeshProUGUI timesText = times.GetComponent<TextMeshProUGUI>();
            timesText.text = $"{startSec}s - {endSec}s";
            timesText.font = Font;

            float totalSec = (end - start) / 60;
            float bloonsPerSec = (float)Math.Round((Group.count - 1) / totalSec, 2);
            Transform bps = timeBox.Find("BPS");
            TextMeshProUGUI bpsText = bps.GetComponent<TextMeshProUGUI>();
            if (IsRealBoss)
                bpsText.text = "Enters this round";
            else if (bloonsPerSec == float.PositiveInfinity)
                bpsText.text = "0 spacing";
            else
                bpsText.text = $"{bloonsPerSec} bloons/s";
            bpsText.font = Font;

            float timesWidth = Math.Max(timesText.preferredWidth, bpsText.preferredWidth);
            int timePadding = (startPadding + timesWidth > RoundInfoTimesWidth) ? (int)(RoundInfoTimesWidth - timesWidth) : startPadding;
            timeBoxVLG.padding.left += timePadding;

            Transform edgeAndSpanBox = infoBox.Find("EdgeAndSpanBox");
            HorizontalLayoutGroup edgeAndSpanBoxHLG = edgeAndSpanBox.GetComponent<HorizontalLayoutGroup>();
            edgeAndSpanBoxHLG.padding.left += startPadding;

            Transform edge1 = edgeAndSpanBox.Find("Edge1");
            Image edge1Image = edge1.GetComponent<Image>();
            edge1Image.sprite = EdgeSprite;

            float spanWidth = (Group.end - start) * UnitsPerTime;
            RectTransform span = edgeAndSpanBox.Find("Span").Cast<RectTransform>();
            span.sizeDelta = new(spanWidth, span.sizeDelta.y);
            Image spanImage = span.GetComponent<Image>();
            spanImage.sprite = SpanSprite;

            Transform edge2 = edgeAndSpanBox.Find("Edge2");
            Image edge2Image = edge2.GetComponent<Image>();
            edge2Image.sprite = EdgeSprite;

            Transform id = infoBox.Find("ID");
            TextMeshProUGUI idText = id.GetComponent<TextMeshProUGUI>();
            idText.enabled = Settings.ShowBloonIdsOn || IsUnknown;
            idText.text = $"ID: {Group.bloon}";
            idText.font = Font;
        }

        public void Init(BloonGroupModel group, float roundSpawnEnd, bool isRealBoss, TMP_FontAsset font) {
            Group = group;
            UnitsPerTime = RoundInfoTimesWidth / roundSpawnEnd;
            Font = font;
            IsRealBoss = isRealBoss;

            BloonModel = InGame.Bridge.Model.GetBloon(group.bloon);
            IsUnknown = !BloonUtils.GetBloonSprites(BloonModel, out IconSprite, out EdgeSprite, out SpanSprite);
        }
#endif
    }
}
