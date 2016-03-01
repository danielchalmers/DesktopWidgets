using System;
using System.ComponentModel;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Classes
{
    [ExpandableObject]
    [DisplayName("Hotkey")]
    public class Hotkey
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

        [Browsable(false)]
        [DisplayName("Guid")]
        public Guid Guid { get; set; } = Guid.NewGuid();

        [DisplayName("Key")]
        public Key Key { get; set; } = Key.None;

        [DisplayName("Modifier Keys")]
        public ModifierKeys ModifierKeys { get; set; } = ModifierKeys.None;

        [DisplayName("Allow Repetition")]
        public bool CanRepeat { get; set; } = true;

        [DisplayName("Works If Foreground Is Fullscreen")]
        public bool WorksIfForegroundIsFullscreen { get; set; }

        [DisplayName("Works If Muted")]
        public bool WorksIfMuted { get; set; }

        [DisplayName("Disabled")]
        public bool Disabled { get; set; } = false;
    }
}