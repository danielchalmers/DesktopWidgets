using System;
using System.Windows.Input;

namespace DesktopWidgets.Classes
{
    internal class Hotkey
    {
        public Hotkey(Key key, ModifierKeys modifierKeys, bool worksIfForegroundIsFullscreen)
        {
            Key = key;
            ModifierKeys = modifierKeys;
            WorksIfForegroundIsFullscreen = worksIfForegroundIsFullscreen;
        }

        private Guid Guid { get; set; } = Guid.NewGuid();
        public Key Key { get; set; }
        public ModifierKeys ModifierKeys { get; set; }
        public bool WorksIfForegroundIsFullscreen { get; set; }
    }
}