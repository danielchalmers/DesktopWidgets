using System.ComponentModel;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Events
{
    internal class HotkeyEvent : IEvent
    {
        [DisplayName("Hotkey Settings")]
        public Hotkey Hotkey { get; set; } = new Hotkey();
    }
}