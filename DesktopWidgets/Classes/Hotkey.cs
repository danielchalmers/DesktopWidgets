using System;
using System.Windows.Input;

namespace DesktopWidgets.Classes
{
    internal class Hotkey
    {
        public Hotkey(Key key, ModifierKeys modifierKeys, bool worksIfForegroundIsFullscreen, bool worksIfMuted)
        {
            Key = key;
            ModifierKeys = modifierKeys;
            WorksIfForegroundIsFullscreen = worksIfForegroundIsFullscreen;
            WorksIfMuted = worksIfMuted;
            Guid = Guid.NewGuid();
        }

        public Hotkey(Key key, ModifierKeys modifierKeys, bool worksIfForegroundIsFullscreen, bool worksIfMuted,
            Guid guid)
        {
            Key = key;
            ModifierKeys = modifierKeys;
            WorksIfForegroundIsFullscreen = worksIfForegroundIsFullscreen;
            WorksIfMuted = worksIfMuted;
            Guid = guid;
        }

        public Guid Guid { get; set; }
        public Key Key { get; set; }
        public ModifierKeys ModifierKeys { get; set; }
        public bool WorksIfForegroundIsFullscreen { get; set; }
        public bool WorksIfMuted { get; set; }
    }
}