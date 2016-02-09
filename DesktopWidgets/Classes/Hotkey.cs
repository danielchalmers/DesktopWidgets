using System;
using System.Windows.Input;

namespace DesktopWidgets.Classes
{
    internal class Hotkey
    {
        public Hotkey(Key key, ModifierKeys modifierKeys, bool worksIfForegroundIsFullscreen, bool worksIfMuted,
            Guid guid)
        {
            Key = key;
            ModifierKeys = modifierKeys;
            WorksIfForegroundIsFullscreen = worksIfForegroundIsFullscreen;
            WorksIfMuted = worksIfMuted;
            Guid = guid;
        }

        public Hotkey()
        {
        }

        public Guid Guid { get; set; } = Guid.NewGuid();
        public Key Key { get; set; } = Key.None;
        public ModifierKeys ModifierKeys { get; set; } = ModifierKeys.None;
        public bool WorksIfForegroundIsFullscreen { get; set; }
        public bool WorksIfMuted { get; set; }
    }
}