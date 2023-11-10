using HarmonyLib;
using HelpfulAdditions.Properties;
using HelpfulAdditions.Utils;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.BloonMenu;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.RightMenu;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.RightMenu.Powers;
using UnityEngine;
using UnityEngine.UI;

namespace HelpfulAdditions {
    [HarmonyPatch]
    internal static class AddPowersToSandbox {
        private static bool IsSandboxAndPowersOn() => InGameUtils.InSandbox() && Settings.PowersInSandboxOn;

        [HarmonyPatch(typeof(RightMenu), nameof(RightMenu.SetPowersInteractable))]
        [HarmonyPrefix]
        public static bool MakePowersAvailable(ref bool interactable) {
            if (IsSandboxAndPowersOn())
                interactable = true;
            return true;
        }

        [HarmonyPatch(typeof(BloonMenu), nameof(BloonMenu.Initialise))]
        [HarmonyPostfix]
        public static void SetInitialState() {
            if (IsSandboxAndPowersOn()) {
                SetBloonMenuToggleVisibility(false);
                ShopMenu.instance.powersButton.SetActive(true);
            }
        }

        [HarmonyPatch(typeof(BloonMenuToggle), nameof(BloonMenuToggle.ToggleBloonMenu))]
        [HarmonyPostfix]
        public static void ShowBloonMenu() {
            if (IsSandboxAndPowersOn() && InGame.instance.BloonMenu.showInternal) {
                SetBloonMenuToggleVisibility(false);
                ShopMenu.instance.powersButton.SetActive(true);
            }
        }

        [HarmonyPatch(typeof(RightMenu), nameof(RightMenu.ShowPowersMenu))]
        [HarmonyPostfix]
        public static void ShowPowersMenu() {
            if (IsSandboxAndPowersOn()) {
                InGame.instance.BloonMenu.showInternal = false;
                SetBloonMenuToggleVisibility(false);
                ShopMenu.instance.powersButton.SetActive(true);
                powerIsBeingUsed = false;
            }
        }

        [HarmonyPatch(typeof(RightMenu), nameof(RightMenu.HidePowersMenu))]
        [HarmonyPostfix]
        public static void ShowTowerMenu() {
            if (IsSandboxAndPowersOn() && !powerIsBeingUsed) {
                SetBloonMenuToggleVisibility(true);
                ShopMenu.instance.powersButton.SetActive(false);
            }
        }

        [HarmonyPatch(typeof(BloonMenuToggle), nameof(BloonMenuToggle.Update))]
        [HarmonyPrefix]
        // Disable updates so I can control the animations (that's all the update does)
        public static bool DisableBloonMenuToggleUpdate() => !IsSandboxAndPowersOn();

        [HarmonyPatch(typeof(PowersMenu), nameof(PowersMenu.Initialise))]
        [HarmonyPrefix]
        public static void InitialiseSandboxPowersMenuPrefix(PowersMenu __instance) {
            if (IsSandboxAndPowersOn()) {
                __instance.instasDisabled = true;
                __instance.dontUsePlayerInventory = true;
            }
        }

        [HarmonyPatch(typeof(ReadonlyInGameData), nameof(ReadonlyInGameData.ArePowersAllowed))]
        [HarmonyPostfix]
        public static void AllowPowersInSandbox(ref bool __result) {
            if (IsSandboxAndPowersOn())
                __result = true;
        }

        private static bool powerIsBeingUsed = false;
        [HarmonyPatch(typeof(PowersMenu), nameof(PowersMenu.StartPowerPlacement))]
        [HarmonyPostfix]
        public static void WhenPowerIsBeingUsed() {
            if (IsSandboxAndPowersOn())
                powerIsBeingUsed = true;
        }

        [HarmonyPatch(typeof(StandardPowerButton), nameof(StandardPowerButton.UpdatePowerDisplay))]
        [HarmonyPostfix]
        public static void DontShowPowerCountIcon(StandardPowerButton __instance) {
            if (IsSandboxAndPowersOn())
                __instance.powerCountIcon.SetActive(false);
        }

        [HarmonyPatch(typeof(StandardPowerButton), nameof(StandardPowerButton.UpdateUseCount))]
        [HarmonyPostfix]
        public static void DontShowPowerCountText(StandardPowerButton __instance) {
            if (IsSandboxAndPowersOn())
                __instance.powerCountText.gameObject.SetActive(false);
        }

        [HarmonyPatch(typeof(StandardPowerButton), nameof(StandardPowerButton.GetUseCount))]
        [HarmonyPostfix]
        public static void ShowHasPowerInSandbox(ref int __result) {
            if (IsSandboxAndPowersOn())
                __result = 1;
        }

        [HarmonyPatch(typeof(PowerButton), nameof(PowerButton.IsAvailable))]
        [HarmonyPostfix]
        public static void AllowNotAvailablePowerInSandbox(ref bool __result) {
            if (IsSandboxAndPowersOn())
                __result = true;
        }

        [HarmonyPatch(typeof(PowersMenu), nameof(PowersMenu.PowerUseSuccess))]
        [HarmonyPrefix]
        public static bool DontLosePowers() {
            // If in sandbox, don't allow for player profile to change when power used
            return !IsSandboxAndPowersOn();
        }

        private static void SetBloonMenuToggleVisibility(bool visible) {
            GameObject bmtGameObject = ShopMenu.instance.bloonsButton;
            BloonMenuToggle bmt = bmtGameObject.GetComponent<BloonMenuToggle>();
            bmtGameObject.GetComponentInChildren<Image>().enabled = visible;
            bmt.bloonsButtonAnimator.SetInteger(bmt.visibleStateLabel, visible ? BloonMenuToggle.animationStateClosed : BloonMenuToggle.animationStateOpen);
        }
    }
}
