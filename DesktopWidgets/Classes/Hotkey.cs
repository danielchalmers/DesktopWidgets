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

        public Key Key { get; set; }
        public ModifierKeys ModifierKeys { get; set; }
        public bool WorksIfForegroundIsFullscreen { get; set; }
    }
}