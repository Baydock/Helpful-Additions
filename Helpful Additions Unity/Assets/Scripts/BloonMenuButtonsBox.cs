using UnityEngine;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
using Il2CppAssets.Scripts.Unity.UI_New;
using MelonLoader;
using UnityEngine.UI;
#endif

namespace HelpfulAdditions.MonoBehaviors {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
    [RegisterTypeInIl2Cpp(logSuccess: false)]
#endif
    public class BloonMenuButtonsBox : MonoBehaviour {
#if !(UNITY_EDITOR || UNITY_STANDALONE)
        void Start() {
            bool is4x3 = ScreenRatio.GetCurrent().ToRatioType() == ScreenRatioType.Ratio4_3;

            GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
            grid.constraint = is4x3 ? GridLayoutGroup.Constraint.FixedRowCount : GridLayoutGroup.Constraint.FixedColumnCount;
        }
#endif
    }
}
