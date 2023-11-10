using Il2CppInterop.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace HelpfulAdditions.Properties {
    internal static class Resources {
        private static readonly Assembly thisAssembly = Assembly.GetExecutingAssembly();
        private static readonly string assemblyName = thisAssembly.GetName().Name.Replace(" ", "");
        private static readonly string[] resourceNames = thisAssembly.GetManifestResourceNames();

        private static Dictionary<string, AssetBundle> AssetBundles { get; } = new();

        private static string ToFull(string resourceName) => $"{assemblyName}.Resources.{resourceName}";

        private static byte[] GetResource(string resourceName) {
            string fullName = ToFull(resourceName);

            if (!resourceNames.Contains(fullName))
                return null;

            using MemoryStream resourceStream = new();
            try {
                thisAssembly.GetManifestResourceStream(fullName).CopyTo(resourceStream);
                return resourceStream.ToArray();
            } catch {
                return null;
            }
        }

        public static AssetBundle GetAssetBundle(string resourceName) {
            if (AssetBundles.TryGetValue(resourceName, out AssetBundle bundle))
                return bundle;

            byte[] data = GetResource(resourceName);
            if (data is null)
                return null;

            bundle = AssetBundle.LoadFromMemory(data);
            AssetBundles[resourceName] = bundle;
            return bundle;
        }

        public static T GetResource<T>(this AssetBundle bundle, string resourceName, string subAssetParent = null) where T : Object {
            Object resource = null;
            if (subAssetParent is null)
                resource = bundle.LoadAsset(resourceName, Il2CppType.Of<T>());
            else {
                Object[] subAssets = bundle.LoadAssetWithSubAssets(subAssetParent);
                foreach(Object subAsset in subAssets) {
                    if (subAsset.GetIl2CppType().Equals(Il2CppType.Of<T>()) && subAsset.name.Equals(resourceName))
                        resource = subAsset;
                }
            }
            if (resource is null)
                return null;
            return resource.Cast<T>();
        }

        private static Texture2D LoadTextureFromBytes(byte[] data) {
            if (data is null)
                return null;

            Texture2D tex = new(0, 0) { wrapMode = TextureWrapMode.Clamp, mipMapBias = -1 };
            bool success = ImageConversion.LoadImage(tex, data);

            if (!success)
                return null;

            return tex;
        }

        private static Sprite LoadSpriteFromTexture(Texture2D tex) {
            if (tex is null)
                return null;

            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }

        public static Sprite LoadSprite(byte[] data) {
            Texture2D tex = LoadTextureFromBytes(data);
            return LoadSpriteFromTexture(tex);
        }
    }
}
