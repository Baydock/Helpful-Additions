using Il2CppAssets.Scripts.Unity.UI_New.InGame;

namespace HelpfulAdditions.Utils {
    internal static class InGameUtils {
        public static bool InSandbox() => InGame.instance?.bridge?.simulation is not null && InGame.instance.bridge.simulation.sandbox;
    }
}
