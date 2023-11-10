using HarmonyLib;
using HelpfulAdditions.Properties;
using Il2CppAssets.Scripts.Unity.UI_New;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.BloonMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HelpfulAdditions {
    [HarmonyPatch]
    internal static class BloonMenuButtons {
        private static bool AnyButtonEnabled() => Settings.DeleteAllProjectilesOn;

        [HarmonyPatch(typeof(BloonMenu), nameof(BloonMenu.Initialise))]
        [HarmonyPrefix]
        public static void AddButtons(BloonMenu __instance) {
            if (AnyButtonEnabled()) {
                GameObject oldDestroyTowersButton = __instance.destroyTowersButton;

                GameObject newDestroyTowersButton = Object.Instantiate(oldDestroyTowersButton, __instance.transform);
                newDestroyTowersButton.name = oldDestroyTowersButton.name;
                newDestroyTowersButton.transform.SetSiblingIndex(1);
                __instance.destroyTowersButton = newDestroyTowersButton;
                __instance.btnDestroyMonkeys = newDestroyTowersButton.GetComponent<ButtonExtended>();

                GameObject buttonsContainer = oldDestroyTowersButton;
                buttonsContainer.name = "ButtonsContainer";
                Object.Destroy(buttonsContainer.GetComponent<Image>());
                Object.Destroy(buttonsContainer.GetComponent<CanvasRenderer>());
                Object.Destroy(buttonsContainer.GetComponent<ButtonExtended>());

                Transform bloonMenuButtonsBox = Object.Instantiate(AssetBundles.UiPrefabs.GetResource<GameObject>("BloonMenuButtonsBox"), buttonsContainer.transform).transform;

                if (Settings.DeleteAllProjectilesOn) {
                    Transform destroyProjectilesBtn = Object.Instantiate(AssetBundles.UiPrefabs.GetResource<GameObject>("DestroyProjectilesBtn"), bloonMenuButtonsBox).transform;
                    ButtonExtended dtbButton = destroyProjectilesBtn.GetComponent<ButtonExtended>();
                    dtbButton.OnPointerUpEvent = new System.Action<PointerEventData>(p => InGame.Bridge.DestroyAllProjectiles());
                }

                for (int i = 1; i < __instance.transform.childCount; i++) {
                    GameObject child = __instance.transform.GetChild(i).gameObject;
                    ButtonExtended childButton = child.GetComponent<ButtonExtended>();
                    if (childButton is not null) {
                        child.transform.parent = bloonMenuButtonsBox;
                        i--;
                    }
                }
            }
        }
    }
}
