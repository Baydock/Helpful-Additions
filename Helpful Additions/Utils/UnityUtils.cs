using UnityEngine;

namespace HelpfulAdditions.Utils {
    internal static class UnityUtils {
        public static void ClearChildren(this Transform transform) {
            foreach(Il2CppSystem.Object child in transform)
                Object.Destroy(child.Cast<Transform>().gameObject);
        }
    }
}
