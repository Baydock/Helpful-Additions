using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace HelpfulAdditions {
    public static class CustomBloons {

        private static readonly Dictionary<string, CustomBloon> customBloons = new();

        internal static bool IsRegistered(string id) => customBloons.ContainsKey(id);

        internal static CustomBloon Get(string id) => customBloons[id];

        private static void RegisterCustomBloon(string bloonId, CustomBloon customBloon) {
            if (!IsRegistered(bloonId))
                customBloons.Add(bloonId, customBloon);
            else
                customBloons[bloonId] = customBloon;
        }

        private static void AddCustomBloon(string bloonId, object icon, object edge, object span) {
            CustomBloon customBloon = new(icon, edge, span, false, null, null, null, null);
            RegisterCustomBloon(bloonId, customBloon);
        }

        // Must convert to byte[] in order for the Il2Cpp side to not garbage collect it
        public static void AddCustomBloon(string bloonId, Texture2D icon, Texture2D edge, Texture2D span) =>
            AddCustomBloon(bloonId, ImageConversion.EncodeToPNG(icon), ImageConversion.EncodeToPNG(edge), ImageConversion.EncodeToPNG(span));

        public static void AddCustomBloon(string bloonId, Sprite icon, Sprite edge, Sprite span) => 
            AddCustomBloon(bloonId, GetBoundTexture(icon), GetBoundTexture(edge), GetBoundTexture(span));

        public static void AddCustomBloon(string bloonId, string icon, string edge, string span, AssetBundle bundle, string iconSpriteSheet, string edgeSpriteSheet, string spanSpriteSheet) {
            CustomBloon customBloon = new(icon, edge, span, true, bundle, iconSpriteSheet, edgeSpriteSheet, spanSpriteSheet);
            RegisterCustomBloon(bloonId, customBloon);
        }

        // For spritesheets when using the sprite param overload
        private static Texture2D GetBoundTexture(Sprite sprite) {
            Texture2D tex = sprite.texture;
            // Check if fits whole texture
            if (sprite.rect.x == 0 && sprite.rect.y == 0 && sprite.rect.width == tex.width && sprite.rect.height == tex.height)
                return tex;

            Texture2D boundTex = new((int)sprite.rect.width, (int)sprite.rect.height);

            if (SystemInfo.copyTextureSupport.HasFlag(CopyTextureSupport.Basic))
                Graphics.CopyTexture(tex, 0, 0, (int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height, boundTex, 0, 0, 0, 0);
            else
                boundTex.SetPixels(tex.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height));
            return boundTex;
        }
    }

    internal sealed record class CustomBloon(object Icon, object Edge, object Span, bool UsesAssetBundle, AssetBundle Bundle, string IconSpriteSheet, string EdgeSpriteSheet, string SpanSpriteSheet);
}
