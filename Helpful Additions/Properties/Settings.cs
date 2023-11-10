using MelonLoader;

namespace HelpfulAdditions.Properties {
    internal static partial class Settings {
        private static MelonPreferences_Category Category { get; } = MelonPreferences.CreateCategory("HelpfulAdditionsSettings");


        public static BoolEntry PowersInSandboxOn { get; } = new(nameof(PowersInSandboxOn), true);

        public static BoolEntry BlonsInEditorOn { get; } = new(nameof(BlonsInEditorOn), true);

        public static BoolEntry DeleteAllProjectilesOn { get; } = new(nameof(DeleteAllProjectilesOn), true);

        public static BoolEntry RoundSetSwitcherOn { get; } = new(nameof(RoundSetSwitcherOn), true);

        public static BoolEntry SinglePlayerCoopOn { get; } = new(nameof(SinglePlayerCoopOn), true);

        public static BoolEntry RoundInfoScreenOn { get; } = new(nameof(RoundInfoScreenOn), true);

        public static BoolEntry ShowBloonIdsOn { get; } = new(nameof(ShowBloonIdsOn), false);

        public static BoolEntry ShowBossBloonsOn { get; } = new(nameof(ShowBossBloonsOn), true);

        public static BoolEntry SkipStartScreen { get; } = new(nameof(SkipStartScreen), false);

        public class Entry<T> {
            private readonly MelonPreferences_Entry<T> entry;

            public Entry(string name, T defaultValue) => entry = Category.CreateEntry(name, defaultValue);

            public T Value {
                get => entry.Value;
                set {
                    entry.Value = value;
                    Category.SaveToFile(false);
                }
            }
        }

        public class BoolEntry : Entry<bool> {
            public BoolEntry(string name, bool defaultValue) : base(name, defaultValue) { }

            public static implicit operator bool(BoolEntry entry) => entry.Value;
        }
    }
}
