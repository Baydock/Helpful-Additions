using UnityEngine;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
using Il2CppAssets.Scripts.Unity.Menu;
using Il2CppAssets.Scripts.Unity.UI_New;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Math = System.Math;
#endif

namespace HelpfulAdditions.MonoBehaviors {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
    [RegisterTypeInIl2Cpp(logSuccess: false)]
#endif
    public class SettingsButton : MonoBehaviour {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
        private const float ScaleSpeed = 1;
        private const float MaxScale = 1;
        private const float MinScale = .95f;

        private TMP_FontAsset Font;

        private Transform ButtonTransform;
        private ButtonExtended Button;
        private Image Image;
        private bool IsPressed = false;
        private float Scale = 1;

        void Start() {
            ButtonTransform = transform.Find("Button");
            Button = ButtonTransform.GetComponent<ButtonExtended>();

            Transform text = ButtonTransform.Find("Text");
            text.GetComponent<TextMeshProUGUI>().font = Font;

            Transform image = ButtonTransform.Find("Image");
            Image = image.GetComponent<Image>();

            Button.onClick.AddListener(new System.Action(() => MenuManager.instance.OpenMenu("ExtraSettingsUI", SettingsMenu.HelpfulAdditionsCode)));
            Button.OnPointerDownEvent = new System.Action<PointerEventData>(p => IsPressed = true);
            Button.OnPointerUpEvent = new System.Action<PointerEventData>(p => IsPressed = false);
        }

        public void Init(TMP_FontAsset font) {
            Font = font;
        }

        void Update() {
            float delta = Time.deltaTime;

            Image.color = Button.targetGraphic.canvasRenderer.GetColor();

            if (IsPressed) {
                if (Scale == MinScale)
                    return;
                Scale -= ScaleSpeed * delta;
            } else {
                if (Scale == MaxScale)
                    return;
                Scale += ScaleSpeed * delta;
            }

            Scale = Math.Max(Math.Min(Scale, 1), .95f);

            ButtonTransform.localScale = new(Scale, Scale);
        }
#endif
    }
}
