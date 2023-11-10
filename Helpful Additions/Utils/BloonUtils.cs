using HelpfulAdditions.Properties;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons;
using System;
using System.Linq;
using UnityEngine;
using Resources = HelpfulAdditions.Properties.Resources;

namespace HelpfulAdditions.Utils {
    internal static class BloonUtils {
        private static string[] NormalBloonBaseTypes { get; } = {
            "Red",
            "Blue",
            "Green",
            "Yellow",
            "Pink",
            "Purple",
            "Black",
            "White",
            "Lead",
            "Zebra",
            "Rainbow",
            "Ceramic"
        };

        private static string[] MoabBloonBaseTypes { get; } = {
            "Moab",
            "Bfb",
            "Zomg",
            "Ddt",
            "Bad"
        };

        private static string[] BossBloonBaseTypes { get; } = {
            "Bloonarius",
            "Lych",
            "Vortex",
            "Dreadbloon",
        };

        public static bool GetBloonSprites(BloonModel bloon, out Sprite icon, out Sprite edge, out Sprite span) {
            if (bloon is null) {
                icon = Sprites.UnknownBloon;
                edge = Sprites.UnknownEdge;
                span = Sprites.UnknownSpan;
                return false;
            }

            if (CustomBloons.IsRegistered(bloon.id)) {
                CustomBloon customBloon = CustomBloons.Get(bloon.id);
                if (customBloon.UsesAssetBundle) {
                    icon = customBloon.Bundle.GetResource<Sprite>(customBloon.Icon as string, customBloon.IconSpriteSheet);
                    edge = customBloon.Bundle.GetResource<Sprite>(customBloon.Edge as string, customBloon.EdgeSpriteSheet);
                    span = customBloon.Bundle.GetResource<Sprite>(customBloon.Span as string, customBloon.SpanSpriteSheet);
                } else {
                    icon = Resources.LoadSprite(customBloon.Icon as byte[]);
                    edge = Resources.LoadSprite(customBloon.Edge as byte[]);
                    span = Resources.LoadSprite(customBloon.Span as byte[]);
                }
                return true;
            }

            string path = null, basePath = null;
            if (bloon.baseId.StartsWith("TestBloon"))
                GetBloonPath(bloon, GetTestBloonPath, out path, out basePath);
            else if (bloon.baseId.StartsWith("Golden"))
                GetBloonPath(bloon, GetGoldenType, out path, out basePath);
            else if (bloon.baseId.StartsWith("DreadRockBloon"))
                GetBloonPath(bloon, GetRockBloonPath, out path, out basePath);
            else if (NormalBloonBaseTypes.Contains(bloon.baseId))
                GetBloonPath(bloon, GetNormalBloonType, out path, out basePath);
            else if (MoabBloonBaseTypes.Contains(bloon.baseId))
                GetBloonPath(bloon, GetMoabBloonType, out path, out basePath);
            else if (BossBloonBaseTypes.Contains(bloon.baseId))
                GetBloonPath(bloon, GetBossBloonType, out path, out basePath);

            if (path is not null) {
                icon = AssetBundles.Bloons.GetResource<Sprite>(path);
                edge = AssetBundles.Bloons.GetResource<Sprite>($"{basePath}Edge");
                span = AssetBundles.Bloons.GetResource<Sprite>($"{basePath}Span");
                return true;
            }

            icon = Sprites.UnknownBloon;
            edge = Sprites.UnknownEdge;
            span = Sprites.UnknownSpan;
            return false;
        }

        private delegate string TypeDelegate(BloonModel bloon, out string newBaseId, out bool overrideBasePath);
        private static void GetBloonPath(BloonModel bloon, TypeDelegate getType, out string path, out string basePath) {
            string bloonType = getType(bloon, out string newBaseId, out bool overrideBasePath);
            string baseId = newBaseId ?? bloon.baseId;
            basePath = "";
            path = bloonType;
            if (overrideBasePath)
                basePath = bloonType;
            basePath += baseId;
            path += baseId;
        }

        public static int GetBossTier(BloonModel bloon) {
            // distance from char 0 gets digit value
            return bloon.id[^1] - '0';
        }

        private static string GetNormalBloonType(BloonModel bloon, out string newBaseId, out bool overrideBasePath) {
            newBaseId = null;
            overrideBasePath = false;
            string bloonType = "";
            if (bloon.isGrow)
                bloonType = "Regrow" + bloonType;
            if (bloon.isCamo)
                bloonType = "Camo" + bloonType;
            if (bloon.isFortified)
                bloonType = "Fortified" + bloonType;
            return bloonType;
        }

        private static string GetMoabBloonType(BloonModel moab, out string newBaseId, out bool overrideBasePath) {
            newBaseId = null;
            overrideBasePath = false;
            if (moab.isFortified)
                return "Fortified";
            return "";
        }

        private static string GetBossBloonType(BloonModel boss, out string newBaseId, out bool overrideBasePath) {
            newBaseId = null;
            overrideBasePath = true;
            if (boss.id.Contains("Elite"))
                return "Elite";
            return "";
        }

        private static string GetGoldenType(BloonModel golden, out string newBaseId, out bool overrideBasePath) {
            newBaseId = "Golden";
            overrideBasePath = false;
            string bloonType = "";
            if (golden.bloonProperties.HasFlag(BloonProperties.Black) && golden.bloonProperties.HasFlag(BloonProperties.White))
                bloonType = "Zebra" + bloonType;
            if (golden.bloonProperties.HasFlag(BloonProperties.Purple))
                bloonType = "Purple" + bloonType;
            if (golden.bloonProperties.HasFlag(BloonProperties.Lead))
                bloonType = "Lead" + bloonType;
            if (golden.isCamo)
                bloonType = "Camo" + bloonType;
            if (golden.isFortified)
                bloonType = "Fortified" + bloonType;
            return bloonType;
        }

        private static string GetRockBloonPath(BloonModel rockBloon, out string newBaseId, out bool overrideBasePath) {
            newBaseId = "DreadRock";
            overrideBasePath = true;
            if (rockBloon.id.Contains("Elite"))
                return "Elite";
            return "";
        }

        private static string GetTestBloonPath(BloonModel testBloon, out string newBaseId, out bool overrideBasePath) {
            newBaseId = null;
            overrideBasePath = false;
            if (testBloon.isFortified)
                return "Fortified";
            return "";
        }
    }
}
